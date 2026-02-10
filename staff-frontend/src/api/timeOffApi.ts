import { baseApi } from './baseApi';
import type { TimeOffRequest, PagedResult } from '../types';

export const timeOffApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getTimeOffRequests: builder.query<PagedResult<TimeOffRequest>, { page?: number; pageSize?: number; employeeId?: string; status?: string }>({
      query: (params) => ({ url: '/timeoff', params }),
      providesTags: ['TimeOff'],
    }),
    createTimeOffRequest: builder.mutation<TimeOffRequest, Record<string, unknown>>({
      query: (body) => ({ url: '/timeoff', method: 'POST', body }),
      invalidatesTags: ['TimeOff', 'Dashboard', 'LeaveBalance'],
    }),
    reviewTimeOffRequest: builder.mutation<TimeOffRequest, { id: string; approved: boolean; denialReason?: string }>({
      query: ({ id, ...body }) => ({ url: `/timeoff/${id}/review`, method: 'POST', body }),
      invalidatesTags: ['TimeOff', 'Dashboard', 'LeaveBalance'],
    }),
    cancelTimeOffRequest: builder.mutation<void, string>({
      query: (id) => ({ url: `/timeoff/${id}/cancel`, method: 'POST' }),
      invalidatesTags: ['TimeOff', 'Dashboard', 'LeaveBalance'],
    }),
  }),
});

export const {
  useGetTimeOffRequestsQuery, useCreateTimeOffRequestMutation,
  useReviewTimeOffRequestMutation, useCancelTimeOffRequestMutation,
} = timeOffApi;
