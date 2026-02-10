using Microsoft.EntityFrameworkCore;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Interfaces;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly StaffProDbContext _db;

    public ScheduleRepository(StaffProDbContext db) => _db = db;

    public async Task<SchedulePeriod?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.SchedulePeriods.Include(sp => sp.Location).FirstOrDefaultAsync(sp => sp.Id == id, ct);

    public async Task<IReadOnlyList<SchedulePeriod>> GetAllAsync(CancellationToken ct = default)
        => await _db.SchedulePeriods.Include(sp => sp.Location).ToListAsync(ct);

    public async Task<SchedulePeriod> AddAsync(SchedulePeriod entity, CancellationToken ct = default)
    {
        await _db.SchedulePeriods.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(SchedulePeriod entity, CancellationToken ct = default)
    {
        _db.SchedulePeriods.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(SchedulePeriod entity, CancellationToken ct = default)
    {
        entity.IsDeleted = true;
        await _db.SaveChangesAsync(ct);
    }

    public async Task<SchedulePeriod?> GetWithAssignmentsAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.SchedulePeriods
            .Include(sp => sp.Location)
            .Include(sp => sp.ShiftAssignments)
                .ThenInclude(sa => sa.Employee)
            .Include(sp => sp.ShiftAssignments)
                .ThenInclude(sa => sa.ShiftTemplate)
            .Include(sp => sp.ShiftAssignments)
                .ThenInclude(sa => sa.Station)
            .Include(sp => sp.ShiftAssignments)
                .ThenInclude(sa => sa.Role)
            .Include(sp => sp.StaffingRequirements)
                .ThenInclude(sr => sr.Role)
            .FirstOrDefaultAsync(sp => sp.Id == id, ct);
    }

    public async Task<SchedulePeriod?> GetCurrentDraftAsync(Guid orgId, Guid locationId, CancellationToken ct = default)
    {
        return await _db.SchedulePeriods
            .Where(sp => sp.OrganizationId == orgId && sp.LocationId == locationId &&
                         sp.Status == Domain.Enums.SchedulePeriodStatus.Draft)
            .OrderByDescending(sp => sp.StartDate)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<ShiftAssignment>> GetAssignmentsForDateRangeAsync(
        Guid orgId, DateOnly start, DateOnly end, Guid? employeeId = null, CancellationToken ct = default)
    {
        var query = _db.ShiftAssignments
            .Include(sa => sa.Employee)
            .Include(sa => sa.ShiftTemplate)
            .Include(sa => sa.Station)
            .Include(sa => sa.Role)
            .Where(sa => sa.OrganizationId == orgId && sa.Date >= start && sa.Date <= end);

        if (employeeId.HasValue)
            query = query.Where(sa => sa.EmployeeId == employeeId);

        return await query.OrderBy(sa => sa.Date).ThenBy(sa => sa.StartTime).ToListAsync(ct);
    }

    public async Task<ShiftAssignment> AddAssignmentAsync(ShiftAssignment assignment, CancellationToken ct = default)
    {
        await _db.ShiftAssignments.AddAsync(assignment, ct);
        await _db.SaveChangesAsync(ct);
        return assignment;
    }

    public async Task UpdateAssignmentAsync(ShiftAssignment assignment, CancellationToken ct = default)
    {
        _db.ShiftAssignments.Update(assignment);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAssignmentAsync(Guid assignmentId, CancellationToken ct = default)
    {
        var assignment = await _db.ShiftAssignments.FindAsync([assignmentId], ct);
        if (assignment != null)
        {
            _db.ShiftAssignments.Remove(assignment);
            await _db.SaveChangesAsync(ct);
        }
    }
}
