using FluentValidation;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Validators;

public class CreateSchedulePeriodValidator : AbstractValidator<CreateSchedulePeriodRequest>
{
    public CreateSchedulePeriodValidator()
    {
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x).Must(x => (x.EndDate.DayNumber - x.StartDate.DayNumber) <= 31)
            .WithMessage("Schedule period cannot exceed 31 days");
    }
}

public class CreateShiftAssignmentValidator : AbstractValidator<CreateShiftAssignmentRequest>
{
    public CreateShiftAssignmentValidator()
    {
        RuleFor(x => x.SchedulePeriodId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();
        RuleFor(x => x.BreakDurationMinutes).GreaterThanOrEqualTo(0).LessThanOrEqualTo(120);
    }
}

public class CreateTimeOffRequestValidator : AbstractValidator<CreateTimeOffRequest>
{
    public CreateTimeOffRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.LeaveTypeId).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.Reason).MaximumLength(500);
    }
}
