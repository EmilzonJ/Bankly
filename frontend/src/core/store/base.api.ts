import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const baseUrl = import.meta.env.VITE_API_BASE_URL;

export const baseApi = createApi({
  reducerPath: 'baseApi',
  baseQuery: fetchBaseQuery({
    baseUrl,
    // prepareHeaders: (headers) => {
    //   TODO: Add JWT
    //   return headers;
    // },
  }),
  endpoints: () => ({}),
});
