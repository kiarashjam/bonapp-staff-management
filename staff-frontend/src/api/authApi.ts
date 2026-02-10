import { baseApi } from './baseApi';
import type { LoginRequest, LoginResponse, RegisterRequest, User } from '../types';

export const authApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    login: builder.mutation<LoginResponse, LoginRequest>({
      query: (body) => ({ url: '/auth/login', method: 'POST', body }),
    }),
    register: builder.mutation<LoginResponse, RegisterRequest>({
      query: (body) => ({ url: '/auth/register', method: 'POST', body }),
    }),
    getMe: builder.query<User, void>({
      query: () => '/auth/me',
    }),
    changePassword: builder.mutation<void, { currentPassword: string; newPassword: string }>({
      query: (body) => ({ url: '/auth/change-password', method: 'POST', body }),
    }),
    inviteUser: builder.mutation<void, { email: string; firstName: string; lastName: string; role: string; employeeId?: string }>({
      query: (body) => ({ url: '/auth/invite', method: 'POST', body }),
    }),
  }),
});

export const { useLoginMutation, useRegisterMutation, useGetMeQuery, useChangePasswordMutation, useInviteUserMutation } = authApi;
