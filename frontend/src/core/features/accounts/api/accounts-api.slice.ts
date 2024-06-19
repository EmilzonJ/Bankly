import {
  Account,
  AccountParams,
  UpdateAccount,
} from "@/core/types/account.type";
import { Paginated, PaginatedParams } from "@/core/types/paginated.type";
import { baseApi } from "@/core/store/base.api";
import { Transaction } from "@/core/types/transaction.type";

const baseApiChild = baseApi.enhanceEndpoints({
  addTagTypes: ["Account", "Transaction"],
});

export const accountsApi = baseApiChild.injectEndpoints({
  endpoints: (builder) => ({
    getAccounts: builder.query<
      Paginated<Account>,
      AccountParams & PaginatedParams
    >({
      query: ({ pageNumber, pageSize, ...filters }) => {
        const queryParams = new URLSearchParams({
          ...filters,
          pageNumber: pageNumber.toString(),
          pageSize: pageSize.toString(),
        }).toString();
        return `accounts?${queryParams}`;
      },
      providesTags: [{ type: "Account", id: "LIST" }],
    }),
    getAccountTransactions: builder.query<Transaction[], string | undefined>({
      query: (id) => `accounts/${id}/transactions`,
      providesTags: [{ type: "Transaction" }],
    }),
    getAccountById: builder.query<Account, string | undefined>({
      query: (id) => `accounts/${id}`,
      providesTags: [{ type: "Account" }],
    }),
    updateAccount: builder.mutation<Account, UpdateAccount>({
      query: (data) => ({
        url: `accounts/${data.id}`,
        method: "PUT",
        body: data,
      }),
      invalidatesTags: [{ type: "Account" }],
    }),
    deleteAccount: builder.mutation<void, string>({
      query: (id) => ({
        url: `accounts/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: [{ type: 'Account' }],
    }),
  }),
});

export const {
  useGetAccountsQuery,
  useGetAccountTransactionsQuery,
  useGetAccountByIdQuery,
  useUpdateAccountMutation,
  useDeleteAccountMutation
} = accountsApi;
