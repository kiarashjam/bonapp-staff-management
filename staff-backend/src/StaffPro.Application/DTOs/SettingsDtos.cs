namespace StaffPro.Application.DTOs;

public record OrganizationDto(
    Guid Id, string Name, string? LogoUrl, string? Phone, string? Email,
    string? Address, string? City, string? Country, string? PostalCode,
    string Timezone, string Currency, string DefaultLanguage);

public record UpdateOrganizationRequest(
    string Name, string? Phone, string? Email, string? Address,
    string? City, string? Country, string? PostalCode,
    string Timezone, string Currency, string DefaultLanguage);

public record SchedulingRulesDto(
    int MinRestHoursBetweenShifts, int MaxConsecutiveWorkDays, int MaxHoursPerWeek,
    int MaxHoursPerDay, int BreakAfterMinutes, int BreakDurationMinutes,
    int ClockInGraceMinutes, int ClockInLateThresholdMinutes,
    int ClockRoundingMinutes, int OvertimeWeeklyThreshold,
    int OvertimeDailyThreshold, decimal OvertimeMultiplier);

public record UpdateSchedulingRulesRequest(
    int MinRestHoursBetweenShifts, int MaxConsecutiveWorkDays, int MaxHoursPerWeek,
    int MaxHoursPerDay, int BreakAfterMinutes, int BreakDurationMinutes,
    int ClockInGraceMinutes, int ClockInLateThresholdMinutes,
    int ClockRoundingMinutes, int OvertimeWeeklyThreshold,
    int OvertimeDailyThreshold, decimal OvertimeMultiplier);

public record RoleDto(Guid Id, string Name, string? Description, decimal DefaultHourlyRate, string Color, Guid? DepartmentId, string? DepartmentName);
public record CreateRoleRequest(string Name, string? Description, decimal DefaultHourlyRate, string Color, Guid? DepartmentId);
public record UpdateRoleRequest(string Name, string? Description, decimal DefaultHourlyRate, string Color, Guid? DepartmentId);

public record StationDto(Guid Id, string Name, string? Description, int MaxCapacity, Guid? LocationId, string? LocationName, IReadOnlyList<Guid> RoleIds);
public record CreateStationRequest(string Name, string? Description, int MaxCapacity, Guid? LocationId, IReadOnlyList<Guid>? RoleIds);
public record UpdateStationRequest(string Name, string? Description, int MaxCapacity, Guid? LocationId, IReadOnlyList<Guid>? RoleIds);

public record DepartmentDto(Guid Id, string Name, string? Description, string Color);
public record CreateDepartmentRequest(string Name, string? Description, string Color);

public record LocationDto(Guid Id, string Name, string? Address, string? City, string? PostalCode, string? Phone, double? Latitude, double? Longitude, int GeofenceRadiusMeters, bool IsActive);
public record CreateLocationRequest(string Name, string? Address, string? City, string? PostalCode, string? Phone, double? Latitude, double? Longitude, int GeofenceRadiusMeters);

public record ShiftTemplateDto(Guid Id, string Name, TimeOnly StartTime, TimeOnly EndTime, int BreakDurationMinutes, bool BreakIsPaid, string Color, double TotalHours, double NetHours, bool IsActive);
public record CreateShiftTemplateRequest(string Name, TimeOnly StartTime, TimeOnly EndTime, int BreakDurationMinutes, bool BreakIsPaid, string Color);

public record LeaveTypeDto(Guid Id, string Name, bool IsPaid, bool RequiresDocument, int? MaxDaysPerYear, decimal? AccrualRatePerMonth, int MaxCarryOverDays, string Color);
public record CreateLeaveTypeRequest(string Name, bool IsPaid, bool RequiresDocument, int? MaxDaysPerYear, decimal? AccrualRatePerMonth, int MaxCarryOverDays, string Color);

public record NotificationDto(Guid Id, string Title, string Message, string Type, string? ActionUrl, bool IsRead, DateTime CreatedAt);
public record AnnouncementDto(Guid Id, string Title, string Content, bool IsPinned, string PostedBy, DateTime CreatedAt, DateTime? ExpiresAt, int ReadCount, int TotalRecipients);
public record CreateAnnouncementRequest(string Title, string Content, bool IsPinned, string TargetType, Guid? TargetId, DateTime? ExpiresAt);

public record AvailabilityDto(Guid Id, DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime, string Type);
public record CreateAvailabilityRequest(DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime, string Type);
public record AvailabilityOverrideDto(Guid Id, DateOnly Date, TimeOnly? StartTime, TimeOnly? EndTime, string Type, string? Note);
public record CreateAvailabilityOverrideRequest(DateOnly Date, TimeOnly? StartTime, TimeOnly? EndTime, string Type, string? Note);

public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize, int TotalPages);
