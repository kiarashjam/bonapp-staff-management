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
public class TimesheetsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public TimesheetsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TimesheetDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] Guid? employeeId = null, [FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetTimesheetsQuery(
            _currentUser.OrganizationId, page, pageSize, employeeId, status));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimesheetDetailDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTimesheetDetailQuery(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpPost("{id}/submit")]
    public async Task<ActionResult<TimesheetDto>> Submit(Guid id)
    {
        var result = await _mediator.Send(new SubmitTimesheetCommand(_currentUser.OrganizationId, id));
        return Ok(result);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TimesheetDto>> Approve(Guid id, ApproveTimesheetRequest request)
    {
        var result = await _mediator.Send(new ApproveTimesheetCommand(
            _currentUser.OrganizationId, id, _currentUser.Email, request.Notes));
        return Ok(result);
    }

    [HttpGet("payroll")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<IReadOnlyList<PayrollExportDto>>> GetPayrollSummary(
        [FromQuery] DateOnly periodStart, [FromQuery] DateOnly periodEnd)
    {
        var result = await _mediator.Send(new GetPayrollSummaryQuery(
            _currentUser.OrganizationId, periodStart, periodEnd));
        return Ok(result);
    }

    [HttpGet("payroll/export")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExportPayroll(
        [FromQuery] DateOnly periodStart, [FromQuery] DateOnly periodEnd,
        [FromServices] IPayrollExportService payrollService)
    {
        var csv = await payrollService.ExportToCsvAsync(
            _currentUser.OrganizationId, periodStart, periodEnd);
        return File(csv, "text/csv", $"payroll_{periodStart}_{periodEnd}.csv");
    }
}
