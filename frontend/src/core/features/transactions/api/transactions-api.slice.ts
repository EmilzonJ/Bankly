import { baseApi } from '@/core/store/base.api';
import {
  CreateTransaction,
  Transaction,
  TransactionParams,
  TransacttionById,
} from '@/core/types/transaction.type';
import { Paginated, PaginatedParams } from '@/core/types/paginated.type';

const baseApiChild = baseApi.enhanceEndpoints({
  addTagTypes: ['Transaction'],
});

export const transactionsApi = baseApiChild.injectEndpoints({
  endpoints: (builder) => ({
    getTransactions: builder.query<
      Paginated<Transaction>,
      TransactionParams & PaginatedParams
    >({
      query: ({ pageNumber, pageSize, ...filters }) => {
        const queryParams = new URLSearchParams({
          ...filters,
          pageNumber: pageNumber.toString(),
          pageSize: pageSize.toString(),
        }).toString();
        return `transactions?${queryParams}`;
      },
      providesTags: [{ type: 'Transaction', id: 'LIST' }],
    }),
    getTransactionById: builder.query<TransacttionById, string | undefined>({
      query: (id) => `transactions/${id}`,
      providesTags: [{ type: 'Transaction' }],
    }),
    createTransaction: builder.mutation<Transaction, CreateTransaction>({
      query: (body) => ({
        url: 'transactions',
        method: 'POST',
        body,
      }),
      invalidatesTags: [{ type: 'Transaction', id: 'LIST' }],
    }),
  }),
});

export const {
  useGetTransactionsQuery,
  useGetTransactionByIdQuery,
  useCreateTransactionMutation,
} = transactionsApi;
