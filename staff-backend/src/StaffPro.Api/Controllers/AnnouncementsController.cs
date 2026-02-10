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
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public AnnouncementsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AnnouncementDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetAnnouncementsQuery(_currentUser.OrganizationId, page, pageSize));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<AnnouncementDto>> Create(CreateAnnouncementRequest request)
    {
        var result = await _mediator.Send(new CreateAnnouncementCommand(
            _currentUser.OrganizationId, request, _currentUser.Email));
        return Ok(result);
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var employeeId = _currentUser.EmployeeId
            ?? throw new InvalidOperationException("No employee linked");
        await _mediator.Send(new MarkAnnouncementReadCommand(
            _currentUser.OrganizationId, id, employeeId));
        return Ok();
    }
}
