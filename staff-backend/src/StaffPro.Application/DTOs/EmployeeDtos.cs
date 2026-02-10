using StaffPro.Domain.Enums;

namespace StaffPro.Application.DTOs;

public record EmployeeListDto(
    Guid Id, string FirstName, string LastName, string Email, string? Phone,
    string? ProfilePhotoUrl, EmployeeStatus Status, string? PrimaryRoleName,
    string? LocationName, DateTime HireDate);

public record EmployeeDetailDto(
    Guid Id, string FirstName, string LastName, string Email, string? Phone,
    string? ProfilePhotoUrl, DateTime? DateOfBirth, string? Address, string? City,
    string? PostalCode, DateTime HireDate, DateTime? TerminationDate,
    EmployeeStatus Status, string? EmergencyContactName, string? EmergencyContactPhone,
    string? EmergencyContactRelation, Guid? LocationId, string? LocationName,
    ContractDto? ActiveContract, IReadOnlyList<EmployeeRoleDto> Roles);

public record CreateEmployeeRequest(
    string FirstName, string LastName, string Email, string? Phone,
    DateTime? DateOfBirth, string? Address, string? City, string? PostalCode,
    DateTime HireDate, Guid? LocationId, CreateContractRequest? Contract,
    IReadOnlyList<CreateEmployeeRoleRequest>? Roles);

public record UpdateEmployeeRequest(
    string FirstName, string LastName, string Email, string? Phone,
    DateTime? DateOfBirth, string? Address, string? City, string? PostalCode,
    EmployeeStatus Status, string? EmergencyContactName,
    string? EmergencyContactPhone, string? EmergencyContactRelation, Guid? LocationId);

public record ContractDto(
    Guid Id, ContractType ContractType, decimal ContractedHoursPerWeek,
    decimal HourlyRate, decimal? SalaryMonthly, DateTime StartDate,
    DateTime? EndDate, DateTime? ProbationEndDate, int NoticePeriodDays, bool IsActive);

public record CreateContractRequest(
    ContractType ContractType, decimal ContractedHoursPerWeek,
    decimal HourlyRate, decimal? SalaryMonthly, DateTime StartDate,
    DateTime? EndDate, DateTime? ProbationEndDate, int NoticePeriodDays);

public record EmployeeRoleDto(Guid RoleId, string RoleName, string RoleColor, ProficiencyLevel ProficiencyLevel, bool IsPrimary);
public record CreateEmployeeRoleRequest(Guid RoleId, ProficiencyLevel ProficiencyLevel, bool IsPrimary);
