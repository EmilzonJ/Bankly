import { TransactionTypes } from "@/core/enums/transaction-types.enum";
import DashboardPresentation, { DailyTransactionData, TransactionData, TransactionTypeData } from "@/core/features/stats/components/DashboardPresentation";
import { PageContainer } from "@ant-design/pro-components";

const totalCustomers = 150;
const transactions: TransactionData[] = [
  {
    reference: '66720449fb1b17d9e46d93eb',
    type: TransactionTypes.DEPOSIT,
    amount: 500,
    description: 'Monthly savings',
    createdAt: new Date().toDateString()
  },
  {
    reference: '66720449fb1b17d9e46d93ec',
    type: TransactionTypes.WITHDRAWAL,
    amount: 100,
    description: 'ATM Withdrawal',
    createdAt: new Date().toDateString()
  },
];

const transactionTypeData: TransactionTypeData[] = [
  { type: 'Depósitos', value: 100},
  { type: 'Retiros', value: 50 },
  { type: 'Transferencias', value: 25 },
];

const dailyTransactionData: DailyTransactionData[] = [
  { fecha: '2023-06-10', transacciones: 10 },
  { fecha: '2023-06-11', transacciones: 15 },
];


function DashboardPage() {
  return (
    <PageContainer title="Bienvenido">
      <DashboardPresentation
        totalCustomers={totalCustomers}
        transactions={transactions}
        transactionTypeData={transactionTypeData}
        dailyTransactionData={dailyTransactionData}
       />
    </PageContainer>
  );
}

export default DashboardPage;
