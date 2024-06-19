import { useGetTransactionByIdQuery } from '@/core/features/transactions/api/transactions-api.slice';
import TransactionDetailPresentation from '@/core/features/transactions/components/DetailPresentation';
import { Routes } from '@/routes/routes';
import { PageContainer } from '@ant-design/pro-components';
import { useNavigate, useParams } from 'react-router-dom';

function TransactionDetailPage() {
  const { transactionId } = useParams<{ transactionId: string }>();
  const { data: transaction } = useGetTransactionByIdQuery(transactionId, {
    skip: !transactionId,
  });

  const navigate = useNavigate();

  const onSeeSourceAccount = () => {
    navigate(
      `${Routes.ACCOUNTS_INDEX}/${transaction?.sourceAccount.id as string}`
    );
  };

  const onSeeDestinationAccount = () => {
    navigate(
      `${Routes.ACCOUNTS_INDEX}/${transaction?.destinationAccount.id as string}`
    );
  };

  return (
    <PageContainer title={transaction?.description}>
      <TransactionDetailPresentation
        onSeeSourceAccount={onSeeSourceAccount}
        onSeeDestinationAccount={onSeeDestinationAccount}
        data={transaction}
      />
    </PageContainer>
  );
}

export default TransactionDetailPage;
