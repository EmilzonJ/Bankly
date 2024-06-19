import {
  useDeleteAccountMutation,
  useGetAccountsQuery,
  useUpdateAccountMutation,
} from "@/core/features/accounts/api/accounts-api.slice";
import AccountListPresentation, {
  FilterState,
} from "@/core/features/accounts/components/ListPresentation";
import { Account, UpdateAccount } from "@/core/types/account.type";
import { LocalPagination } from "@/core/types/local-pagination.type";

import { Routes } from "@/routes/routes";
import { PageContainer, ProCoreActionType } from "@ant-design/pro-components";
import { App } from "antd";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

function AccountListPage() {
  const [pagination, setPagination] = useState<LocalPagination>({
    current: 1,
    pageSize: 10,
  });

  const { notification } = App.useApp();

  const [filters, setFilters] = useState<FilterState>({});

  const { data, isLoading, refetch } = useGetAccountsQuery({
    pageNumber: pagination.current,
    pageSize: pagination.pageSize,
    ...filters,
  });

  const navigate = useNavigate();

  const [updateMutation] = useUpdateAccountMutation();
  const [deleteMutation] = useDeleteAccountMutation();

  const handleUpdate = async (data: UpdateAccount) => {
    await updateMutation(data).unwrap();
    notification.success({ message: "Cuenta actualizado correctamente" });
  };

  const handleDelete = async (id: string) => {
    await deleteMutation(id).unwrap();
    notification.success({ message: "Cuenta eliminada correctamente" });
  };

  const onSelect = (
    key: string,
    account: Account,
    action: ProCoreActionType<object>
  ) => {
    const actions: Record<string, () => void> = {
      "1": () => {
        handleDelete(account.id);
      },
      "2": () => {
        action.startEditable?.(account.id);
      },
      "3": () => {
        navigate(`${Routes.ACCOUNTS_INDEX}/${account.id}`);
      },
    };

    actions[key]?.();
  };

  return (
    <PageContainer>
      <AccountListPresentation
        handleUpdate={handleUpdate}
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

export default AccountListPage;
