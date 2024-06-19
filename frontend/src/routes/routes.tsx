import CustomerDetailPage from "@/pages/customers/detail/Page";
import CustomerListPage from "@/pages/customers/list/Page";
import AccountListPage from "@/pages/accounts/list/Page";
import AccountDetailPage from "@/pages/accounts/detail/Page";
import AppLayout from "@/theme/layouts/AppLayout";
import { createBrowserRouter, Navigate, Outlet } from "react-router-dom";
import DashboardPage from "@/pages/Page";
import TransactionListPage from "@/pages/transactions/list/Page";

export enum Routes {
  LIST_RELATIVE_PATH = "list",

  CUSTOMERS_INDEX = "/customers",
  CUSTOMER_LIST_PAGE = `/customers/${Routes.LIST_RELATIVE_PATH}`,
  CUSTOMER_DETAIL_PAGE = "/customers/:customerId",
  CUSTOMER_DETAIL_RELATIVE_PATH = ":customerId",

  ACCOUNTS_INDEX = "/accounts",
  ACCOUNT_LIST_PAGE = `/accounts/${Routes.LIST_RELATIVE_PATH}`,
  ACCOUNT_DETAIL_PAGE = "/accounts/:accountId",
  ACCOUNT_DETAIL_RELATIVE_PATH = ":accountId",

  TRANSACTIONS_INDEX = "/transactions",
  TRANSACTION_LIST_PAGE = `/transactions/${Routes.LIST_RELATIVE_PATH}`,
  TRANSACTION_DETAIL_PAGE = "/transactions/:transactionId",
  TRANSACTION_DETAIL_RELATIVE_PATH = ":transactionId",
}

export const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <AppLayout>
        <Outlet />
      </AppLayout>
    ),
    children: [
      {
        index: true,
        element: <DashboardPage />,
      },
      {
        path: Routes.CUSTOMERS_INDEX,
        children: [
          {
            path: Routes.CUSTOMER_LIST_PAGE,
            element: <CustomerListPage />,
          },
          {
            path: Routes.CUSTOMER_DETAIL_PAGE,
            element: <CustomerDetailPage />,
          },
          {
            path: "",
            element: <Navigate to={Routes.LIST_RELATIVE_PATH} />,
          },
        ],
      },
      {
        path: Routes.ACCOUNTS_INDEX,
        children: [
          {
            path: Routes.ACCOUNT_LIST_PAGE,
            element: <AccountListPage />,
          },
          {
            path: Routes.ACCOUNT_DETAIL_PAGE,
            element: <AccountDetailPage />,
          },
          {
            path: "",
            element: <Navigate to={Routes.LIST_RELATIVE_PATH} />,
          },
        ],
      },
      {
        path: Routes.TRANSACTIONS_INDEX,
        children: [
          {
            path: Routes.TRANSACTION_LIST_PAGE,
            element: <TransactionListPage />,
          },
          {
            path: "",
            element: <Navigate to={Routes.LIST_RELATIVE_PATH} />,
          },
        ],
      },
    ],
  },
]);
