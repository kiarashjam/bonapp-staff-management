using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffPro.Application.DTOs;
using StaffPro.Application.Interfaces;

namespace StaffPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        => Ok(await _authService.LoginAsync(request));

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register(RegisterRequest request)
        => Ok(await _authService.RegisterAsync(request));

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> RefreshToken(RefreshTokenRequest request)
        => Ok(await _authService.RefreshTokenAsync(request.RefreshToken));

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpPost("invite")]
    public async Task<IActionResult> InviteUser(InviteUserRequest request)
    {
        var orgId = Guid.Parse(User.FindFirstValue("organizationId")!);
        await _authService.InviteUserAsync(orgId, request, User.FindFirstValue(ClaimTypes.Email)!);
        return Ok(new { message = "Invitation sent" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _authService.GetCurrentUserAsync(userId));
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _authService.ChangePasswordAsync(userId, request);
        return Ok(new { message = "Password changed" });
    }
}
