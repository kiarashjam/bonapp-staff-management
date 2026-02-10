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
public class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public SettingsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    // ── Organization ──
    [HttpGet("organization")]
    public async Task<ActionResult<OrganizationDto>> GetOrganization()
        => Ok(await _mediator.Send(new GetOrganizationQuery(_currentUser.OrganizationId)));

    [HttpPut("organization")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrganizationDto>> UpdateOrganization(UpdateOrganizationRequest request)
        => Ok(await _mediator.Send(new UpdateOrganizationCommand(_currentUser.OrganizationId, request)));

    [HttpGet("scheduling-rules")]
    public async Task<ActionResult<SchedulingRulesDto>> GetSchedulingRules()
        => Ok(await _mediator.Send(new GetSchedulingRulesQuery(_currentUser.OrganizationId)));

    [HttpPut("scheduling-rules")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SchedulingRulesDto>> UpdateSchedulingRules(UpdateSchedulingRulesRequest request)
        => Ok(await _mediator.Send(new UpdateSchedulingRulesCommand(_currentUser.OrganizationId, request)));

    // ── Roles ──
    [HttpGet("roles")]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetRoles()
        => Ok(await _mediator.Send(new GetRolesQuery(_currentUser.OrganizationId)));

    [HttpPost("roles")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleRequest request)
        => Ok(await _mediator.Send(new CreateRoleCommand(_currentUser.OrganizationId, request)));

    [HttpPut("roles/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoleDto>> UpdateRole(Guid id, UpdateRoleRequest request)
        => Ok(await _mediator.Send(new UpdateRoleCommand(_currentUser.OrganizationId, id, request)));

    [HttpDelete("roles/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        await _mediator.Send(new DeleteRoleCommand(_currentUser.OrganizationId, id));
        return NoContent();
    }

    // ── Stations ──
    [HttpGet("stations")]
    public async Task<ActionResult<IReadOnlyList<StationDto>>> GetStations()
        => Ok(await _mediator.Send(new GetStationsQuery(_currentUser.OrganizationId)));

    [HttpPost("stations")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StationDto>> CreateStation(CreateStationRequest request)
        => Ok(await _mediator.Send(new CreateStationCommand(_currentUser.OrganizationId, request)));

    [HttpPut("stations/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StationDto>> UpdateStation(Guid id, UpdateStationRequest request)
        => Ok(await _mediator.Send(new UpdateStationCommand(_currentUser.OrganizationId, id, request)));

    // ── Departments ──
    [HttpGet("departments")]
    public async Task<ActionResult<IReadOnlyList<DepartmentDto>>> GetDepartments()
        => Ok(await _mediator.Send(new GetDepartmentsQuery(_currentUser.OrganizationId)));

    [HttpPost("departments")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentRequest request)
        => Ok(await _mediator.Send(new CreateDepartmentCommand(_currentUser.OrganizationId, request)));

    // ── Locations ──
    [HttpGet("locations")]
    public async Task<ActionResult<IReadOnlyList<LocationDto>>> GetLocations()
        => Ok(await _mediator.Send(new GetLocationsQuery(_currentUser.OrganizationId)));

    [HttpPost("locations")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<LocationDto>> CreateLocation(CreateLocationRequest request)
        => Ok(await _mediator.Send(new CreateLocationCommand(_currentUser.OrganizationId, request)));

    // ── Shift Templates ──
    [HttpGet("shift-templates")]
    public async Task<ActionResult<IReadOnlyList<ShiftTemplateDto>>> GetShiftTemplates()
        => Ok(await _mediator.Send(new GetShiftTemplatesQuery(_currentUser.OrganizationId)));

    [HttpPost("shift-templates")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ShiftTemplateDto>> CreateShiftTemplate(CreateShiftTemplateRequest request)
        => Ok(await _mediator.Send(new CreateShiftTemplateCommand(_currentUser.OrganizationId, request)));

    [HttpPut("shift-templates/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ShiftTemplateDto>> UpdateShiftTemplate(Guid id, CreateShiftTemplateRequest request)
        => Ok(await _mediator.Send(new UpdateShiftTemplateCommand(_currentUser.OrganizationId, id, request)));

    // ── Leave Types ──
    [HttpGet("leave-types")]
    public async Task<ActionResult<IReadOnlyList<LeaveTypeDto>>> GetLeaveTypes()
        => Ok(await _mediator.Send(new GetLeaveTypesQuery(_currentUser.OrganizationId)));

    [HttpPost("leave-types")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<LeaveTypeDto>> CreateLeaveType(CreateLeaveTypeRequest request)
        => Ok(await _mediator.Send(new CreateLeaveTypeCommand(_currentUser.OrganizationId, request)));
}
