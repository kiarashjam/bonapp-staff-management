using StaffPro.Domain.Enums;

namespace StaffPro.Domain.Entities;

public class Contract : TenantEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public ContractType ContractType { get; set; }
    public decimal ContractedHoursPerWeek { get; set; }
    public decimal HourlyRateCents { get; set; } // Stored in cents
    public decimal? SalaryMonthlyCents { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }
    public int NoticePeriodDays { get; set; } = 30;
    public bool IsActive { get; set; } = true;

    public decimal HourlyRate => HourlyRateCents / 100m;
}
