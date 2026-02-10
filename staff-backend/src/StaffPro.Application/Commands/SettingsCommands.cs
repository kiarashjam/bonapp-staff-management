using MediatR;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Commands;

// Organization
public record UpdateOrganizationCommand(Guid OrgId, UpdateOrganizationRequest Dto) : IRequest<OrganizationDto>;
public record UpdateSchedulingRulesCommand(Guid OrgId, UpdateSchedulingRulesRequest Dto) : IRequest<SchedulingRulesDto>;

// Roles
public record CreateRoleCommand(Guid OrgId, CreateRoleRequest Dto) : IRequest<RoleDto>;
public record UpdateRoleCommand(Guid OrgId, Guid RoleId, UpdateRoleRequest Dto) : IRequest<RoleDto>;
public record DeleteRoleCommand(Guid OrgId, Guid RoleId) : IRequest<Unit>;

// Stations
public record CreateStationCommand(Guid OrgId, CreateStationRequest Dto) : IRequest<StationDto>;
public record UpdateStationCommand(Guid OrgId, Guid StationId, UpdateStationRequest Dto) : IRequest<StationDto>;
public record DeleteStationCommand(Guid OrgId, Guid StationId) : IRequest<Unit>;

// Departments
public record CreateDepartmentCommand(Guid OrgId, CreateDepartmentRequest Dto) : IRequest<DepartmentDto>;
public record UpdateDepartmentCommand(Guid OrgId, Guid DepartmentId, string Name, string? Description, string Color) : IRequest<DepartmentDto>;

// Locations
public record CreateLocationCommand(Guid OrgId, CreateLocationRequest Dto) : IRequest<LocationDto>;
public record UpdateLocationCommand(Guid OrgId, Guid LocationId, CreateLocationRequest Dto) : IRequest<LocationDto>;

// Shift Templates
public record CreateShiftTemplateCommand(Guid OrgId, CreateShiftTemplateRequest Dto) : IRequest<ShiftTemplateDto>;
public record UpdateShiftTemplateCommand(Guid OrgId, Guid TemplateId, CreateShiftTemplateRequest Dto) : IRequest<ShiftTemplateDto>;
public record DeleteShiftTemplateCommand(Guid OrgId, Guid TemplateId) : IRequest<Unit>;

// Leave Types
public record CreateLeaveTypeCommand(Guid OrgId, CreateLeaveTypeRequest Dto) : IRequest<LeaveTypeDto>;
public record UpdateLeaveTypeCommand(Guid OrgId, Guid LeaveTypeId, CreateLeaveTypeRequest Dto) : IRequest<LeaveTypeDto>;

// Announcements
public record CreateAnnouncementCommand(Guid OrgId, CreateAnnouncementRequest Dto, string PostedBy) : IRequest<AnnouncementDto>;
public record MarkAnnouncementReadCommand(Guid OrgId, Guid AnnouncementId, Guid EmployeeId) : IRequest<Unit>;

// Availability
public record SetAvailabilityCommand(Guid OrgId, Guid EmployeeId, IReadOnlyList<CreateAvailabilityRequest> Items) : IRequest<IReadOnlyList<AvailabilityDto>>;
public record CreateAvailabilityOverrideCommand(Guid OrgId, Guid EmployeeId, CreateAvailabilityOverrideRequest Dto) : IRequest<AvailabilityOverrideDto>;
public record DeleteAvailabilityOverrideCommand(Guid OrgId, Guid OverrideId) : IRequest<Unit>;

// Notifications
public record MarkNotificationReadCommand(Guid UserId, Guid NotificationId) : IRequest<Unit>;
public record MarkAllNotificationsReadCommand(Guid UserId) : IRequest<Unit>;
