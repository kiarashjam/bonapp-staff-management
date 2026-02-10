import { baseApi } from './baseApi';
import type { Role, Station, Department, Location, ShiftTemplate, LeaveType, Notification, Announcement } from '../types';

export const settingsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    // Roles
    getRoles: builder.query<Role[], void>({ query: () => '/settings/roles', providesTags: ['Role'] }),
    createRole: builder.mutation<Role, Record<string, unknown>>({ query: (body) => ({ url: '/settings/roles', method: 'POST', body }), invalidatesTags: ['Role'] }),
    updateRole: builder.mutation<Role, { id: string; body: Record<string, unknown> }>({ query: ({ id, body }) => ({ url: `/settings/roles/${id}`, method: 'PUT', body }), invalidatesTags: ['Role'] }),
    deleteRole: builder.mutation<void, string>({ query: (id) => ({ url: `/settings/roles/${id}`, method: 'DELETE' }), invalidatesTags: ['Role'] }),

    // Stations
    getStations: builder.query<Station[], void>({ query: () => '/settings/stations', providesTags: ['Station'] }),
    createStation: builder.mutation<Station, Record<string, unknown>>({ query: (body) => ({ url: '/settings/stations', method: 'POST', body }), invalidatesTags: ['Station'] }),

    // Departments
    getDepartments: builder.query<Department[], void>({ query: () => '/settings/departments', providesTags: ['Department'] }),
    createDepartment: builder.mutation<Department, Record<string, unknown>>({ query: (body) => ({ url: '/settings/departments', method: 'POST', body }), invalidatesTags: ['Department'] }),

    // Locations
    getLocations: builder.query<Location[], void>({ query: () => '/settings/locations', providesTags: ['Location'] }),
    createLocation: builder.mutation<Location, Record<string, unknown>>({ query: (body) => ({ url: '/settings/locations', method: 'POST', body }), invalidatesTags: ['Location'] }),

    // Shift Templates
    getShiftTemplates: builder.query<ShiftTemplate[], void>({ query: () => '/settings/shift-templates', providesTags: ['ShiftTemplate'] }),
    createShiftTemplate: builder.mutation<ShiftTemplate, Record<string, unknown>>({ query: (body) => ({ url: '/settings/shift-templates', method: 'POST', body }), invalidatesTags: ['ShiftTemplate'] }),

    // Leave Types
    getLeaveTypes: builder.query<LeaveType[], void>({ query: () => '/settings/leave-types', providesTags: ['LeaveType'] }),
    createLeaveType: builder.mutation<LeaveType, Record<string, unknown>>({ query: (body) => ({ url: '/settings/leave-types', method: 'POST', body }), invalidatesTags: ['LeaveType'] }),

    // Notifications
    getNotifications: builder.query<{ items: Notification[]; unreadCount: number }, { page?: number; pageSize?: number }>({
      query: (params) => ({ url: '/notifications', params }),
      providesTags: ['Notification'],
    }),
    markNotificationRead: builder.mutation<void, string>({ query: (id) => ({ url: `/notifications/${id}/read`, method: 'POST' }), invalidatesTags: ['Notification'] }),
    markAllNotificationsRead: builder.mutation<void, void>({ query: () => ({ url: '/notifications/read-all', method: 'POST' }), invalidatesTags: ['Notification'] }),

    // Announcements
    getAnnouncements: builder.query<{ items: Announcement[]; totalCount: number }, { page?: number; pageSize?: number }>({
      query: (params) => ({ url: '/announcements', params }),
      providesTags: ['Announcement'],
    }),
    createAnnouncement: builder.mutation<Announcement, Record<string, unknown>>({ query: (body) => ({ url: '/announcements', method: 'POST', body }), invalidatesTags: ['Announcement'] }),

    // Dashboard
    getManagerDashboard: builder.query<Record<string, unknown>, void>({ query: () => '/dashboard/manager', providesTags: ['Dashboard'] }),
    getEmployeeDashboard: builder.query<Record<string, unknown>, void>({ query: () => '/dashboard/employee', providesTags: ['Dashboard'] }),
  }),
});

export const {
  useGetRolesQuery, useCreateRoleMutation, useUpdateRoleMutation, useDeleteRoleMutation,
  useGetStationsQuery, useCreateStationMutation,
  useGetDepartmentsQuery, useCreateDepartmentMutation,
  useGetLocationsQuery, useCreateLocationMutation,
  useGetShiftTemplatesQuery, useCreateShiftTemplateMutation,
  useGetLeaveTypesQuery, useCreateLeaveTypeMutation,
  useGetNotificationsQuery, useMarkNotificationReadMutation, useMarkAllNotificationsReadMutation,
  useGetAnnouncementsQuery, useCreateAnnouncementMutation,
  useGetManagerDashboardQuery, useGetEmployeeDashboardQuery,
} = settingsApi;
