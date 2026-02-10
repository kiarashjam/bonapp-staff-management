using MediatR;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Commands;

public record CreateTimeOffRequestCommand(Guid OrgId, CreateTimeOffRequest Dto) : IRequest<TimeOffRequestDto>;
public record ReviewTimeOffRequestCommand(Guid OrgId, Guid RequestId, ReviewTimeOffRequest Dto, string ReviewedBy) : IRequest<TimeOffRequestDto>;
public record CancelTimeOffRequestCommand(Guid OrgId, Guid RequestId) : IRequest<Unit>;
