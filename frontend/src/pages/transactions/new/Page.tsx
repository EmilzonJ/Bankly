import {
  useLazyGetCustomerAccountsQuery,
  useLazyGetCustomersQuery,
} from "@/core/features/customers/api/customers-api.slice";
import { useCreateTransactionMutation } from "@/core/features/transactions/api/transactions-api.slice";
import CreateTransactionPresentation from "@/core/features/transactions/components/CreatePresentation";
import { CreateTransaction } from "@/core/types/transaction.type";
import { Routes } from "@/routes/routes";
import { PageContainer } from "@ant-design/pro-components";
import { App } from "antd";
import { useNavigate } from "react-router-dom";

function TransactionNewPage() {
  const [getCustomerAccounts] = useLazyGetCustomerAccountsQuery();
  const [getCustomers, { isLoading, isFetching }] = useLazyGetCustomersQuery();
  const [createTransactionMutation] = useCreateTransactionMutation();

  const { notification } = App.useApp();
  const navigate = useNavigate();

  const handleGetCustomer = async (name: string) => {
    const data = await getCustomers({
      name,
      pageNumber: 1,
      pageSize: 20,
    }).unwrap();
    return data.items;
  };

  const onSumbit = async (values: CreateTransaction) => {
    const response = await createTransactionMutation({
      ...values,
      amount: Number(values.amount),
      type: Number(values.type),
    }).unwrap();

    notification.success({
      message: "Transacción creada con éxito",
    });

    navigate(`${Routes.TRANSACTIONS_INDEX}/${response}`);
  };

  const handleGetAccounts = async (customerId: string) => {
    const data = await getCustomerAccounts(customerId).unwrap();
    return data;
  };

  return (
    <PageContainer>
      <CreateTransactionPresentation
        isLoading={isLoading}
        onSubmit={onSumbit}
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
