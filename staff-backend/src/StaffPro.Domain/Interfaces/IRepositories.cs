using StaffPro.Domain.Entities;

namespace StaffPro.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
}

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<(IReadOnlyList<Employee> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, int page, int pageSize, string? search = null,
        Guid? locationId = null, Guid? roleId = null, CancellationToken ct = default);
    Task<Employee?> GetWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(Guid orgId, string email, Guid? excludeId = null, CancellationToken ct = default);
}

public interface IScheduleRepository : IRepository<SchedulePeriod>
{
    Task<SchedulePeriod?> GetWithAssignmentsAsync(Guid id, CancellationToken ct = default);
    Task<SchedulePeriod?> GetCurrentDraftAsync(Guid orgId, Guid locationId, CancellationToken ct = default);
    Task<IReadOnlyList<ShiftAssignment>> GetAssignmentsForDateRangeAsync(
        Guid orgId, DateOnly start, DateOnly end, Guid? employeeId = null, CancellationToken ct = default);
    Task<ShiftAssignment> AddAssignmentAsync(ShiftAssignment assignment, CancellationToken ct = default);
    Task UpdateAssignmentAsync(ShiftAssignment assignment, CancellationToken ct = default);
    Task DeleteAssignmentAsync(Guid assignmentId, CancellationToken ct = default);
}

public interface ITimeOffRepository : IRepository<TimeOffRequest>
{
    Task<(IReadOnlyList<TimeOffRequest> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, int page, int pageSize, Guid? employeeId = null,
        string? status = null, CancellationToken ct = default);
    Task<IReadOnlyList<TimeOffRequest>> GetApprovedForDateRangeAsync(
        Guid orgId, DateOnly start, DateOnly end, CancellationToken ct = default);
    Task<int> CountApprovedOnDateAsync(Guid orgId, DateOnly date, CancellationToken ct = default);
}

public interface IClockRepository
{
    Task<ClockEntry> AddAsync(ClockEntry entry, CancellationToken ct = default);
    Task<ClockEntry?> GetLastEntryAsync(Guid employeeId, CancellationToken ct = default);
    Task<IReadOnlyList<ClockEntry>> GetEntriesForDateAsync(
        Guid employeeId, DateOnly date, CancellationToken ct = default);
    Task<IReadOnlyList<ClockEntry>> GetEntriesForPeriodAsync(
        Guid employeeId, DateOnly start, DateOnly end, CancellationToken ct = default);
}

public interface ITimesheetRepository : IRepository<Timesheet>
{
    Task<(IReadOnlyList<Timesheet> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, int page, int pageSize, Guid? employeeId = null,
        string? status = null, CancellationToken ct = default);
    Task<Timesheet?> GetWithEntriesAsync(Guid id, CancellationToken ct = default);
    Task<Timesheet?> GetForEmployeePeriodAsync(
        Guid employeeId, DateOnly start, DateOnly end, CancellationToken ct = default);
}

public interface INotificationRepository
{
    Task<Notification> AddAsync(Notification notification, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct = default);
    Task<(IReadOnlyList<Notification> Items, int UnreadCount)> GetForUserAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default);
    Task MarkAsReadAsync(Guid notificationId, CancellationToken ct = default);
    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
