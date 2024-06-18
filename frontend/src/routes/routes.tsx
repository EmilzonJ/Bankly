import CustomerDetailPage from '@/pages/customers/detail/Page';
import CustomerListPage from '@/pages/customers/list/Page';
import AppLayout from '@/theme/layouts/AppLayout';
import {
    createBrowserRouter,
    Navigate,
    Outlet
} from 'react-router-dom';

export enum Routes {
    CUSTOMERS_INDEX = '/customers',
    CUSTOMER_LIST_PAGE = '/customers/list',
    CUSTOMER_LIST_RELATIVE_PATH = 'list',
    CUSTOMER_DETAIL_PAGE = '/customers/:customerId',
    CUSTOMER_DETAIL_RELATIVE_PATH = ':customerId',
}

export const router = createBrowserRouter([
    {
      path: '/',
      element: (
        <AppLayout>
          <Outlet />
        </AppLayout>
      ),
      children: [
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
              path: '',
              element: <Navigate to={Routes.CUSTOMER_LIST_RELATIVE_PATH} />,
            },
          ],
        },
      ],
    },
  ]);