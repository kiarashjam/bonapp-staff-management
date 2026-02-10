using StaffPro.Application.DTOs;

namespace StaffPro.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    Task InviteUserAsync(Guid orgId, InviteUserRequest request, string invitedBy);
    Task AcceptInviteAsync(AcceptInviteRequest request);
    Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    Task<UserDto> GetCurrentUserAsync(Guid userId);
}

public interface IConflictDetectionService
{
    Task<ConflictCheckResult> CheckAssignmentAsync(
        Guid orgId, Guid employeeId, DateOnly date, TimeOnly startTime, TimeOnly endTime,
        Guid? excludeAssignmentId = null, CancellationToken ct = default);
}

public interface ITimesheetGenerationService
{
    Task<TimesheetDto> GenerateForEmployeeAsync(
        Guid employeeId, DateOnly periodStart, DateOnly periodEnd, CancellationToken ct = default);
    Task GenerateAllForPeriodAsync(
        Guid orgId, DateOnly periodStart, DateOnly periodEnd, CancellationToken ct = default);
}

public interface IPayrollExportService
{
    Task<byte[]> ExportToCsvAsync(Guid orgId, DateOnly periodStart, DateOnly periodEnd, CancellationToken ct = default);
    Task<IReadOnlyList<PayrollExportDto>> GetPayrollSummaryAsync(Guid orgId, DateOnly periodStart, DateOnly periodEnd, CancellationToken ct = default);
}

public interface INotificationService
{
    Task SendAsync(Guid orgId, Guid recipientUserId, string title, string message, string type, string? actionUrl = null);
    Task SendToManyAsync(Guid orgId, IEnumerable<Guid> recipientUserIds, string title, string message, string type, string? actionUrl = null);
    Task SendToAllInOrgAsync(Guid orgId, string title, string message, string type, string? actionUrl = null);
}

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid OrganizationId { get; }
    string Email { get; }
    string Role { get; }
    Guid? EmployeeId { get; }
}
