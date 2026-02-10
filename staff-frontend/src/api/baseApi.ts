import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { RootState } from '../store/store';

export const baseApi = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({
    baseUrl: '/api',
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).auth.accessToken;
      if (token) headers.set('Authorization', `Bearer ${token}`);
      return headers;
    },
  }),
  tagTypes: [
    'Employee', 'Schedule', 'ShiftAssignment', 'TimeOff', 'Clock',
    'Timesheet', 'Notification', 'Announcement', 'Role', 'Station',
    'Department', 'Location', 'ShiftTemplate', 'LeaveType', 'Availability',
    'Dashboard', 'LeaveBalance',
  ],
  endpoints: () => ({}),
});
