import { Paginated } from "@/core/types/paginated.type";
import { Transaction } from "@/core/types/transaction.type";
import {
  DeleteOutlined,
  EllipsisOutlined,
  EyeOutlined,
} from "@ant-design/icons";
import {
  EditableProTable,
  ProCoreActionType,
} from "@ant-design/pro-components";
import { Dropdown } from "antd";
import { ItemType } from "antd/es/menu/interface";

export type FilterState = {
  type?: string;
  sourceAccountId?: string;
  destinationAccountId?: string;
  reference?: string;
  customerName?: string;
};

export type LocalPagination = {
  current: number;
  pageSize: number;
};

type ListPresentationProps = {
  isLoading: boolean;
  setFilters: (data: FilterState) => void;
  setPagination: (data: LocalPagination) => void;
  pagination: LocalPagination;
  data?: Paginated<Transaction>;
  onSelectOptions: (
    key: string,
    entity: Transaction,
    action: ProCoreActionType<object>
  ) => void;
  refetch: () => void;
};

function TransactionListPresentation({
  isLoading,
  setFilters,
  setPagination,
  pagination,
  onSelectOptions,
  data,
  refetch,
}: ListPresentationProps) {

  return (
    <EditableProTable<Partial<Transaction>>
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
      rowKey="id"
      search={{
        labelWidth: "auto",
      }}
      onSubmit={(values) => {
        setFilters({
          sourceAccountId: values.sourceAccountId ?? "",
              type: values.type ?? "",
              destinationAccountId: values.destinationAccountId ?? "",
              reference: values.reference ?? "",
              customerName: values.customerName ?? "",
        });
      }}
      value={data?.items || []}
      columns={[
        {
          title: "Correo",
          dataIndex: "email",
          filters: true,
          valueType: "text",
        },
        {
          title: "Nombre",
          dataIndex: "name",
          filters: true,
          valueType: "text",
        },
        {
          title: "Fecha de registro",
          dataIndex: "registeredAt",
          valueType: "date",
          filters: true,
        },
        {
          title: "Acciones",
          dataIndex: "option",
          valueType: "option",
          width: 200,
          render: (_, entity, _2, action) => (
            <Dropdown
              menu={{
                items,
                onClick: (e) =>
                  onSelectOptions(
                    e.key,
                    entity as Transaction,
                    action as ProCoreActionType<object>
                  ),
              }}
            >
              <EllipsisOutlined
                style={{
                  cursor: "pointer",
                }}
              />
            </Dropdown>
          ),
        },
      ]}
    />
  );
}

const items: ItemType[] = [
  {
    key: '1',
    label: 'Eliminar',
    icon: <DeleteOutlined />,
  },
  {
    key: '3',
    label: 'Ver detalle',
    icon: <EyeOutlined />,
  },
];

export default TransactionListPresentation;
