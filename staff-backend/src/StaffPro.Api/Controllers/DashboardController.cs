using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffPro.Application.Interfaces;
using StaffPro.Application.Queries;

namespace StaffPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public DashboardController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("manager")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ManagerDashboardDto>> GetManagerDashboard()
    {
        var result = await _mediator.Send(new GetManagerDashboardQuery(_currentUser.OrganizationId));
        return Ok(result);
    }

    [HttpGet("employee")]
    public async Task<ActionResult<EmployeeDashboardDto>> GetEmployeeDashboard()
    {
        var employeeId = _currentUser.EmployeeId
            ?? throw new InvalidOperationException("No employee linked to this user");
        var result = await _mediator.Send(new GetEmployeeDashboardQuery(
            _currentUser.OrganizationId, employeeId));
        return Ok(result);
    }
}
