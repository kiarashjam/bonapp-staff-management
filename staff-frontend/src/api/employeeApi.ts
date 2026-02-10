import { baseApi } from './baseApi';
import type { EmployeeList, EmployeeDetail, PagedResult, Availability, AvailabilityOverride, LeaveBalance } from '../types';

export const employeeApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getEmployees: builder.query<PagedResult<EmployeeList>, { page?: number; pageSize?: number; search?: string; locationId?: string; roleId?: string }>({
      query: (params) => ({ url: '/employees', params }),
      providesTags: ['Employee'],
    }),
    getEmployee: builder.query<EmployeeDetail, string>({
      query: (id) => `/employees/${id}`,
      providesTags: (_r, _e, id) => [{ type: 'Employee', id }],
    }),
    createEmployee: builder.mutation<EmployeeDetail, Record<string, unknown>>({
      query: (body) => ({ url: '/employees', method: 'POST', body }),
      invalidatesTags: ['Employee', 'Dashboard'],
    }),
    updateEmployee: builder.mutation<EmployeeDetail, { id: string; body: Record<string, unknown> }>({
      query: ({ id, body }) => ({ url: `/employees/${id}`, method: 'PUT', body }),
      invalidatesTags: (_r, _e, { id }) => [{ type: 'Employee', id }, 'Dashboard'],
    }),
    deleteEmployee: builder.mutation<void, string>({
      query: (id) => ({ url: `/employees/${id}`, method: 'DELETE' }),
      invalidatesTags: ['Employee', 'Dashboard'],
    }),
    assignRoles: builder.mutation<unknown, { id: string; roles: Array<{ roleId: string; proficiencyLevel: number; isPrimary: boolean }> }>({
      query: ({ id, roles }) => ({ url: `/employees/${id}/roles`, method: 'PUT', body: roles }),
      invalidatesTags: (_r, _e, { id }) => [{ type: 'Employee', id }],
    }),
    createContract: builder.mutation<unknown, { id: string; body: Record<string, unknown> }>({
      query: ({ id, body }) => ({ url: `/employees/${id}/contract`, method: 'POST', body }),
      invalidatesTags: (_r, _e, { id }) => [{ type: 'Employee', id }],
    }),
    getAvailability: builder.query<Availability[], string>({
      query: (id) => `/employees/${id}/availability`,
      providesTags: ['Availability'],
    }),
    setAvailability: builder.mutation<Availability[], { id: string; items: Array<Record<string, unknown>> }>({
      query: ({ id, items }) => ({ url: `/employees/${id}/availability`, method: 'PUT', body: items }),
      invalidatesTags: ['Availability'],
    }),
    getOverrides: builder.query<AvailabilityOverride[], { id: string; from?: string; to?: string }>({
      query: ({ id, ...params }) => ({ url: `/employees/${id}/availability/overrides`, params }),
      providesTags: ['Availability'],
    }),
    createOverride: builder.mutation<AvailabilityOverride, { id: string; body: Record<string, unknown> }>({
      query: ({ id, body }) => ({ url: `/employees/${id}/availability/overrides`, method: 'POST', body }),
      invalidatesTags: ['Availability'],
    }),
    getLeaveBalances: builder.query<LeaveBalance[], { id: string; year?: number }>({
      query: ({ id, ...params }) => ({ url: `/employees/${id}/leave-balances`, params }),
      providesTags: ['LeaveBalance'],
    }),
  }),
});

export const {
  useGetEmployeesQuery, useGetEmployeeQuery, useCreateEmployeeMutation,
  useUpdateEmployeeMutation, useDeleteEmployeeMutation, useAssignRolesMutation,
  useCreateContractMutation, useGetAvailabilityQuery, useSetAvailabilityMutation,
  useGetOverridesQuery, useCreateOverrideMutation, useGetLeaveBalancesQuery,
} = employeeApi;
