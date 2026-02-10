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
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public SchedulesController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SchedulePeriodDto>>> GetAll(
        [FromQuery] Guid? locationId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetSchedulePeriodsQuery(
            _currentUser.OrganizationId, locationId, page, pageSize));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SchedulePeriodDetailDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSchedulePeriodDetailQuery(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<SchedulePeriodDto>> Create(CreateSchedulePeriodRequest request)
    {
        var result = await _mediator.Send(new CreateSchedulePeriodCommand(_currentUser.OrganizationId, request));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id}/publish")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<SchedulePeriodDto>> Publish(Guid id)
    {
        var result = await _mediator.Send(new PublishScheduleCommand(
            _currentUser.OrganizationId, id, _currentUser.Email));
        return Ok(result);
    }

    [HttpPost("{id}/lock")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SchedulePeriodDto>> Lock(Guid id)
    {
        var result = await _mediator.Send(new LockScheduleCommand(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpPost("{id}/copy")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<SchedulePeriodDto>> Copy(Guid id, CopyScheduleRequest request)
    {
        var result = await _mediator.Send(new CopyScheduleCommand(_currentUser.OrganizationId, request));
        return Ok(result);
    }

    // ── Shift Assignments ──
    [HttpGet("assignments")]
    public async Task<ActionResult<IReadOnlyList<ShiftAssignmentDto>>> GetAssignments(
        [FromQuery] DateOnly start, [FromQuery] DateOnly end, [FromQuery] Guid? employeeId = null)
    {
        var result = await _mediator.Send(new GetAssignmentsForDateRangeQuery(
            _currentUser.OrganizationId, start, end, employeeId));
        return Ok(result);
    }

    [HttpPost("assignments")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ShiftAssignmentDto>> CreateAssignment(CreateShiftAssignmentRequest request)
    {
        var result = await _mediator.Send(new CreateShiftAssignmentCommand(_currentUser.OrganizationId, request));
        return Ok(result);
    }

    [HttpPut("assignments/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ShiftAssignmentDto>> UpdateAssignment(Guid id, UpdateShiftAssignmentRequest request)
    {
        var result = await _mediator.Send(new UpdateShiftAssignmentCommand(_currentUser.OrganizationId, id, request));
        return Ok(result);
    }

    [HttpDelete("assignments/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteAssignment(Guid id)
    {
        await _mediator.Send(new DeleteShiftAssignmentCommand(_currentUser.OrganizationId, id));
        return NoContent();
    }

    [HttpPost("assignments/check-conflicts")]
    public async Task<ActionResult<ConflictCheckResult>> CheckConflicts(
        [FromQuery] Guid employeeId, [FromQuery] DateOnly date,
        [FromQuery] TimeOnly startTime, [FromQuery] TimeOnly endTime,
        [FromQuery] Guid? excludeAssignmentId = null)
    {
        var result = await _mediator.Send(new CheckConflictsQuery(
            _currentUser.OrganizationId, employeeId, date, startTime, endTime, excludeAssignmentId));
        return Ok(result);
    }

    // ── Staffing Requirements ──
    [HttpPost("{id}/staffing-requirements")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<StaffingRequirementDto>> CreateStaffingRequirement(
        Guid id, CreateStaffingRequirementRequest request)
    {
        var result = await _mediator.Send(new CreateStaffingRequirementCommand(
            _currentUser.OrganizationId, request));
        return Ok(result);
    }
}
