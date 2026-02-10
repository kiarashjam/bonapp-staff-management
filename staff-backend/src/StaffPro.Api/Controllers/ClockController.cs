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
public class ClockController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public ClockController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpPost("action")]
    public async Task<ActionResult<ClockEntryDto>> ClockAction(ClockActionRequest request)
    {
        var employeeId = _currentUser.EmployeeId
            ?? throw new InvalidOperationException("No employee linked to this user");
        var result = await _mediator.Send(new ClockActionCommand(
            _currentUser.OrganizationId, employeeId, request));
        return Ok(result);
    }

    [HttpGet("status")]
    public async Task<ActionResult<ClockStatusDto>> GetStatus([FromQuery] Guid? employeeId = null)
    {
        var id = employeeId ?? _currentUser.EmployeeId
            ?? throw new InvalidOperationException("No employee linked");
        var result = await _mediator.Send(new GetClockStatusQuery(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpGet("entries")]
    public async Task<ActionResult<IReadOnlyList<ClockEntryDto>>> GetEntries(
        [FromQuery] Guid employeeId, [FromQuery] DateOnly date)
    {
        var result = await _mediator.Send(new GetClockEntriesQuery(
            _currentUser.OrganizationId, employeeId, date));
        return Ok(result);
    }

    [HttpPost("override")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ClockEntryDto>> ManualOverride(ManualClockOverrideRequest request)
    {
        var result = await _mediator.Send(new ManualClockOverrideCommand(
            _currentUser.OrganizationId, request, _currentUser.Email));
        return Ok(result);
    }
}
