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
public class TimeOffController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public TimeOffController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TimeOffRequestDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] Guid? employeeId = null, [FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetTimeOffRequestsQuery(
            _currentUser.OrganizationId, page, pageSize, employeeId, status));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeOffRequestDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTimeOffRequestByIdQuery(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TimeOffRequestDto>> Create(CreateTimeOffRequest request)
    {
        var result = await _mediator.Send(new CreateTimeOffRequestCommand(_currentUser.OrganizationId, request));
        return Ok(result);
    }

    [HttpPost("{id}/review")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TimeOffRequestDto>> Review(Guid id, ReviewTimeOffRequest request)
    {
        var result = await _mediator.Send(new ReviewTimeOffRequestCommand(
            _currentUser.OrganizationId, id, request, _currentUser.Email));
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelTimeOffRequestCommand(_currentUser.OrganizationId, id));
        return Ok(new { message = "Request cancelled" });
    }

    [HttpGet("staffing-impact")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<IReadOnlyList<StaffingImpactDto>>> GetStaffingImpact(
        [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var result = await _mediator.Send(new GetStaffingImpactQuery(
            _currentUser.OrganizationId, startDate, endDate));
        return Ok(result);
    }
}
