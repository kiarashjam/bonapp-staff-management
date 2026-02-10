namespace StaffPro.Domain.Entities;

public class ShiftTemplate : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int BreakDurationMinutes { get; set; } = 30;
    public bool BreakIsPaid { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public bool IsActive { get; set; } = true;

    public double TotalHours
    {
        get
        {
            var duration = EndTime.ToTimeSpan() - StartTime.ToTimeSpan();
            if (duration < TimeSpan.Zero) duration += TimeSpan.FromHours(24);
            return duration.TotalHours;
        }
    }

    public double NetHours => TotalHours - (BreakIsPaid ? 0 : BreakDurationMinutes / 60.0);
}
