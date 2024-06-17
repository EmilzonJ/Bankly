import { useGetCustomersQuery } from '@/core/features/customers/api/customers-api.slice';
import { EditableProTable, PageContainer } from '@ant-design/pro-components';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

interface FilterState {
  name?: string;
  email?: string;
  registeredAt?: string;
}

const CustomerListPage = () => {
  const [pagination, setPagination] = useState<{
    current: number;
    pageSize: number;
  }>({
    current: 1,
    pageSize: 10,
  });
  const [filters, setFilters] = useState<FilterState>({});

  const { data, isLoading, refetch } = useGetCustomersQuery({
    pageNumber: pagination.current,
    pageSize: pagination.pageSize,
    ...filters,
  });

  const navigate = useNavigate();

  return (
    <PageContainer>
      <EditableProTable
        loading={isLoading}
        onReset={() => {
          setFilters({});
        }}
        options={{
          reload: () => {
            refetch();
          },
          fullScreen: true,
          setting: true,
          density: true,
        }}
        pagination={{
          current: pagination.current,
          pageSize: pagination.pageSize,
          total: data?.totalCount || 0,
          onChange: (page, pageSize) => {
            setPagination({
              current: page,
              pageSize: pageSize,
            });
          },
        }}
        cardBordered
        rowKey='id'
        search={{
          labelWidth: 'auto',
        }}
        onSubmit={(values) => {
          setFilters({
            name: values.name ?? '',
            email: values.email ?? '',
            registeredAt: values.registeredAt ?? '',
          });
        }}
        onRow={(customer) => ({
          onClick: () => {
            navigate(`/customers/detail/${customer.id}`);
          },
        })}
        value={data?.items || []}
        columns={[
          {
            title: 'Correo',
            dataIndex: 'email',
            filters: true,
            valueType: 'text',
          },
          {
            title: 'Nombre',
            dataIndex: 'name',
            filters: true,
            valueType: 'text',
          },
          {
            title: 'Fecha de registro',
            dataIndex: 'registeredAt',
            valueType: 'date',
            filters: true,
          },
        ]}
      />
    </PageContainer>
  );
};

export default CustomerListPage;
