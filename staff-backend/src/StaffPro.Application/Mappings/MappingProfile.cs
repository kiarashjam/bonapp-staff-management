using AutoMapper;
using StaffPro.Application.DTOs;
using StaffPro.Domain.Entities;
using StaffPro.Domain.Enums;

namespace StaffPro.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Organization
        CreateMap<Organization, OrganizationDto>();
        CreateMap<Organization, SchedulingRulesDto>();

        // Employee
        CreateMap<Employee, EmployeeListDto>()
            .ForMember(d => d.PrimaryRoleName, o => o.MapFrom(s =>
                s.EmployeeRoles.Where(er => er.IsPrimary).Select(er => er.Role.Name).FirstOrDefault()))
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location != null ? s.Location.Name : null));

        CreateMap<Employee, EmployeeDetailDto>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location != null ? s.Location.Name : null))
            .ForMember(d => d.ActiveContract, o => o.MapFrom(s => s.Contracts.FirstOrDefault(c => c.IsActive)))
            .ForMember(d => d.Roles, o => o.MapFrom(s => s.EmployeeRoles));

        CreateMap<Contract, ContractDto>()
            .ForMember(d => d.HourlyRate, o => o.MapFrom(s => s.HourlyRateCents / 100m))
            .ForMember(d => d.SalaryMonthly, o => o.MapFrom(s => s.SalaryMonthlyCents.HasValue ? s.SalaryMonthlyCents / 100m : (decimal?)null));

        CreateMap<EmployeeRole, EmployeeRoleDto>()
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name))
            .ForMember(d => d.RoleColor, o => o.MapFrom(s => s.Role.Color));

        // Roles & Settings
        CreateMap<Role, RoleDto>()
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : null));
        CreateMap<Station, StationDto>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location != null ? s.Location.Name : null))
            .ForMember(d => d.RoleIds, o => o.MapFrom(s => s.StationRoles.Select(sr => sr.RoleId)));
        CreateMap<Department, DepartmentDto>();
        CreateMap<Location, LocationDto>();
        CreateMap<ShiftTemplate, ShiftTemplateDto>();
        CreateMap<LeaveType, LeaveTypeDto>();

        // Schedule
        CreateMap<SchedulePeriod, SchedulePeriodDto>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.Name))
            .ForMember(d => d.TotalAssignments, o => o.MapFrom(s => s.ShiftAssignments.Count))
            .ForMember(d => d.TotalEmployees, o => o.MapFrom(s => s.ShiftAssignments.Select(a => a.EmployeeId).Distinct().Count()));

        CreateMap<SchedulePeriod, SchedulePeriodDetailDto>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.Name))
            .ForMember(d => d.Assignments, o => o.MapFrom(s => s.ShiftAssignments))
            .ForMember(d => d.StaffingRequirements, o => o.MapFrom(s => s.StaffingRequirements));

        CreateMap<ShiftAssignment, ShiftAssignmentDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FullName))
            .ForMember(d => d.EmployeePhotoUrl, o => o.MapFrom(s => s.Employee.ProfilePhotoUrl))
            .ForMember(d => d.ShiftTemplateName, o => o.MapFrom(s => s.ShiftTemplate != null ? s.ShiftTemplate.Name : null))
            .ForMember(d => d.StationName, o => o.MapFrom(s => s.Station != null ? s.Station.Name : null))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.Name : null))
            .ForMember(d => d.RoleColor, o => o.MapFrom(s => s.Role != null ? s.Role.Color : null));

        CreateMap<StaffingRequirement, StaffingRequirementDto>()
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name))
            .ForMember(d => d.ShiftTemplateName, o => o.MapFrom(s => s.ShiftTemplate != null ? s.ShiftTemplate.Name : null));

        // Time Off
        CreateMap<TimeOffRequest, TimeOffRequestDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FullName))
            .ForMember(d => d.LeaveTypeName, o => o.MapFrom(s => s.LeaveType.Name))
            .ForMember(d => d.LeaveTypeColor, o => o.MapFrom(s => s.LeaveType.Color));

        CreateMap<LeaveBalance, LeaveBalanceDto>()
            .ForMember(d => d.LeaveTypeName, o => o.MapFrom(s => s.LeaveType.Name));

        // Clock & Timesheet
        CreateMap<ClockEntry, ClockEntryDto>();
        CreateMap<Timesheet, TimesheetDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FullName))
            .ForMember(d => d.TotalGrossPay, o => o.MapFrom(s => s.TotalGrossPayCents / 100m));

        CreateMap<Timesheet, TimesheetDetailDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FullName))
            .ForMember(d => d.RegularPay, o => o.MapFrom(s => s.RegularPayCents / 100m))
            .ForMember(d => d.OvertimePay, o => o.MapFrom(s => s.OvertimePayCents / 100m))
            .ForMember(d => d.TotalGrossPay, o => o.MapFrom(s => s.TotalGrossPayCents / 100m));

        CreateMap<TimesheetEntry, TimesheetEntryDto>();

        // Notifications
        CreateMap<Notification, NotificationDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()));

        CreateMap<Announcement, AnnouncementDto>()
            .ForMember(d => d.ReadCount, o => o.MapFrom(s => s.ReadReceipts.Count));

        // Availability
        CreateMap<RecurringAvailability, AvailabilityDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()));
        CreateMap<AvailabilityOverride, AvailabilityOverrideDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()));
    }
}
