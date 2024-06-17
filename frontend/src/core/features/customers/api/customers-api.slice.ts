import { Account, CreateAccount } from '@/core/data/accounts.type';
import {
  CreateCustomer,
  Customer,
  CustomerParams,
  UpdateCustomer,
} from '@/core/data/customer.type';
import { Paginated, PaginatedParams } from '@/core/data/paginated.type';
import { baseApi } from '@/core/store/base.api';

const baseApiChild = baseApi.enhanceEndpoints({
  addTagTypes: ['Customer', 'Account'],
});

export const customersApi = baseApiChild.injectEndpoints({
  endpoints: (builder) => ({
    getCustomers: builder.query<
      Paginated<Customer>,  
      CustomerParams & PaginatedParams
    >({
      query: ({ pageNumber, pageSize, ...filters }) => {
        const queryParams = new URLSearchParams({
          ...filters,
          pageNumber: pageNumber.toString(),
          pageSize: pageSize.toString(),
        }).toString();
        return `customers?${queryParams}`;
      },
      providesTags: [{ type: 'Customer', id: 'LIST' }],
    }),
    getCustomerAccounts: builder.query<Account[], string | undefined>({
      query: (id) => `customers/${id}/accounts`,
      providesTags: [{ type: 'Account' }],
    }),
    createAccount: builder.mutation<Account, CreateAccount>({
      query: ({ balance, customerId }) => ({
        url: `customers/${customerId}/accounts`,
        method: 'POST',
        body: { balance },
      }),
      invalidatesTags: [{ type: 'Account' }],
    }),
    getCustomerById: builder.query<Customer, string | undefined>({
      query: (id) => `customers/${id}`,
      providesTags: [{ type: 'Customer' }],
    }),
    createCustomer: builder.mutation<Customer, CreateCustomer>({
      query: (body) => ({
        url: 'customers',
        method: 'POST',
        body,
      }),
      invalidatesTags: [{ type: 'Customer', id: 'LIST' }],
    }),
    updateCustomer: builder.mutation<Customer, UpdateCustomer>({
      query: ({ id, ...body }) => ({
        url: `customers/${id}`,
        method: 'PUT',
        body,
      }),
      invalidatesTags: [{ type: 'Customer' }],
    }),
    deleteCustomer: builder.mutation<void, string>({
      query: (id) => ({
        url: `customers/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: [{ type: 'Customer' }],
    }),
  }),
});

export const {
  useGetCustomersQuery,
  useGetCustomerByIdQuery,
  useCreateCustomerMutation,
  useGetCustomerAccountsQuery,
  useCreateAccountMutation,
  useDeleteCustomerMutation,
  useUpdateCustomerMutation,
} = customersApi;
