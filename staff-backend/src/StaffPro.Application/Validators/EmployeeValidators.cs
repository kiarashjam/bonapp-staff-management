using FluentValidation;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Validators;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Phone).MaximumLength(20);
        RuleFor(x => x.HireDate).NotEmpty();
        RuleFor(x => x.Contract).SetValidator(new CreateContractValidator()!).When(x => x.Contract != null);
    }
}

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Phone).MaximumLength(20);
    }
}

public class CreateContractValidator : AbstractValidator<CreateContractRequest>
{
    public CreateContractValidator()
    {
        RuleFor(x => x.ContractedHoursPerWeek).GreaterThan(0).LessThanOrEqualTo(168);
        RuleFor(x => x.HourlyRate).GreaterThan(0);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue);
    }
}
