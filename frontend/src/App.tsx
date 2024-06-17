import CustomerDetailPage from './pages/customers/DetailPage';
import CustomerListPage from './pages/customers/list/CustomersListPage';
import AppLayout from './theme/layouts/AppLayout';

import {
  createBrowserRouter,
  Navigate,
  Outlet,
  RouterProvider,
} from 'react-router-dom';

const router = createBrowserRouter([
  {
    path: '/',
    element: (
      <AppLayout>
        <Outlet />
      </AppLayout>
    ),
    children: [
      {
        path: '/customers',
        children: [
          {
            path: 'list',
            element: <CustomerListPage />,
          },
          {
            path: 'detail/:customerId',
            element: <CustomerDetailPage />,
          },
          {
            path: '',
            element: <Navigate to='list' />,
          },
        ],
      },
    ],
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
