using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffPro.Application.Commands;
using StaffPro.Application.DTOs;
using StaffPro.Application.Queries;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Enums;
using StaffPro.Infrastructure.Data;

namespace StaffPro.Infrastructure.Handlers;

public class GetAvailabilityHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetAvailabilityQuery, IReadOnlyList<AvailabilityDto>>
{
    public async Task<IReadOnlyList<AvailabilityDto>> Handle(GetAvailabilityQuery r, CancellationToken ct)
    {
        var items = await db.RecurringAvailabilities
            .Where(a => a.EmployeeId == r.EmployeeId)
            .OrderBy(a => a.DayOfWeek).ThenBy(a => a.StartTime).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<AvailabilityDto>>(items);
    }
}

public class GetAvailabilityOverridesHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<GetAvailabilityOverridesQuery, IReadOnlyList<AvailabilityOverrideDto>>
{
    public async Task<IReadOnlyList<AvailabilityOverrideDto>> Handle(GetAvailabilityOverridesQuery r, CancellationToken ct)
    {
        var q = db.AvailabilityOverrides.Where(a => a.EmployeeId == r.EmployeeId);
        if (r.From.HasValue) q = q.Where(a => a.Date >= r.From);
        if (r.To.HasValue) q = q.Where(a => a.Date <= r.To);
        var items = await q.OrderBy(a => a.Date).ToListAsync(ct);
        return mapper.Map<IReadOnlyList<AvailabilityOverrideDto>>(items);
    }
}

public class SetAvailabilityHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<SetAvailabilityCommand, IReadOnlyList<AvailabilityDto>>
{
    public async Task<IReadOnlyList<AvailabilityDto>> Handle(SetAvailabilityCommand r, CancellationToken ct)
    {
        // Remove existing and replace
        var existing = await db.RecurringAvailabilities.Where(a => a.EmployeeId == r.EmployeeId).ToListAsync(ct);
        db.RecurringAvailabilities.RemoveRange(existing);

        foreach (var a in r.Items)
        {
            db.RecurringAvailabilities.Add(new RecurringAvailability
            {
                OrganizationId = r.OrgId, EmployeeId = r.EmployeeId,
                DayOfWeek = a.DayOfWeek, StartTime = a.StartTime, EndTime = a.EndTime,
                Type = Enum.TryParse<AvailabilityType>(a.Type, out var t) ? t : AvailabilityType.Available
            });
        }
        await db.SaveChangesAsync(ct);
        return await new GetAvailabilityHandler(db, mapper).Handle(new GetAvailabilityQuery(r.OrgId, r.EmployeeId), ct);
    }
}

public class CreateAvailabilityOverrideHandler(StaffProDbContext db, IMapper mapper)
    : IRequestHandler<CreateAvailabilityOverrideCommand, AvailabilityOverrideDto>
{
    public async Task<AvailabilityOverrideDto> Handle(CreateAvailabilityOverrideCommand r, CancellationToken ct)
    {
        var req = r.Dto;
        var ao = new AvailabilityOverride
        {
            OrganizationId = r.OrgId, EmployeeId = r.EmployeeId,
            Date = req.Date, StartTime = req.StartTime, EndTime = req.EndTime,
            Type = Enum.TryParse<AvailabilityType>(req.Type, out var t) ? t : AvailabilityType.Unavailable,
            Note = req.Note
        };
        db.AvailabilityOverrides.Add(ao);
        await db.SaveChangesAsync(ct);
        return mapper.Map<AvailabilityOverrideDto>(ao);
    }
}

public class DeleteAvailabilityOverrideHandler(StaffProDbContext db)
    : IRequestHandler<DeleteAvailabilityOverrideCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAvailabilityOverrideCommand r, CancellationToken ct)
    {
        var ao = await db.AvailabilityOverrides.FirstOrDefaultAsync(a => a.Id == r.OverrideId, ct)
            ?? throw new KeyNotFoundException("Availability override not found");
        db.AvailabilityOverrides.Remove(ao);
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
