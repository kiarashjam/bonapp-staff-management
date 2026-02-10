using MediatR;
using StaffPro.Application.DTOs;

namespace StaffPro.Application.Commands;

public record CreateEmployeeCommand(Guid OrgId, CreateEmployeeRequest Dto) : IRequest<EmployeeDetailDto>;
public record UpdateEmployeeCommand(Guid OrgId, Guid EmployeeId, UpdateEmployeeRequest Dto) : IRequest<EmployeeDetailDto>;
public record DeleteEmployeeCommand(Guid OrgId, Guid EmployeeId) : IRequest<Unit>;
public record CreateContractCommand(Guid OrgId, Guid EmployeeId, CreateContractRequest Dto) : IRequest<ContractDto>;
public record AssignRolesCommand(Guid OrgId, Guid EmployeeId, IReadOnlyList<CreateEmployeeRoleRequest> Roles) : IRequest<IReadOnlyList<EmployeeRoleDto>>;
