import { CreateAccount } from '@/core/data/accounts.type';

import {
  useCreateAccountMutation,
  useGetCustomerAccountsQuery,
  useGetCustomerByIdQuery,
} from '@/core/features/customers/api/customers-api.slice';
import DetailPresentation from '@/core/features/customers/components/DetailPresentation';
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
  }: Omit<CreateAccount, 'customerId'>) => {
    if (!customerId) return;
    await createAccountMutation({
      balance,
      customerId: customerId as string,
    }).unwrap();
  };

  return (
    <PageContainer title={customer?.name}>
      <DetailPresentation
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
