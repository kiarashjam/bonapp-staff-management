import { baseApi } from './baseApi';
import type { SchedulePeriod, SchedulePeriodDetail, ShiftAssignment, ConflictCheck, PagedResult } from '../types';

export const scheduleApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getSchedules: builder.query<PagedResult<SchedulePeriod>, { locationId?: string; page?: number; pageSize?: number }>({
      query: (params) => ({ url: '/schedules', params }),
      providesTags: ['Schedule'],
    }),
    getScheduleDetail: builder.query<SchedulePeriodDetail, string>({
      query: (id) => `/schedules/${id}`,
      providesTags: (_r, _e, id) => [{ type: 'Schedule', id }],
    }),
    createSchedule: builder.mutation<SchedulePeriod, { locationId: string; startDate: string; endDate: string; notes?: string }>({
      query: (body) => ({ url: '/schedules', method: 'POST', body }),
      invalidatesTags: ['Schedule'],
    }),
    publishSchedule: builder.mutation<SchedulePeriod, string>({
      query: (id) => ({ url: `/schedules/${id}/publish`, method: 'POST' }),
      invalidatesTags: ['Schedule', 'Notification'],
    }),
    lockSchedule: builder.mutation<SchedulePeriod, string>({
      query: (id) => ({ url: `/schedules/${id}/lock`, method: 'POST' }),
      invalidatesTags: ['Schedule'],
    }),
    copySchedule: builder.mutation<SchedulePeriod, { id: string; sourceSchedulePeriodId: string; newStartDate: string }>({
      query: ({ id, ...body }) => ({ url: `/schedules/${id}/copy`, method: 'POST', body }),
      invalidatesTags: ['Schedule'],
    }),
    getAssignments: builder.query<ShiftAssignment[], { start: string; end: string; employeeId?: string }>({
      query: (params) => ({ url: '/schedules/assignments', params }),
      providesTags: ['ShiftAssignment'],
    }),
    createAssignment: builder.mutation<ShiftAssignment, Record<string, unknown>>({
      query: (body) => ({ url: '/schedules/assignments', method: 'POST', body }),
      invalidatesTags: ['ShiftAssignment', 'Schedule', 'Dashboard'],
    }),
    updateAssignment: builder.mutation<ShiftAssignment, { id: string; body: Record<string, unknown> }>({
      query: ({ id, body }) => ({ url: `/schedules/assignments/${id}`, method: 'PUT', body }),
      invalidatesTags: ['ShiftAssignment', 'Schedule'],
    }),
    deleteAssignment: builder.mutation<void, string>({
      query: (id) => ({ url: `/schedules/assignments/${id}`, method: 'DELETE' }),
      invalidatesTags: ['ShiftAssignment', 'Schedule', 'Dashboard'],
    }),
    checkConflicts: builder.query<ConflictCheck, { employeeId: string; date: string; startTime: string; endTime: string; excludeAssignmentId?: string }>({
      query: (params) => ({ url: '/schedules/assignments/check-conflicts', params }),
    }),
  }),
});

export const {
  useGetSchedulesQuery, useGetScheduleDetailQuery, useCreateScheduleMutation,
  usePublishScheduleMutation, useLockScheduleMutation, useCopyScheduleMutation,
  useGetAssignmentsQuery, useCreateAssignmentMutation, useUpdateAssignmentMutation,
  useDeleteAssignmentMutation, useCheckConflictsQuery,
} = scheduleApi;
