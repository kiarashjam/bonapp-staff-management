using MediatR;

namespace StaffPro.Domain.Events;

public record SchedulePublishedEvent(Guid SchedulePeriodId, Guid OrganizationId) : INotification;
public record ShiftAssignedEvent(Guid ShiftAssignmentId, Guid EmployeeId) : INotification;
public record ShiftChangedEvent(Guid ShiftAssignmentId, Guid EmployeeId, string ChangeDescription) : INotification;
public record TimeOffRequestCreatedEvent(Guid RequestId, Guid EmployeeId, Guid ManagerUserId) : INotification;
public record TimeOffRequestReviewedEvent(Guid RequestId, Guid EmployeeId, bool Approved) : INotification;
public record ClockEntryCreatedEvent(Guid ClockEntryId, Guid EmployeeId) : INotification;
public record TimesheetApprovedEvent(Guid TimesheetId, Guid EmployeeId) : INotification;
public record AnnouncementPostedEvent(Guid AnnouncementId, Guid OrganizationId) : INotification;
public record EmployeeCreatedEvent(Guid EmployeeId, Guid OrganizationId) : INotification;
