using StaffPro.Domain.Enums;

namespace StaffPro.Application.DTOs;

public record SchedulePeriodDto(
    Guid Id, Guid LocationId, string LocationName, DateOnly StartDate,
    DateOnly EndDate, SchedulePeriodStatus Status, DateTime? PublishedAt,
    int TotalAssignments, int TotalEmployees);

public record SchedulePeriodDetailDto(
    Guid Id, Guid LocationId, string LocationName, DateOnly StartDate,
    DateOnly EndDate, SchedulePeriodStatus Status, DateTime? PublishedAt,
    string? Notes, IReadOnlyList<ShiftAssignmentDto> Assignments,
    IReadOnlyList<StaffingRequirementDto> StaffingRequirements);

public record CreateSchedulePeriodRequest(Guid LocationId, DateOnly StartDate, DateOnly EndDate, string? Notes);
public record PublishScheduleRequest(Guid SchedulePeriodId);

public record ShiftAssignmentDto(
    Guid Id, Guid EmployeeId, string EmployeeName, string? EmployeePhotoUrl,
    Guid? ShiftTemplateId, string? ShiftTemplateName, Guid? StationId,
    string? StationName, Guid? RoleId, string? RoleName, string? RoleColor,
    DateOnly Date, TimeOnly StartTime, TimeOnly EndTime,
    int BreakDurationMinutes, ShiftAssignmentStatus Status, double NetHours, string? Notes);

public record CreateShiftAssignmentRequest(
    Guid SchedulePeriodId, Guid EmployeeId, Guid? ShiftTemplateId,
    Guid? StationId, Guid? RoleId, DateOnly Date,
    TimeOnly StartTime, TimeOnly EndTime,
    int BreakDurationMinutes, bool BreakIsPaid, string? Notes);

public record UpdateShiftAssignmentRequest(
    Guid? ShiftTemplateId, Guid? StationId, Guid? RoleId,
    TimeOnly StartTime, TimeOnly EndTime,
    int BreakDurationMinutes, string? Notes);

public record CopyScheduleRequest(Guid SourceSchedulePeriodId, DateOnly NewStartDate);

public record StaffingRequirementDto(
    Guid Id, Guid RoleId, string RoleName, DayOfWeek DayOfWeek,
    Guid? ShiftTemplateId, string? ShiftTemplateName, int MinStaff, int MaxStaff);

public record CreateStaffingRequirementRequest(
    Guid SchedulePeriodId, Guid RoleId, DayOfWeek DayOfWeek,
    Guid? ShiftTemplateId, int MinStaff, int MaxStaff);

public record ConflictDto(string Code, string Message, ConflictSeverity Severity, Guid? EmployeeId);
public record ConflictCheckResult(bool HasErrors, IReadOnlyList<ConflictDto> Conflicts);
