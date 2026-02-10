namespace StaffPro.Application.DTOs;

public record LoginRequest(string Email, string Password);
public record LoginResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);
public record RegisterRequest(string Email, string Password, string FirstName, string LastName, string OrganizationName);
public record RefreshTokenRequest(string RefreshToken);
public record InviteUserRequest(string Email, string FirstName, string LastName, string Role, Guid? EmployeeId);
public record AcceptInviteRequest(string Token, string Password);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public record UserDto(
    Guid Id, string Email, string FirstName, string LastName,
    string Role, Guid OrganizationId, string OrganizationName,
    Guid? EmployeeId);
