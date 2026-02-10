using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StaffPro.Application.DTOs;
using StaffPro.Application.Interfaces;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly StaffProDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(UserManager<ApplicationUser> userManager, StaffProDbContext db, IConfiguration config)
    {
        _userManager = userManager;
        _db = db;
        _config = config;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated");

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!valid) throw new UnauthorizedAccessException("Invalid email or password");

        user.LastLoginAt = DateTime.UtcNow;
        var (accessToken, expiresAt) = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        var org = await _db.Organizations.FindAsync(user.OrganizationId);
        return new LoginResponse(accessToken, refreshToken, expiresAt,
            new UserDto(user.Id, user.Email!, user.FirstName, user.LastName,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Employee",
                user.OrganizationId, org?.Name ?? "", user.EmployeeId));
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        // Create organization
        var org = new Organization { Name = request.OrganizationName };
        await _db.Organizations.AddAsync(org);
        await _db.SaveChangesAsync();

        // Create default leave types
        await SeedDefaultLeaveTypes(org.Id);

        // Create user
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            OrganizationId = org.Id,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "Admin");

        var (accessToken, expiresAt) = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new LoginResponse(accessToken, refreshToken, expiresAt,
            new UserDto(user.Id, user.Email!, user.FirstName, user.LastName,
                "Admin", org.Id, org.Name, null));
    }

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var (newAccessToken, expiresAt) = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        var org = await _db.Organizations.FindAsync(user.OrganizationId);
        return new LoginResponse(newAccessToken, newRefreshToken, expiresAt,
            new UserDto(user.Id, user.Email!, user.FirstName, user.LastName,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Employee",
                user.OrganizationId, org?.Name ?? "", user.EmployeeId));
    }

    public async Task InviteUserAsync(Guid orgId, InviteUserRequest request, string invitedBy)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            OrganizationId = orgId,
            EmployeeId = request.EmployeeId,
            EmailConfirmed = true
        };

        var tempPassword = GenerateRefreshToken()[..12] + "A1!";
        var result = await _userManager.CreateAsync(user, tempPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, request.Role);
        // TODO: Send invite email with temp password via SendGrid
    }

    public async Task AcceptInviteAsync(AcceptInviteRequest request)
    {
        // TODO: Implement token-based invite acceptance
        await Task.CompletedTask;
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new InvalidOperationException("User not found");

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<UserDto> GetCurrentUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new InvalidOperationException("User not found");
        var org = await _db.Organizations.FindAsync(user.OrganizationId);
        var roles = await _userManager.GetRolesAsync(user);
        return new UserDto(user.Id, user.Email!, user.FirstName, user.LastName,
            roles.FirstOrDefault() ?? "Employee", user.OrganizationId, org?.Name ?? "", user.EmployeeId);
    }

    private (string Token, DateTime ExpiresAt) GenerateJwtToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddHours(2);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new("organizationId", user.OrganizationId.ToString()),
            new("employeeId", user.EmployeeId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task SeedDefaultLeaveTypes(Guid orgId)
    {
        var leaveTypes = new List<LeaveType>
        {
            new() { OrganizationId = orgId, Name = "Vacation", IsPaid = true, MaxDaysPerYear = 25, AccrualRatePerMonth = 2.08m, Color = "#3B82F6" },
            new() { OrganizationId = orgId, Name = "Sick Leave", IsPaid = true, RequiresDocument = true, Color = "#EF4444" },
            new() { OrganizationId = orgId, Name = "Personal Day", IsPaid = true, MaxDaysPerYear = 3, Color = "#8B5CF6" },
            new() { OrganizationId = orgId, Name = "Unpaid Leave", IsPaid = false, Color = "#6B7280" },
        };

        await _db.LeaveTypes.AddRangeAsync(leaveTypes);
        await _db.SaveChangesAsync();
    }
}
