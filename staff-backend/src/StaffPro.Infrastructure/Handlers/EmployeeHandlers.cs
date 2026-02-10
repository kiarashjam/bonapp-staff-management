using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffPro.Application.Commands;
using StaffPro.Application.DTOs;
using StaffPro.Application.Queries;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Handlers;

// ── Queries ──

public class GetEmployeesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetEmployeesQuery, PagedResult<EmployeeListDto>>
{
    public async Task<PagedResult<EmployeeListDto>> Handle(GetEmployeesQuery r, CancellationToken ct)
    {
        var q = db.Employees
            .Include(e => e.EmployeeRoles).ThenInclude(er => er.Role)
            .Include(e => e.Location)
            .Where(e => e.OrganizationId == r.OrgId);

        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.ToLower();
            q = q.Where(e => e.FirstName.ToLower().Contains(s) || e.LastName.ToLower().Contains(s) || e.Email.ToLower().Contains(s));
        }
        if (r.LocationId.HasValue) q = q.Where(e => e.LocationId == r.LocationId);
        if (r.RoleId.HasValue) q = q.Where(e => e.EmployeeRoles.Any(er => er.RoleId == r.RoleId));

        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize).ToListAsync(ct);

        return new PagedResult<EmployeeListDto>(
            mapper.Map<IReadOnlyList<EmployeeListDto>>(items), total, r.Page, r.PageSize,
            (int)Math.Ceiling(total / (double)r.PageSize));
    }
}

public class GetEmployeeByIdHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailDto>
{
    public async Task<EmployeeDetailDto> Handle(GetEmployeeByIdQuery r, CancellationToken ct)
    {
        var emp = await db.Employees
            .Include(e => e.Location).Include(e => e.Contracts)
            .Include(e => e.EmployeeRoles).ThenInclude(er => er.Role)
            .FirstOrDefaultAsync(e => e.Id == r.EmployeeId && e.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException($"Employee {r.EmployeeId} not found");
        return mapper.Map<EmployeeDetailDto>(emp);
    }
}

// ── Commands ──

public class CreateEmployeeHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateEmployeeCommand, EmployeeDetailDto>
{
    public async Task<EmployeeDetailDto> Handle(CreateEmployeeCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        if (await db.Employees.AnyAsync(e => e.OrganizationId == r.OrgId && e.Email == req.Email, ct))
            throw new InvalidOperationException("An employee with this email already exists");

        var emp = new Employee
        {
            OrganizationId = r.OrgId, FirstName = req.FirstName, LastName = req.LastName,
            Email = req.Email, Phone = req.Phone, DateOfBirth = req.DateOfBirth,
            Address = req.Address, City = req.City, PostalCode = req.PostalCode,
            HireDate = req.HireDate, LocationId = req.LocationId
        };
        db.Employees.Add(emp);

        if (req.Contract != null)
        {
            var c = req.Contract;
            db.Contracts.Add(new Contract
            {
                OrganizationId = r.OrgId, EmployeeId = emp.Id, ContractType = c.ContractType,
                ContractedHoursPerWeek = c.ContractedHoursPerWeek,
                HourlyRateCents = c.HourlyRate * 100, SalaryMonthlyCents = c.SalaryMonthly * 100,
                StartDate = c.StartDate, EndDate = c.EndDate, ProbationEndDate = c.ProbationEndDate,
                NoticePeriodDays = c.NoticePeriodDays, IsActive = true
            });
        }

        if (req.Roles?.Any() == true)
        {
            foreach (var role in req.Roles)
                db.EmployeeRoles.Add(new EmployeeRole
                {
                    EmployeeId = emp.Id, RoleId = role.RoleId,
                    ProficiencyLevel = role.ProficiencyLevel, IsPrimary = role.IsPrimary
                });
        }

        await db.SaveChangesAsync(ct);

        return await new GetEmployeeByIdHandler(db, mapper)
            .Handle(new GetEmployeeByIdQuery(r.OrgId, emp.Id), ct);
    }
}

public class UpdateEmployeeHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateEmployeeCommand, EmployeeDetailDto>
{
    public async Task<EmployeeDetailDto> Handle(UpdateEmployeeCommand r, CancellationToken ct)
    {
        var emp = await db.Employees.FirstOrDefaultAsync(e => e.Id == r.EmployeeId && e.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException($"Employee {r.EmployeeId} not found");
        var req = r.Dto;
        emp.FirstName = req.FirstName; emp.LastName = req.LastName; emp.Email = req.Email;
        emp.Phone = req.Phone; emp.DateOfBirth = req.DateOfBirth; emp.Address = req.Address;
        emp.City = req.City; emp.PostalCode = req.PostalCode; emp.Status = req.Status;
        emp.EmergencyContactName = req.EmergencyContactName;
        emp.EmergencyContactPhone = req.EmergencyContactPhone;
        emp.EmergencyContactRelation = req.EmergencyContactRelation;
        emp.LocationId = req.LocationId;
        await db.SaveChangesAsync(ct);
        return await new GetEmployeeByIdHandler(db, mapper)
            .Handle(new GetEmployeeByIdQuery(r.OrgId, emp.Id), ct);
    }
}

public class DeleteEmployeeHandler(StaffProDbContext db)
    : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    public async Task<Unit> Handle(DeleteEmployeeCommand r, CancellationToken ct)
    {
        var emp = await db.Employees.FirstOrDefaultAsync(e => e.Id == r.EmployeeId && e.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException($"Employee {r.EmployeeId} not found");
        emp.IsDeleted = true;
        emp.Status = EmployeeStatus.Terminated;
        emp.TerminationDate = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class CreateContractHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateContractCommand, ContractDto>
{
    public async Task<ContractDto> Handle(CreateContractCommand r, CancellationToken ct)
    {
        // Deactivate old contracts
        var old = await db.Contracts.Where(c => c.EmployeeId == r.EmployeeId && c.IsActive).ToListAsync(ct);
        foreach (var o in old) o.IsActive = false;

        var req = r.Dto;
        var contract = new Contract
        {
            OrganizationId = r.OrgId, EmployeeId = r.EmployeeId, ContractType = req.ContractType,
            ContractedHoursPerWeek = req.ContractedHoursPerWeek,
            HourlyRateCents = req.HourlyRate * 100, SalaryMonthlyCents = req.SalaryMonthly * 100,
            StartDate = req.StartDate, EndDate = req.EndDate, ProbationEndDate = req.ProbationEndDate,
            NoticePeriodDays = req.NoticePeriodDays, IsActive = true
        };
        db.Contracts.Add(contract);
        await db.SaveChangesAsync(ct);
        return mapper.Map<ContractDto>(contract);
    }
}

public class AssignRolesHandler(StaffProDbContext db)
    : IRequestHandler<AssignRolesCommand, IReadOnlyList<EmployeeRoleDto>>
{
    public async Task<IReadOnlyList<EmployeeRoleDto>> Handle(AssignRolesCommand r, CancellationToken ct)
    {
        var existing = await db.EmployeeRoles.Where(er => er.EmployeeId == r.EmployeeId).ToListAsync(ct);
        db.EmployeeRoles.RemoveRange(existing);

        foreach (var role in r.Roles)
            db.EmployeeRoles.Add(new EmployeeRole
            {
                EmployeeId = r.EmployeeId, RoleId = role.RoleId,
                ProficiencyLevel = role.ProficiencyLevel, IsPrimary = role.IsPrimary
            });
        await db.SaveChangesAsync(ct);

        var result = await db.EmployeeRoles
            .Include(er => er.Role)
            .Where(er => er.EmployeeId == r.EmployeeId).ToListAsync(ct);
        return result.Select(er => new EmployeeRoleDto(
            er.RoleId, er.Role.Name, er.Role.Color, er.ProficiencyLevel, er.IsPrimary)).ToList();
    }
}
