using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StaffPro.Application.Interfaces;

namespace StaffPro.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => Guid.Parse(
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());

    public Guid OrganizationId => Guid.Parse(
        _httpContextAccessor.HttpContext?.User.FindFirstValue("organizationId") ?? Guid.Empty.ToString());

    public string Email =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    public string Role =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? "Employee";

    public Guid? EmployeeId
    {
        get
        {
            var val = _httpContextAccessor.HttpContext?.User.FindFirstValue("employeeId");
            return !string.IsNullOrEmpty(val) && Guid.TryParse(val, out var id) ? id : null;
        }
    }
}
