using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffPro.Application.Commands;
using StaffPro.Application.DTOs;
using StaffPro.Application.Interfaces;
using StaffPro.Application.Queries;

namespace StaffPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public EmployeesController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<EmployeeListDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null, [FromQuery] Guid? locationId = null,
        [FromQuery] Guid? roleId = null)
    {
        var result = await _mediator.Send(new GetEmployeesQuery(
            _currentUser.OrganizationId, page, pageSize, search, locationId, roleId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDetailDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<EmployeeDetailDto>> Create(CreateEmployeeRequest request)
    {
        var result = await _mediator.Send(new CreateEmployeeCommand(_currentUser.OrganizationId, request));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<EmployeeDetailDto>> Update(Guid id, UpdateEmployeeRequest request)
    {
        var result = await _mediator.Send(new UpdateEmployeeCommand(_currentUser.OrganizationId, id, request));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(_currentUser.OrganizationId, id));
        return NoContent();
    }

    [HttpPost("{id}/contract")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ContractDto>> CreateContract(Guid id, CreateContractRequest request)
    {
        var result = await _mediator.Send(new CreateContractCommand(_currentUser.OrganizationId, id, request));
        return Ok(result);
    }

    [HttpPut("{id}/roles")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<IReadOnlyList<EmployeeRoleDto>>> AssignRoles(
        Guid id, List<CreateEmployeeRoleRequest> roles)
    {
        var result = await _mediator.Send(new AssignRolesCommand(_currentUser.OrganizationId, id, roles));
        return Ok(result);
    }

    // ── Availability ──
    [HttpGet("{id}/availability")]
    public async Task<ActionResult<IReadOnlyList<AvailabilityDto>>> GetAvailability(Guid id)
    {
        var result = await _mediator.Send(new GetAvailabilityQuery(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpPut("{id}/availability")]
    public async Task<ActionResult<IReadOnlyList<AvailabilityDto>>> SetAvailability(
        Guid id, List<CreateAvailabilityRequest> items)
    {
        var result = await _mediator.Send(new SetAvailabilityCommand(_currentUser.OrganizationId, id, items));
        return Ok(result);
    }

    [HttpGet("{id}/availability/overrides")]
    public async Task<ActionResult<IReadOnlyList<AvailabilityOverrideDto>>> GetOverrides(
        Guid id, [FromQuery] DateOnly? from = null, [FromQuery] DateOnly? to = null)
    {
        var result = await _mediator.Send(new GetAvailabilityOverridesQuery(
            _currentUser.OrganizationId, id, from, to));
        return Ok(result);
    }

    [HttpPost("{id}/availability/overrides")]
    public async Task<ActionResult<AvailabilityOverrideDto>> CreateOverride(
        Guid id, CreateAvailabilityOverrideRequest request)
    {
        var result = await _mediator.Send(new CreateAvailabilityOverrideCommand(
            _currentUser.OrganizationId, id, request));
        return Ok(result);
    }

    // ── Leave Balances ──
    [HttpGet("{id}/leave-balances")]
    public async Task<ActionResult<IReadOnlyList<LeaveBalanceDto>>> GetLeaveBalances(
        Guid id, [FromQuery] int? year = null)
    {
        var result = await _mediator.Send(new GetLeaveBalancesQuery(
            _currentUser.OrganizationId, id, year));
        return Ok(result);
    }
}
