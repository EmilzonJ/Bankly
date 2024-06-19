import {
  useLazyGetCustomerAccountsQuery,
  useLazyGetCustomersQuery,
} from '@/core/features/customers/api/customers-api.slice';
import CreateTransactionPresentation from '@/core/features/transactions/components/CreatePresentation';
import { PageContainer } from '@ant-design/pro-components';

function TransactionNewPage() {
  const [getCustomerAccounts] = useLazyGetCustomerAccountsQuery();
  const [getCustomers, { isLoading, isFetching }] = useLazyGetCustomersQuery();

  const handleGetCustomer = async (name: string) => {
    const data = await getCustomers({
      name,
      pageNumber: 1,
      pageSize: 20,
    }).unwrap();
    return data.items;
  };

  const handleGetAccounts = async (customerId: string) => {
    console.log(customerId);

    const data = await getCustomerAccounts(customerId).unwrap();
    return data;
  };

  return (
    <PageContainer>
      <CreateTransactionPresentation
        customerMeta={{
          handleGetCustomers: handleGetCustomer,
          loading: isLoading || isFetching,
        }}
        handleGetAccounts={handleGetAccounts}
      />
    </PageContainer>
  );
}

export default TransactionNewPage;
