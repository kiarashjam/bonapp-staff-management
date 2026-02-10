import { baseApi } from './baseApi';
import type { ClockStatus, ClockEntry, TimesheetSummary, PayrollExport, PagedResult } from '../types';

export const clockApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    clockAction: builder.mutation<ClockEntry, { entryType: string; latitude?: number; longitude?: number; gpsAccuracyMeters?: number; ipAddress?: string }>({
      query: (body) => ({ url: '/clock/action', method: 'POST', body }),
      invalidatesTags: ['Clock', 'Dashboard'],
    }),
    getClockStatus: builder.query<ClockStatus, string | void>({
      query: (employeeId) => ({ url: '/clock/status', params: employeeId ? { employeeId } : {} }),
      providesTags: ['Clock'],
    }),
    getClockEntries: builder.query<ClockEntry[], { employeeId: string; date: string }>({
      query: (params) => ({ url: '/clock/entries', params }),
      providesTags: ['Clock'],
    }),
    getTimesheets: builder.query<PagedResult<TimesheetSummary>, { page?: number; pageSize?: number; employeeId?: string; status?: string }>({
      query: (params) => ({ url: '/timesheets', params }),
      providesTags: ['Timesheet'],
    }),
    approveTimesheet: builder.mutation<TimesheetSummary, { id: string; notes?: string }>({
      query: ({ id, ...body }) => ({ url: `/timesheets/${id}/approve`, method: 'POST', body }),
      invalidatesTags: ['Timesheet', 'Dashboard'],
    }),
    getPayrollSummary: builder.query<PayrollExport[], { periodStart: string; periodEnd: string }>({
      query: (params) => ({ url: '/timesheets/payroll', params }),
    }),
  }),
});

export const {
  useClockActionMutation, useGetClockStatusQuery, useGetClockEntriesQuery,
  useGetTimesheetsQuery, useApproveTimesheetMutation, useGetPayrollSummaryQuery,
} = clockApi;
