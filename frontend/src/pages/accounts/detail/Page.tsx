import {
  useGetAccountTransactionsQuery,
  useGetAccountByIdQuery,
} from '@/core/features/accounts/api/accounts-api.slice';
import DetailPresentation from '@/core/features/accounts/components/DetailPresentation';
import { Routes } from '@/routes/routes';
import { PageContainer } from '@ant-design/pro-components';
import { useNavigate, useParams } from 'react-router-dom';

function AccountDetailPage() {
  const navigate = useNavigate();
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

  const onSeeTransaction = (id: string) => {
    navigate(`${Routes.TRANSACTIONS_INDEX}/${id}`);
  };

  return (
    <PageContainer title='Detalles de la cuenta'>
      <DetailPresentation
        onSeeTransaction={onSeeTransaction}
        data={account}
        accountTransactions={transactions}
        loadingTransactions={loadingTable}
        reloadAccounts={reloadAccounts}
      />
    </PageContainer>
  );
}

export default AccountDetailPage;
