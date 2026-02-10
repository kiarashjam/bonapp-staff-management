using StaffPro.Domain.Enums;

namespace StaffPro.Application.DTOs;

public record TimeOffRequestDto(
    Guid Id, Guid EmployeeId, string EmployeeName, Guid LeaveTypeId,
    string LeaveTypeName, string LeaveTypeColor, DateOnly StartDate,
    DateOnly EndDate, int TotalDays, string? Reason,
    TimeOffRequestStatus Status, DateTime CreatedAt,
    string? ReviewedBy, DateTime? ReviewedAt, string? DenialReason);

public record CreateTimeOffRequest(
    Guid EmployeeId, Guid LeaveTypeId, DateOnly StartDate, DateOnly EndDate,
    TimeOnly? StartTime, TimeOnly? EndTime, string? Reason);

public record ReviewTimeOffRequest(bool Approved, string? DenialReason);

public record StaffingImpactDto(
    DateOnly Date, int ScheduledStaff, int AlreadyOnLeave,
    int MinRequirement, bool MeetsMinimum);

public record LeaveBalanceDto(
    Guid Id, Guid LeaveTypeId, string LeaveTypeName, int Year,
    decimal Entitled, decimal Used, decimal CarriedOver,
    decimal Adjustment, decimal Remaining);
