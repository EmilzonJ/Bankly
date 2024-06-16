import { Customer, CustomerParams } from "@/core/data/customer.type";
import { Paginated, PaginatedParams } from "@/core/data/paginated.type";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const baseUrl = import.meta.env.VITE_API_BASE_URL;

export const customersApi = createApi({
  reducerPath: "customersApi",
  baseQuery: fetchBaseQuery({
    baseUrl,
    // prepareHeaders: (headers) => {
    //   TODO: Add JWT
    //   return headers;
    // },
  }),
  tagTypes: ["Customer"],
  endpoints: (builder) => ({
    getCustomers: builder.query<Paginated<Customer>, CustomerParams & PaginatedParams>({
      query: ({ pageNumber, pageSize, ...filters }) => {
        const queryParams = new URLSearchParams({
          ...filters,
          pageNumber: pageNumber.toString(),
          pageSize: pageSize.toString(),
        }).toString();
        return `customers?${queryParams}`;
      },
      providesTags: [{ type: "Customer", id: "LIST" }],
    }),
    getCustomerAccounts: builder.query<Customer, string>({
      query: (id) => `customers/${id}/accounts`,
      providesTags: [{ type: "Customer", id: "DETAIL" }],
    }),
    getCustomerById: builder.query<Customer, string>({
      query: (id) => `customers/${id}`,
      providesTags: [{ type: "Customer", id: "DETAIL" }],
    }),
    createCustomer: builder.mutation<Customer, Partial<Customer>>({
      query: (body) => ({
        url: "customers",
        method: "POST",
        body,
      }),
      invalidatesTags: [{ type: "Customer", id: "LIST" }],
    }),
    updateCustomer: builder.mutation<Customer, Partial<Customer>>({
      query: ({ id, ...body }) => ({
        url: `customers/${id}`,
        method: "PUT",
        body,
      }),
      invalidatesTags: [{ type: "Customer", id: "DETAIL" }],
    }),
    deleteCustomer: builder.mutation<void, string>({
      query: (id) => ({
        url: `customers/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: [{ type: "Customer", id: "DETAIL" }],
    })
  }),
});

export const {
  useGetCustomersQuery,
  useGetCustomerByIdQuery,
  useCreateCustomerMutation,
} = customersApi;
