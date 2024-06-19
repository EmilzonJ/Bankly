import { Account, UpdateAccount } from "@/core/types/account.type";
import { LocalPagination } from "@/core/types/local-pagination.type";
import { Paginated } from "@/core/types/paginated.type";
import {
  DeleteOutlined,
  EllipsisOutlined,
  EyeOutlined,
} from "@ant-design/icons";
import {
  ProCoreActionType,
  ProTable,
} from "@ant-design/pro-components";
import { Dropdown } from "antd";
import { ItemType } from "antd/es/menu/interface";
import { accountTypesMap } from "../utils/account-types-map.util";

export type FilterState = {
  identifier?: string;
  alias?: string;
  customerName?: string;
  createdAt?: string;
};

type ListPresentationProps = {
  handleUpdate: (data: UpdateAccount) => Promise<void>;
  isLoading: boolean;
  setFilters: (data: FilterState) => void;
  setPagination: (data: LocalPagination) => void;
  pagination: LocalPagination;
  data?: Paginated<Account>;
  onSelectOptions: (
    key: string,
    entity: Account,
    action: ProCoreActionType<object>
  ) => void;
  refetch: () => void;
};

function AccountListPresentation({
  isLoading,
  setFilters,
  setPagination,
  pagination,
  onSelectOptions,
  data,
  refetch,
}: ListPresentationProps) {
  return (
    <ProTable<Partial<Account>>
      scroll={{ y: 'calc(100vh - 450px)', x: 'max-content'}}
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
          identifier: values.id ?? "",
          alias: values.alias ?? "",
          createdAt: values.createdAt ?? "",
          customerName: values.customerName ?? ""
        });

        setPagination({
          current: pagination.current,
          pageSize: pagination.pageSize,
        });
      }}
      dataSource={data?.items || []}
      columns={[
        {
          title: "Identificador",
          dataIndex: "id",
          valueType: "text",
          filters: true,
        },
        {
          title: "Cliente",
          dataIndex: "customerName",
          valueType: "text",
          fieldProps: {
            placeholder: "Nombre del cliente"
          },
          filters: true,
        },
        {
          title: "Alias",
          dataIndex: "alias",
          filters: true,
          valueType: "text",
        },
        {
          title: "Saldo",
          dataIndex: "balance",
          hideInSearch: true,
          fieldProps: {
            precision: 2,
            customSymbol: "MXN ",
          },
          valueType: 'money'
        },
        {
          title: "Tipo",
          dataIndex: "type",
          valueType: "text",
          filters: true,
          hideInSearch: true,
          renderText: (_text, record) => accountTypesMap(record?.type),
        },
        {
          title: "Creada en",
          dataIndex: "createdAt",
          valueType: "date",
          filters: true,
        },
        {
          title: "Actualizada en",
          dataIndex: "updatedAt",
          valueType: "date",
          width: 200,
          hideInSearch: true,
        },
        {
          title: "Acciones",
          dataIndex: "option",
          valueType: "option",
          width: 150,
          render: (_, entity, _2, action) => (
            <Dropdown
              menu={{
                items,
                onClick: (e) =>
                  onSelectOptions(
                    e.key,
                    entity as Account,
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
    key: "1",
    label: "Eliminar",
    icon: <DeleteOutlined />,
  },
  {
    key: "3",
    label: "Ver detalle",
    icon: <EyeOutlined />,
  },
];

export default AccountListPresentation;
