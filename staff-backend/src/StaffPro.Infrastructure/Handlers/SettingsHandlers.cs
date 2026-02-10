using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffPro.Application.Commands;
using StaffPro.Application.DTOs;
using StaffPro.Application.Queries;
using StaffPro.Domain.Entities;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Handlers;

// ── Organization ──

public class GetOrganizationHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetOrganizationQuery, OrganizationDto>
{
    public async Task<OrganizationDto> Handle(GetOrganizationQuery r, CancellationToken ct)
    {
        var org = await db.Organizations.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.Id == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Organization not found");
        return mapper.Map<OrganizationDto>(org);
    }
}

public class UpdateOrganizationHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateOrganizationCommand, OrganizationDto>
{
    public async Task<OrganizationDto> Handle(UpdateOrganizationCommand r, CancellationToken ct)
    {
        var org = await db.Organizations.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.Id == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Organization not found");
        var req = r.Dto;
        org.Name = req.Name; org.Phone = req.Phone; org.Email = req.Email;
        org.Address = req.Address; org.City = req.City; org.Country = req.Country;
        org.PostalCode = req.PostalCode; org.Timezone = req.Timezone;
        org.Currency = req.Currency; org.DefaultLanguage = req.DefaultLanguage;
        await db.SaveChangesAsync(ct);
        return mapper.Map<OrganizationDto>(org);
    }
}

// ── Scheduling Rules ──

public class GetSchedulingRulesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetSchedulingRulesQuery, SchedulingRulesDto>
{
    public async Task<SchedulingRulesDto> Handle(GetSchedulingRulesQuery r, CancellationToken ct)
    {
        var org = await db.Organizations.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.Id == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Organization not found");
        return mapper.Map<SchedulingRulesDto>(org);
    }
}

public class UpdateSchedulingRulesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateSchedulingRulesCommand, SchedulingRulesDto>
{
    public async Task<SchedulingRulesDto> Handle(UpdateSchedulingRulesCommand r, CancellationToken ct)
    {
        var org = await db.Organizations.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.Id == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Organization not found");
        var req = r.Dto;
        org.MinRestHoursBetweenShifts = req.MinRestHoursBetweenShifts;
        org.MaxConsecutiveWorkDays = req.MaxConsecutiveWorkDays;
        org.MaxHoursPerWeek = req.MaxHoursPerWeek; org.MaxHoursPerDay = req.MaxHoursPerDay;
        org.BreakAfterMinutes = req.BreakAfterMinutes; org.BreakDurationMinutes = req.BreakDurationMinutes;
        org.ClockInGraceMinutes = req.ClockInGraceMinutes;
        org.ClockInLateThresholdMinutes = req.ClockInLateThresholdMinutes;
        org.ClockRoundingMinutes = req.ClockRoundingMinutes;
        org.OvertimeWeeklyThreshold = req.OvertimeWeeklyThreshold;
        org.OvertimeDailyThreshold = req.OvertimeDailyThreshold;
        org.OvertimeMultiplier = req.OvertimeMultiplier;
        await db.SaveChangesAsync(ct);
        return mapper.Map<SchedulingRulesDto>(org);
    }
}

// ── Roles ──

public class GetRolesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetRolesQuery, IReadOnlyList<RoleDto>>
{
    public async Task<IReadOnlyList<RoleDto>> Handle(GetRolesQuery r, CancellationToken ct)
    {
        var items = await db.Roles_Custom.Include(x => x.Department)
            .Where(x => x.OrganizationId == r.OrgId).OrderBy(x => x.Name).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<RoleDto>>(items);
    }
}

public class CreateRoleHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateRoleCommand, RoleDto>
{
    public async Task<RoleDto> Handle(CreateRoleCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var role = new Role
        {
            OrganizationId = r.OrgId, Name = req.Name, Description = req.Description,
            DefaultHourlyRate = req.DefaultHourlyRate, Color = req.Color, DepartmentId = req.DepartmentId
        };
        db.Roles_Custom.Add(role);
        await db.SaveChangesAsync(ct);
        var loaded = await db.Roles_Custom.Include(x => x.Department).FirstAsync(x => x.Id == role.Id, ct);
        return mapper.Map<RoleDto>(loaded);
    }
}

public class UpdateRoleHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    public async Task<RoleDto> Handle(UpdateRoleCommand r, CancellationToken ct)
    {
        var role = await db.Roles_Custom.FirstOrDefaultAsync(x => x.Id == r.RoleId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Role not found");
        var req = r.Dto;
        role.Name = req.Name; role.Description = req.Description;
        role.DefaultHourlyRate = req.DefaultHourlyRate; role.Color = req.Color; role.DepartmentId = req.DepartmentId;
        await db.SaveChangesAsync(ct);
        var loaded = await db.Roles_Custom.Include(x => x.Department).FirstAsync(x => x.Id == role.Id, ct);
        return mapper.Map<RoleDto>(loaded);
    }
}

public class DeleteRoleHandler(StaffProDbContext db)
    : IRequestHandler<DeleteRoleCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRoleCommand r, CancellationToken ct)
    {
        var role = await db.Roles_Custom.FirstOrDefaultAsync(x => x.Id == r.RoleId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Role not found");
        db.Roles_Custom.Remove(role);
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

// ── Stations ──

public class GetStationsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetStationsQuery, IReadOnlyList<StationDto>>
{
    public async Task<IReadOnlyList<StationDto>> Handle(GetStationsQuery r, CancellationToken ct)
    {
        var items = await db.Stations.Include(x => x.Location).Include(x => x.StationRoles)
            .Where(x => x.OrganizationId == r.OrgId).OrderBy(x => x.Name).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<StationDto>>(items);
    }
}

public class CreateStationHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateStationCommand, StationDto>
{
    public async Task<StationDto> Handle(CreateStationCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var station = new Station
        {
            OrganizationId = r.OrgId, Name = req.Name, Description = req.Description,
            MaxCapacity = req.MaxCapacity, LocationId = req.LocationId
        };
        if (req.RoleIds?.Any() == true)
            foreach (var rid in req.RoleIds)
                station.StationRoles.Add(new StationRole { StationId = station.Id, RoleId = rid });
        db.Stations.Add(station);
        await db.SaveChangesAsync(ct);
        var loaded = await db.Stations.Include(x => x.Location).Include(x => x.StationRoles).FirstAsync(x => x.Id == station.Id, ct);
        return mapper.Map<StationDto>(loaded);
    }
}

public class UpdateStationHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateStationCommand, StationDto>
{
    public async Task<StationDto> Handle(UpdateStationCommand r, CancellationToken ct)
    {
        var station = await db.Stations.Include(x => x.StationRoles)
            .FirstOrDefaultAsync(x => x.Id == r.StationId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Station not found");
        var req = r.Dto;
        station.Name = req.Name; station.Description = req.Description;
        station.MaxCapacity = req.MaxCapacity; station.LocationId = req.LocationId;
        db.StationRoles.RemoveRange(station.StationRoles);
        if (req.RoleIds?.Any() == true)
            foreach (var rid in req.RoleIds)
                station.StationRoles.Add(new StationRole { StationId = station.Id, RoleId = rid });
        await db.SaveChangesAsync(ct);
        var loaded = await db.Stations.Include(x => x.Location).Include(x => x.StationRoles).FirstAsync(x => x.Id == station.Id, ct);
        return mapper.Map<StationDto>(loaded);
    }
}

public class DeleteStationHandler(StaffProDbContext db)
    : IRequestHandler<DeleteStationCommand, Unit>
{
    public async Task<Unit> Handle(DeleteStationCommand r, CancellationToken ct)
    {
        var station = await db.Stations.FirstOrDefaultAsync(x => x.Id == r.StationId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Station not found");
        db.Stations.Remove(station);
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

// ── Departments ──

public class GetDepartmentsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    public async Task<IReadOnlyList<DepartmentDto>> Handle(GetDepartmentsQuery r, CancellationToken ct)
    {
        var items = await db.Departments.Where(x => x.OrganizationId == r.OrgId).OrderBy(x => x.Name).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<DepartmentDto>>(items);
    }
}

public class CreateDepartmentHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(CreateDepartmentCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var dept = new Department { OrganizationId = r.OrgId, Name = req.Name, Description = req.Description, Color = req.Color };
        db.Departments.Add(dept);
        await db.SaveChangesAsync(ct);
        return mapper.Map<DepartmentDto>(dept);
    }
}

public class UpdateDepartmentHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(UpdateDepartmentCommand r, CancellationToken ct)
    {
        var dept = await db.Departments.FirstOrDefaultAsync(x => x.Id == r.DepartmentId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Department not found");
        dept.Name = r.Name; dept.Description = r.Description; dept.Color = r.Color;
        await db.SaveChangesAsync(ct);
        return mapper.Map<DepartmentDto>(dept);
    }
}

// ── Locations ──

public class GetLocationsHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetLocationsQuery, IReadOnlyList<LocationDto>>
{
    public async Task<IReadOnlyList<LocationDto>> Handle(GetLocationsQuery r, CancellationToken ct)
    {
        var items = await db.Locations.Where(x => x.OrganizationId == r.OrgId).OrderBy(x => x.Name).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<LocationDto>>(items);
    }
}

public class CreateLocationHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateLocationCommand, LocationDto>
{
    public async Task<LocationDto> Handle(CreateLocationCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var loc = new Location
        {
            OrganizationId = r.OrgId, Name = req.Name, Address = req.Address,
            City = req.City, PostalCode = req.PostalCode, Phone = req.Phone,
            Latitude = req.Latitude, Longitude = req.Longitude, GeofenceRadiusMeters = req.GeofenceRadiusMeters
        };
        db.Locations.Add(loc);
        await db.SaveChangesAsync(ct);
        return mapper.Map<LocationDto>(loc);
    }
}

public class UpdateLocationHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateLocationCommand, LocationDto>
{
    public async Task<LocationDto> Handle(UpdateLocationCommand r, CancellationToken ct)
    {
        var loc = await db.Locations.FirstOrDefaultAsync(x => x.Id == r.LocationId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Location not found");
        var req = r.Dto;
        loc.Name = req.Name; loc.Address = req.Address; loc.City = req.City;
        loc.PostalCode = req.PostalCode; loc.Phone = req.Phone;
        loc.Latitude = req.Latitude; loc.Longitude = req.Longitude;
        loc.GeofenceRadiusMeters = req.GeofenceRadiusMeters;
        await db.SaveChangesAsync(ct);
        return mapper.Map<LocationDto>(loc);
    }
}

// ── Shift Templates ──

public class GetShiftTemplatesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetShiftTemplatesQuery, IReadOnlyList<ShiftTemplateDto>>
{
    public async Task<IReadOnlyList<ShiftTemplateDto>> Handle(GetShiftTemplatesQuery r, CancellationToken ct)
    {
        var items = await db.ShiftTemplates.Where(x => x.OrganizationId == r.OrgId).OrderBy(x => x.Name).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<ShiftTemplateDto>>(items);
    }
}

public class CreateShiftTemplateHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateShiftTemplateCommand, ShiftTemplateDto>
{
    public async Task<ShiftTemplateDto> Handle(CreateShiftTemplateCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var t = new ShiftTemplate
        {
            OrganizationId = r.OrgId, Name = req.Name, StartTime = req.StartTime,
            EndTime = req.EndTime, BreakDurationMinutes = req.BreakDurationMinutes,
            BreakIsPaid = req.BreakIsPaid, Color = req.Color
        };
        db.ShiftTemplates.Add(t);
        await db.SaveChangesAsync(ct);
        return mapper.Map<ShiftTemplateDto>(t);
    }
}

public class UpdateShiftTemplateHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateShiftTemplateCommand, ShiftTemplateDto>
{
    public async Task<ShiftTemplateDto> Handle(UpdateShiftTemplateCommand r, CancellationToken ct)
    {
        var t = await db.ShiftTemplates.FirstOrDefaultAsync(x => x.Id == r.TemplateId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Shift template not found");
        var req = r.Dto;
        t.Name = req.Name; t.StartTime = req.StartTime; t.EndTime = req.EndTime;
        t.BreakDurationMinutes = req.BreakDurationMinutes; t.BreakIsPaid = req.BreakIsPaid; t.Color = req.Color;
        await db.SaveChangesAsync(ct);
        return mapper.Map<ShiftTemplateDto>(t);
    }
}

public class DeleteShiftTemplateHandler(StaffProDbContext db)
    : IRequestHandler<DeleteShiftTemplateCommand, Unit>
{
    public async Task<Unit> Handle(DeleteShiftTemplateCommand r, CancellationToken ct)
    {
        var t = await db.ShiftTemplates.FirstOrDefaultAsync(x => x.Id == r.TemplateId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Shift template not found");
        db.ShiftTemplates.Remove(t);
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

// ── Leave Types ──

public class GetLeaveTypesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetLeaveTypesQuery, IReadOnlyList<LeaveTypeDto>>
{
    public async Task<IReadOnlyList<LeaveTypeDto>> Handle(GetLeaveTypesQuery r, CancellationToken ct)
    {
        var items = await db.LeaveTypes.Where(x => x.OrganizationId == r.OrgId).OrderBy(x => x.Name).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<LeaveTypeDto>>(items);
    }
}

public class CreateLeaveTypeHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateLeaveTypeCommand, LeaveTypeDto>
{
    public async Task<LeaveTypeDto> Handle(CreateLeaveTypeCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var lt = new LeaveType
        {
            OrganizationId = r.OrgId, Name = req.Name, IsPaid = req.IsPaid,
            RequiresDocument = req.RequiresDocument, MaxDaysPerYear = req.MaxDaysPerYear,
            AccrualRatePerMonth = req.AccrualRatePerMonth, MaxCarryOverDays = req.MaxCarryOverDays,
            Color = req.Color
        };
        db.LeaveTypes.Add(lt);
        await db.SaveChangesAsync(ct);
        return mapper.Map<LeaveTypeDto>(lt);
    }
}

public class UpdateLeaveTypeHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<UpdateLeaveTypeCommand, LeaveTypeDto>
{
    public async Task<LeaveTypeDto> Handle(UpdateLeaveTypeCommand r, CancellationToken ct)
    {
        var lt = await db.LeaveTypes.FirstOrDefaultAsync(x => x.Id == r.LeaveTypeId && x.OrganizationId == r.OrgId, ct)
            ?? throw new KeyNotFoundException("Leave type not found");
        var req = r.Dto;
        lt.Name = req.Name; lt.IsPaid = req.IsPaid; lt.RequiresDocument = req.RequiresDocument;
        lt.MaxDaysPerYear = req.MaxDaysPerYear; lt.AccrualRatePerMonth = req.AccrualRatePerMonth;
        lt.MaxCarryOverDays = req.MaxCarryOverDays; lt.Color = req.Color;
        await db.SaveChangesAsync(ct);
        return mapper.Map<LeaveTypeDto>(lt);
    }
}
