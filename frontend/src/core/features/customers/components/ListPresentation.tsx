import {
  CreateCustomer,
  Customer,
  UpdateCustomer,
} from "@/core/types/customer.type";
import { Paginated } from "@/core/types/paginated.type";
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  EyeOutlined,
} from "@ant-design/icons";
import {
  EditableProTable,
  ProCoreActionType,
} from "@ant-design/pro-components";
import { Dropdown } from "antd";
import { useForm } from "antd/es/form/Form";
import { ItemType } from "antd/es/menu/interface";

export type FilterState = {
  name?: string;
  email?: string;
  registeredAt?: string;
};

export type LocalPagination = {
  current: number;
  pageSize: number;
};

type ListPresentationProps = {
  handleCreate: (data: CreateCustomer) => Promise<void>;
  handleUpdate: (data: UpdateCustomer) => Promise<void>;
  isLoading: boolean;
  setFilters: (data: FilterState) => void;
  setPagination: (data: LocalPagination) => void;
  pagination: LocalPagination;
  data?: Paginated<Customer>;
  onSelectOptions: (
    key: string,
    entity: Customer,
    action: ProCoreActionType<object>
  ) => void;
  refetch: () => void;
};

function CustomerListPresentation({
  handleCreate,
  handleUpdate,
  isLoading,
  setFilters,
  setPagination,
  pagination,
  onSelectOptions,
  data,
  refetch,
}: ListPresentationProps) {
  const [form] = useForm();

  return (
    <EditableProTable<Partial<Customer>>
      scroll={{ y: "calc(100vh - 450px)", x: "max-content" }}
      recordCreatorProps={{
        position: "top",
        record: () => ({
          id: "create",
          name: "",
          email: "",
        }),
        creatorButtonText: "Crear nuevo cliente",
      }}
      editable={{
        form,
        type: "multiple",
        onSave: async (rowKey, data) => {
          if (rowKey === "create") {
            await handleCreate({
              email: data.email as string,
              name: data.name as string,
            });
            form.resetFields();
          } else {
            await handleUpdate({
              email: data.email as string,
              id: data.id as string,
              name: data.name as string,
            });
          }
        },

        actionRender: (_row, _config, defaultDoms) => [
          defaultDoms.save,
          defaultDoms.cancel,
        ],
      }}
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
          name: values.name ?? "",
          email: values.email ?? "",
          registeredAt: values.registeredAt ?? "",
        });

        setPagination({
          current: pagination.current,
          pageSize: pagination.pageSize,
        });
      }}
      value={data?.items || []}
      columns={[
        {
          title: "Nombre",
          dataIndex: "name",
          filters: true,
          copyable: true,
          valueType: "text",
          fieldProps: {
            placeholder: "Ingrese un nombre",
          },
          formItemProps: {
            label: "Nombre",
            rules: [{ type: "string" }],
          },
        },
        {
          title: "Correo",
          dataIndex: "email",
          filters: true,
          valueType: "text",
          fieldProps: {
            placeholder: "Ingrese un email",
          },
          formItemProps: {
            label: "Correo",
            rules: [{ type: "email", message: "El correo no es válido" }],
          },
        },
        {
          title: "Fecha de registro",
          dataIndex: "registeredAt",
          valueType: "date",
          filters: true,
          editable: false,
        },
        {
          title: "Acciones",
          dataIndex: "option",
          valueType: "option",
          align: "center",
          width: 200,
          render: (_, entity, _2, action) => (
            <Dropdown
              menu={{
                items,
                onClick: (e) =>
                  onSelectOptions(
                    e.key,
                    entity as Customer,
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
    key: "2",
    label: "Editar",
    icon: <EditOutlined />,
  },
  {
    key: "3",
    label: "Ver detalle",
    icon: <EyeOutlined />,
  },
];

export default CustomerListPresentation;
