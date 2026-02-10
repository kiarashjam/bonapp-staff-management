using MediatR;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Queries;

// Employees
public record GetEmployeesQuery(Guid OrgId, int Page = 1, int PageSize = 20, string? Search = null, Guid? LocationId = null, Guid? RoleId = null) : IRequest<PagedResult<EmployeeListDto>>;
public record GetEmployeeByIdQuery(Guid OrgId, Guid EmployeeId) : IRequest<EmployeeDetailDto>;

// Schedule
public record GetSchedulePeriodsQuery(Guid OrgId, Guid? LocationId = null, int Page = 1, int PageSize = 20) : IRequest<PagedResult<SchedulePeriodDto>>;
public record GetSchedulePeriodDetailQuery(Guid OrgId, Guid SchedulePeriodId) : IRequest<SchedulePeriodDetailDto>;
public record GetAssignmentsForDateRangeQuery(Guid OrgId, DateOnly Start, DateOnly End, Guid? EmployeeId = null) : IRequest<IReadOnlyList<ShiftAssignmentDto>>;
public record CheckConflictsQuery(Guid OrgId, Guid EmployeeId, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime, Guid? ExcludeAssignmentId = null) : IRequest<ConflictCheckResult>;

// Time Off
public record GetTimeOffRequestsQuery(Guid OrgId, int Page = 1, int PageSize = 20, Guid? EmployeeId = null, string? Status = null) : IRequest<PagedResult<TimeOffRequestDto>>;
public record GetTimeOffRequestByIdQuery(Guid OrgId, Guid RequestId) : IRequest<TimeOffRequestDto>;
public record GetStaffingImpactQuery(Guid OrgId, DateOnly StartDate, DateOnly EndDate) : IRequest<IReadOnlyList<StaffingImpactDto>>;
public record GetLeaveBalancesQuery(Guid OrgId, Guid EmployeeId, int? Year = null) : IRequest<IReadOnlyList<LeaveBalanceDto>>;

// Clock
public record GetClockStatusQuery(Guid OrgId, Guid EmployeeId) : IRequest<ClockStatusDto>;
public record GetClockEntriesQuery(Guid OrgId, Guid EmployeeId, DateOnly Date) : IRequest<IReadOnlyList<ClockEntryDto>>;

// Timesheets
public record GetTimesheetsQuery(Guid OrgId, int Page = 1, int PageSize = 20, Guid? EmployeeId = null, string? Status = null) : IRequest<PagedResult<TimesheetDto>>;
public record GetTimesheetDetailQuery(Guid OrgId, Guid TimesheetId) : IRequest<TimesheetDetailDto>;
public record GetPayrollSummaryQuery(Guid OrgId, DateOnly PeriodStart, DateOnly PeriodEnd) : IRequest<IReadOnlyList<PayrollExportDto>>;

// Settings
public record GetOrganizationQuery(Guid OrgId) : IRequest<OrganizationDto>;
public record GetSchedulingRulesQuery(Guid OrgId) : IRequest<SchedulingRulesDto>;
public record GetRolesQuery(Guid OrgId) : IRequest<IReadOnlyList<RoleDto>>;
public record GetStationsQuery(Guid OrgId) : IRequest<IReadOnlyList<StationDto>>;
public record GetDepartmentsQuery(Guid OrgId) : IRequest<IReadOnlyList<DepartmentDto>>;
public record GetLocationsQuery(Guid OrgId) : IRequest<IReadOnlyList<LocationDto>>;
public record GetShiftTemplatesQuery(Guid OrgId) : IRequest<IReadOnlyList<ShiftTemplateDto>>;
public record GetLeaveTypesQuery(Guid OrgId) : IRequest<IReadOnlyList<LeaveTypeDto>>;

// Notifications
public record GetNotificationsQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<(IReadOnlyList<NotificationDto> Items, int UnreadCount)>;
public record GetAnnouncementsQuery(Guid OrgId, int Page = 1, int PageSize = 20) : IRequest<PagedResult<AnnouncementDto>>;

// Availability
public record GetAvailabilityQuery(Guid OrgId, Guid EmployeeId) : IRequest<IReadOnlyList<AvailabilityDto>>;
public record GetAvailabilityOverridesQuery(Guid OrgId, Guid EmployeeId, DateOnly? From = null, DateOnly? To = null) : IRequest<IReadOnlyList<AvailabilityOverrideDto>>;

// Dashboard
public record GetManagerDashboardQuery(Guid OrgId) : IRequest<ManagerDashboardDto>;
public record GetEmployeeDashboardQuery(Guid OrgId, Guid EmployeeId) : IRequest<EmployeeDashboardDto>;

public record ManagerDashboardDto(
    int TotalEmployees, int ActiveToday, int OnLeaveToday,
    int PendingTimeOffRequests, int PendingTimesheets,
    int OpenShiftsCount, decimal WeeklyLaborHours,
    IReadOnlyList<ShiftAssignmentDto> TodayShifts);

public record EmployeeDashboardDto(
    ClockStatusDto ClockStatus, IReadOnlyList<ShiftAssignmentDto> UpcomingShifts,
    IReadOnlyList<LeaveBalanceDto> LeaveBalances,
    decimal HoursThisWeek, decimal HoursThisPeriod,
    int PendingTimeOffRequests);
