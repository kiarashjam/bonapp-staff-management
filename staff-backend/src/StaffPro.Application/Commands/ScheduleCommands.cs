using MediatR;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Commands;

public record CreateSchedulePeriodCommand(Guid OrgId, CreateSchedulePeriodRequest Dto) : IRequest<SchedulePeriodDto>;
public record PublishScheduleCommand(Guid OrgId, Guid SchedulePeriodId, string PublishedBy) : IRequest<SchedulePeriodDto>;
public record LockScheduleCommand(Guid OrgId, Guid SchedulePeriodId) : IRequest<SchedulePeriodDto>;
public record CreateShiftAssignmentCommand(Guid OrgId, CreateShiftAssignmentRequest Dto) : IRequest<ShiftAssignmentDto>;
public record UpdateShiftAssignmentCommand(Guid OrgId, Guid AssignmentId, UpdateShiftAssignmentRequest Dto) : IRequest<ShiftAssignmentDto>;
public record DeleteShiftAssignmentCommand(Guid OrgId, Guid AssignmentId) : IRequest<Unit>;
public record CopyScheduleCommand(Guid OrgId, CopyScheduleRequest Dto) : IRequest<SchedulePeriodDto>;
public record CreateStaffingRequirementCommand(Guid OrgId, CreateStaffingRequirementRequest Dto) : IRequest<StaffingRequirementDto>;
