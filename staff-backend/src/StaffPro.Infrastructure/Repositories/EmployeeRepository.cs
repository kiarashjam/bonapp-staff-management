using Microsoft.EntityFrameworkCore;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Interfaces;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly StaffProDbContext _db;

    public EmployeeRepository(StaffProDbContext db) => _db = db;

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Employees.FindAsync([id], ct);

    public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken ct = default)
        => await _db.Employees.ToListAsync(ct);

    public async Task<Employee> AddAsync(Employee entity, CancellationToken ct = default)
    {
        await _db.Employees.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Employee entity, CancellationToken ct = default)
    {
        _db.Employees.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Employee entity, CancellationToken ct = default)
    {
        entity.IsDeleted = true;
        await _db.SaveChangesAsync(ct);
    }

    public async Task<(IReadOnlyList<Employee> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, int page, int pageSize, string? search = null,
        Guid? locationId = null, Guid? roleId = null, CancellationToken ct = default)
    {
        var query = _db.Employees
            .Include(e => e.EmployeeRoles).ThenInclude(er => er.Role)
            .Include(e => e.Location)
            .Where(e => e.OrganizationId == orgId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(e =>
                e.FirstName.ToLower().Contains(s) ||
                e.LastName.ToLower().Contains(s) ||
                e.Email.ToLower().Contains(s));
        }

        if (locationId.HasValue)
            query = query.Where(e => e.LocationId == locationId);

        if (roleId.HasValue)
            query = query.Where(e => e.EmployeeRoles.Any(er => er.RoleId == roleId));

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Employee?> GetWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Employees
            .Include(e => e.Location)
            .Include(e => e.Contracts)
            .Include(e => e.EmployeeRoles).ThenInclude(er => er.Role)
            .Include(e => e.RecurringAvailabilities)
            .Include(e => e.LeaveBalances).ThenInclude(lb => lb.LeaveType)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<bool> EmailExistsAsync(Guid orgId, string email, Guid? excludeId = null, CancellationToken ct = default)
    {
        var query = _db.Employees.Where(e => e.OrganizationId == orgId && e.Email == email);
        if (excludeId.HasValue)
            query = query.Where(e => e.Id != excludeId);
        return await query.AnyAsync(ct);
    }
}
