import { CreateAccount } from '@/core/types/account.type';

import {
  useCreateAccountMutation,
  useGetCustomerAccountsQuery,
  useGetCustomerByIdQuery,
} from '@/core/features/customers/api/customers-api.slice';
import CustomerDetailPresentation from '@/core/features/customers/components/DetailPresentation';
import { PageContainer } from '@ant-design/pro-components';
import { useParams } from 'react-router-dom';

function CustomerDetailPage() {
  const { customerId } = useParams<{ customerId: string }>();
  const { data: customer } = useGetCustomerByIdQuery(customerId, {
    skip: !customerId,
  });

  const {
    data: accounts,
    isLoading: loadingTable,
    refetch: reloadAccounts,
  } = useGetCustomerAccountsQuery(customerId, {
    skip: !customerId,
  });

  const [createAccountMutation] = useCreateAccountMutation();

  const handleCreateAccount = async ({
    balance,
    alias
  }: Omit<CreateAccount, 'customerId'>) => {
    if (!customerId) return;
    await createAccountMutation({
      balance,
      customerId: customerId as string,
      alias
    }).unwrap();
  };

  return (
    <PageContainer title={customer?.name}>
      <CustomerDetailPresentation
        onCreateAccount={handleCreateAccount}
        data={customer}
        customerAccounts={accounts}
        loadingAccounts={loadingTable}
        reloadAccounts={reloadAccounts}
      />
    </PageContainer>
  );
}

export default CustomerDetailPage;
