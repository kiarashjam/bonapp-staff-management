using MediatR;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Commands;

public record ClockActionCommand(Guid OrgId, Guid EmployeeId, ClockActionRequest Dto) : IRequest<ClockEntryDto>;
public record ManualClockOverrideCommand(Guid OrgId, ManualClockOverrideRequest Dto, string OverrideBy) : IRequest<ClockEntryDto>;
public record ApproveTimesheetCommand(Guid OrgId, Guid TimesheetId, string ApprovedBy, string? Notes) : IRequest<TimesheetDto>;
public record SubmitTimesheetCommand(Guid OrgId, Guid TimesheetId) : IRequest<TimesheetDto>;
