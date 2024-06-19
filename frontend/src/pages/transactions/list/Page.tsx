import {
    useGetTransactionsQuery
} from "@/core/features/transactions/api/transactions-api.slice";
import TransactionListPresentation, {
    FilterState,
} from "@/core/features/transactions/components/ListPresentation";
import { LocalPagination } from "@/core/types/local-pagination.type";
import { Transaction } from "@/core/types/transaction.type";

import { Routes } from "@/routes/routes";
import { PageContainer } from "@ant-design/pro-components";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

function TransactionListPage() {
  const [pagination, setPagination] = useState<LocalPagination>({
    current: 1,
    pageSize: 10,
  });

  const [filters, setFilters] = useState<FilterState>({});

  const { data, isLoading, refetch } = useGetTransactionsQuery({
    pageNumber: pagination.current,
    pageSize: pagination.pageSize,
    ...filters,
  });

  const navigate = useNavigate();

  const onSelect = (key: string, transaction: Transaction) => {
    const actions: Record<string, () => void> = {
      "1": () => {
        navigate(`${Routes.TRANSACTIONS_INDEX}/${transaction.id}`);
      },
    };

    actions[key]?.();
  };

  return (
    <PageContainer>
      <TransactionListPresentation
        isLoading={isLoading}
        onSelectOptions={onSelect}
        pagination={pagination}
        refetch={refetch}
        setFilters={setFilters}
        setPagination={setPagination}
        data={data}
      />
    </PageContainer>
  );
}

export default TransactionListPage;
