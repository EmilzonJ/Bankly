import {
  useGetAccountTransactionsQuery,
  useGetAccountByIdQuery,
} from '@/core/features/accounts/api/accounts-api.slice';
import DetailPresentation from '@/core/features/accounts/components/DetailPresentation';
import { PageContainer } from '@ant-design/pro-components';
import { useParams } from 'react-router-dom';

function AccountDetailPage() {
  const { accountId } = useParams<{ accountId: string }>();
  const { data: account } = useGetAccountByIdQuery(accountId, {
    skip: !accountId,
  });

  const {
    data: transactions,
    isLoading: loadingTable,
    refetch: reloadAccounts,
  } = useGetAccountTransactionsQuery(accountId, {
    skip: !accountId,
  });

  return (
    <PageContainer title="Detalles de la cuenta">
      <DetailPresentation
        data={account}
        accountTransactions={transactions}
        loadingTransactions={loadingTable}
        reloadAccounts={reloadAccounts}
      />
    </PageContainer>
  );
}

export default AccountDetailPage;
