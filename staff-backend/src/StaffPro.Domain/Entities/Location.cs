namespace StaffPro.Domain.Entities;

public class Location : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int GeofenceRadiusMeters { get; set; } = 100;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Employee> Employees { get; set; } = [];
    public ICollection<Station> Stations { get; set; } = [];
    public ICollection<SchedulePeriod> SchedulePeriods { get; set; } = [];
}
