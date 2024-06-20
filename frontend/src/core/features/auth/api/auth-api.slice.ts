import { baseApi } from "@/core/store/base.api";
import { Login, Auth } from "@/core/types/auth.type";

export const accountsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    login: builder.mutation<Auth, Login>({
      query: (body) => ({
        url: "auth/login",
        method: "POST",
        body: body,
      }),
    }),
  }),
});

export const { useLoginMutation } = accountsApi;
