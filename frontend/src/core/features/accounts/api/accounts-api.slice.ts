import { baseApi } from '@/core/store/base.api';

const baseApiChild = baseApi.enhanceEndpoints({
  addTagTypes: ['Accounts'],
});

export const accountsApi = baseApiChild.injectEndpoints({
  endpoints: (builder) => ({}),
});

export const {} = accountsApi;
