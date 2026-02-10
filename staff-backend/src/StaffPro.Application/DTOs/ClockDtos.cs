using StaffPro.Domain.Enums;

namespace StaffPro.Application.DTOs;

public record ClockActionRequest(
    ClockEntryType EntryType, double? Latitude, double? Longitude,
    double? GpsAccuracyMeters, string? IpAddress);

public record ClockEntryDto(
    Guid Id, Guid EmployeeId, ClockEntryType EntryType,
    DateTime Timestamp, ClockSource Source, bool IsManualOverride, string? Notes);

public record ClockStatusDto(
    bool IsClockedIn, bool IsOnBreak, DateTime? LastClockIn,
    DateTime? LastBreakStart, string? CurrentShiftInfo,
    TimeSpan? WorkedSoFar);

public record ManualClockOverrideRequest(
    Guid EmployeeId, ClockEntryType EntryType,
    DateTime Timestamp, string Reason);

public record TimesheetDto(
    Guid Id, Guid EmployeeId, string EmployeeName, DateOnly PeriodStart,
    DateOnly PeriodEnd, decimal RegularHours, decimal OvertimeHours,
    decimal NetPayableHours, decimal TotalGrossPay,
    TimesheetStatus Status, DateTime? ApprovedAt, string? ApprovedBy);

public record TimesheetDetailDto(
    Guid Id, Guid EmployeeId, string EmployeeName, DateOnly PeriodStart,
    DateOnly PeriodEnd, decimal RegularHours, decimal OvertimeHours,
    decimal NightHours, decimal WeekendHours, decimal BreakHours,
    decimal NetPayableHours, decimal RegularPay, decimal OvertimePay,
    decimal TotalGrossPay, TimesheetStatus Status,
    IReadOnlyList<TimesheetEntryDto> Entries, string? Notes);

public record TimesheetEntryDto(
    Guid Id, DateOnly Date, DateTime? ClockIn, DateTime? ClockOut,
    decimal BreakMinutes, decimal RegularHours, decimal OvertimeHours,
    bool IsNoShow, string? Notes);

public record ApproveTimesheetRequest(string? Notes);

public record PayrollExportDto(
    Guid EmployeeId, string EmployeeName, string? EmployeeEmail,
    string? ContractType, decimal HourlyRate, decimal RegularHours,
    decimal OvertimeHours, decimal RegularPay, decimal OvertimePay,
    decimal TotalGrossPay, DateOnly PeriodStart, DateOnly PeriodEnd);
