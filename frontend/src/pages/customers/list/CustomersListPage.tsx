import {
  CreateCustomer,
  Customer,
  UpdateCustomer,
} from '@/core/data/customer.type';
import {
  useCreateCustomerMutation,
  useDeleteCustomerMutation,
  useGetCustomersQuery,
  useUpdateCustomerMutation,
} from '@/core/features/customers/api/customers-api.slice';
import ListPresentation, {
  FilterState,
  LocalPagination,
} from '@/core/features/customers/components/ListPresentation';
import { PageContainer, ProCoreActionType } from '@ant-design/pro-components';
import { App } from 'antd';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const CustomerListPage = () => {
  const [pagination, setPagination] = useState<LocalPagination>({
    current: 1,
    pageSize: 10,
  });

  const { notification } = App.useApp();

  const [filters, setFilters] = useState<FilterState>({});

  const { data, isLoading, refetch } = useGetCustomersQuery({
    pageNumber: pagination.current,
    pageSize: pagination.pageSize,
    ...filters,
  });

  const navigate = useNavigate();

  const [createMutation] = useCreateCustomerMutation();
  const [updateMutation] = useUpdateCustomerMutation();
  const [deleteMutation] = useDeleteCustomerMutation();

  const handleCreate = async (data: CreateCustomer) => {
    await createMutation(data).unwrap();
    notification.success({ message: 'Cliente creado correctamente' });
  };

  const handleUpdate = async (data: UpdateCustomer) => {
    await updateMutation(data).unwrap();
    notification.success({ message: 'Cliente actualizado correctamente' });
  };

  const handleDelete = async (id: string) => {
    await deleteMutation(id).unwrap();
    notification.success({ message: 'Cliente eliminado correctamente' });
  };

  const onSelect = (
    key: string,
    customer: Customer,
    action: ProCoreActionType<object>
  ) => {
    const actions: Record<string, () => void> = {
      '1': () => {
        handleDelete(customer.id);
      },
      '2': () => {
        action.startEditable?.(customer.id);
      },
      '3': () => {
        navigate(`/customers/detail/${customer.id}`);
      },
    };

    actions[key]?.();
  };

  return (
    <PageContainer>
      <ListPresentation
        handleCreate={handleCreate}
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
};

export default CustomerListPage;
