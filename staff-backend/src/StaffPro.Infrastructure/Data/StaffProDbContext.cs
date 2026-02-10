using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Interfaces;

namespace StaffPro.Infrastructure.Data;

public class StaffProDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    private Guid? _currentOrganizationId;

    public StaffProDbContext(DbContextOptions<StaffProDbContext> options) : base(options) { }

    // Core
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Role> Roles_Custom => Set<Role>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<StationRole> StationRoles => Set<StationRole>();

    // Employees
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<EmployeeRole> EmployeeRoles => Set<EmployeeRole>();

    // Availability
    public DbSet<RecurringAvailability> RecurringAvailabilities => Set<RecurringAvailability>();
    public DbSet<AvailabilityOverride> AvailabilityOverrides => Set<AvailabilityOverride>();

    // Schedule
    public DbSet<ShiftTemplate> ShiftTemplates => Set<ShiftTemplate>();
    public DbSet<SchedulePeriod> SchedulePeriods => Set<SchedulePeriod>();
    public DbSet<ShiftAssignment> ShiftAssignments => Set<ShiftAssignment>();
    public DbSet<StaffingRequirement> StaffingRequirements => Set<StaffingRequirement>();

    // Time Off
    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
    public DbSet<TimeOffRequest> TimeOffRequests => Set<TimeOffRequest>();
    public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();

    // Clock & Timesheet
    public DbSet<ClockEntry> ClockEntries => Set<ClockEntry>();
    public DbSet<Timesheet> Timesheets => Set<Timesheet>();
    public DbSet<TimesheetEntry> TimesheetEntries => Set<TimesheetEntry>();

    // Notifications
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<AnnouncementReadReceipt> AnnouncementReadReceipts => Set<AnnouncementReadReceipt>();

    // Audit
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // App Users (extended)
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    public void SetTenantId(Guid organizationId) => _currentOrganizationId = organizationId;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Identity table names
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers");

        // Composite keys
        builder.Entity<EmployeeRole>().HasKey(er => new { er.EmployeeId, er.RoleId });
        builder.Entity<StationRole>().HasKey(sr => new { sr.StationId, sr.RoleId });

        // Soft delete filter
        builder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Organization>().HasQueryFilter(e => !e.IsDeleted);

        // Decimal precision
        builder.Entity<Contract>().Property(c => c.HourlyRateCents).HasPrecision(18, 2);
        builder.Entity<Contract>().Property(c => c.SalaryMonthlyCents).HasPrecision(18, 2);
        builder.Entity<LeaveBalance>().Property(lb => lb.Entitled).HasPrecision(8, 2);
        builder.Entity<LeaveBalance>().Property(lb => lb.Used).HasPrecision(8, 2);
        builder.Entity<LeaveBalance>().Property(lb => lb.CarriedOver).HasPrecision(8, 2);
        builder.Entity<LeaveBalance>().Property(lb => lb.Adjustment).HasPrecision(8, 2);
        builder.Entity<Timesheet>().Property(t => t.RegularHours).HasPrecision(8, 2);
        builder.Entity<Timesheet>().Property(t => t.OvertimeHours).HasPrecision(8, 2);
        builder.Entity<Timesheet>().Property(t => t.NightHours).HasPrecision(8, 2);
        builder.Entity<Timesheet>().Property(t => t.WeekendHours).HasPrecision(8, 2);
        builder.Entity<Timesheet>().Property(t => t.BreakHours).HasPrecision(8, 2);
        builder.Entity<Timesheet>().Property(t => t.NetPayableHours).HasPrecision(8, 2);
        builder.Entity<Timesheet>().Property(t => t.RegularPayCents).HasPrecision(18, 2);
        builder.Entity<Timesheet>().Property(t => t.OvertimePayCents).HasPrecision(18, 2);
        builder.Entity<Timesheet>().Property(t => t.TotalGrossPayCents).HasPrecision(18, 2);
        builder.Entity<TimesheetEntry>().Property(te => te.BreakMinutes).HasPrecision(8, 2);
        builder.Entity<TimesheetEntry>().Property(te => te.RegularHours).HasPrecision(8, 2);
        builder.Entity<TimesheetEntry>().Property(te => te.OvertimeHours).HasPrecision(8, 2);
        builder.Entity<Organization>().Property(o => o.OvertimeMultiplier).HasPrecision(4, 2);

        // Indexes
        builder.Entity<Employee>().HasIndex(e => new { e.OrganizationId, e.Email }).IsUnique();
        builder.Entity<Employee>().HasIndex(e => new { e.OrganizationId, e.Status });
        builder.Entity<ShiftAssignment>().HasIndex(sa => new { sa.EmployeeId, sa.Date });
        builder.Entity<ShiftAssignment>().HasIndex(sa => new { sa.SchedulePeriodId, sa.Date });
        builder.Entity<ClockEntry>().HasIndex(ce => new { ce.EmployeeId, ce.Timestamp });
        builder.Entity<TimeOffRequest>().HasIndex(tr => new { tr.EmployeeId, tr.Status });
        builder.Entity<Notification>().HasIndex(n => new { n.RecipientUserId, n.IsRead });
        builder.Entity<Timesheet>().HasIndex(t => new { t.EmployeeId, t.PeriodStart });
        builder.Entity<AuditLog>().HasIndex(a => new { a.EntityType, a.EntityId });
        builder.Entity<AuditLog>().HasIndex(a => a.Timestamp);

        // Relationships
        builder.Entity<Employee>()
            .HasOne(e => e.ActiveContract)
            .WithMany()
            .HasForeignKey("ActiveContractId")
            .IsRequired(false);

        builder.Entity<Contract>()
            .HasOne(c => c.Employee)
            .WithMany(e => e.Contracts)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ShiftAssignment>()
            .HasOne(sa => sa.Employee)
            .WithMany(e => e.ShiftAssignments)
            .HasForeignKey(sa => sa.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TimeOffRequest>()
            .HasOne(tr => tr.Employee)
            .WithMany(e => e.TimeOffRequests)
            .HasForeignKey(tr => tr.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ClockEntry>()
            .HasOne(ce => ce.Employee)
            .WithMany(e => e.ClockEntries)
            .HasForeignKey(ce => ce.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<LeaveBalance>()
            .HasOne(lb => lb.Employee)
            .WithMany(e => e.LeaveBalances)
            .HasForeignKey(lb => lb.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RecurringAvailability>()
            .HasOne(ra => ra.Employee)
            .WithMany(e => e.RecurringAvailabilities)
            .HasForeignKey(ra => ra.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AvailabilityOverride>()
            .HasOne(ao => ao.Employee)
            .WithMany(e => e.AvailabilityOverrides)
            .HasForeignKey(ao => ao.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Fix multiple cascade path issues on SQL Server — set all FKs to Restrict
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var fk in entityType.GetForeignKeys())
            {
                if (fk.DeleteBehavior == DeleteBehavior.Cascade)
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(ct);
    }
}

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
