import { useGetTransactionsQuery } from "@/core/features/transactions/api/transactions-api.slice";
import TransactionListPresentation, {
  FilterState,
} from "@/core/features/transactions/components/ListPresentation";
import { LocalPagination } from "@/core/types/local-pagination.type";

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

  const onSelectRow = (id: string) => {
    navigate(`${Routes.TRANSACTIONS_INDEX}/${id}`);
  };

  return (
    <PageContainer>
      <TransactionListPresentation
        isLoading={isLoading}
        pagination={pagination}
        refetch={refetch}
        setFilters={setFilters}
        setPagination={setPagination}
        data={data}
        onSelectRow={onSelectRow}
      />
    </PageContainer>
  );
}

export default TransactionListPage;
