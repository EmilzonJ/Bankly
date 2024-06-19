import { Account, CreateAccount } from "@/core/types/account.type";
import { Customer } from "@/core/types/customer.type";
import { Routes } from "@/routes/routes";
import { EllipsisOutlined, EyeOutlined } from "@ant-design/icons";
import { EditableProTable, ProDescriptions } from "@ant-design/pro-components";
import { Dropdown } from "antd";
import { useForm } from "antd/es/form/Form";
import { ItemType } from "antd/es/menu/interface";
import { useNavigate } from "react-router-dom";
import { accountTypesMap } from "../../accounts/utils/account-types-map.util";

type DetailPresentationProps = {
  data?: Customer;
  customerAccounts?: Account[];
  loadingAccounts: boolean;
  reloadAccounts: () => void;
  onCreateAccount: (data: Omit<CreateAccount, "customerId">) => Promise<void>;
};

function CustomerDetailPresentation({
  data,
  customerAccounts,
  loadingAccounts,
  reloadAccounts,
  onCreateAccount,
}: DetailPresentationProps) {
  const [form] = useForm();
  const navigate = useNavigate();

  const onSelectAccount = (
    key: string,
    account: Account
  ) => {
    const actions: Record<string, () => void> = {
      '1': () => {
        navigate(`${Routes.ACCOUNTS_INDEX}/${account.id}`);
      },
    };

    actions[key]?.();
  };


  return (
    <>
      <ProDescriptions
        bordered
        dataSource={data}
        columns={[
          {
            title: "Nombre",
            dataIndex: "name",
          },
          {
            title: "Email",
            dataIndex: "email",
          },
          {
            title: "Fecha de Registro",
            dataIndex: "registeredAt",
            valueType: "date",
          },
        ]}
      />
      <EditableProTable<Partial<Account>>
        headerTitle="Cuentas del cliente"
        rowKey="id"
        recordCreatorProps={{
          position: "bottom",
          record: () => ({
            id: "Nueva",
            balance: 0,
          }),
          creatorButtonText: "Crear nueva cuenta",
        }}
        editable={{
          form,
          type: "multiple",
          onSave: async (rowKey, data) => {
            if (rowKey === "Nueva") {
              await onCreateAccount({
                balance: data.balance as number,
                alias: data.alias as string,
              });
              form.resetFields();
              return true;
            }
          },
          actionRender: (_row, _config, defaultDoms) => [
            defaultDoms.save,
            defaultDoms.cancel,
          ],
        }}
        loading={loadingAccounts}
        value={customerAccounts}
        search={false}
        options={{
          reload: reloadAccounts,
          fullScreen: true,
        }}
        columns={[
          {
            title: "Identificador",
            dataIndex: "id",
            hideInSearch: true,
            editable: false,
            width: 300,
          },
          {
            title: "Alias",
            dataIndex: "alias",
            width: 300,
          },
          {
            title: "Balance",
            dataIndex: "balance",
            valueType: "money",
            fieldProps: {
              min: 0,
              precision: 2,
              customSymbol: "MXN ",
            }
          },
          {
            editable: false,
            title: "Tipo",
            dataIndex: "type",
            renderText: (_text, record) => accountTypesMap(record?.type),
          },
          {
            title: 'Acciones',
            dataIndex: 'option',
            valueType: 'option',
            align: 'center',
            width: 200,
            render: (_, entity) => (
              <Dropdown
                menu={{
                  items,
                  onClick: (e) =>
                    onSelectAccount(
                      e.key,
                      entity as Account
                    ),
                }}
              >
                <EllipsisOutlined
                  style={{
                    cursor: 'pointer',
                  }}
                />
              </Dropdown>
            ),
          },
        ]}
      />
    </>
  );
}

const items: ItemType[] = [
  {
    key: '1',
    label: 'Ver detalle',
    icon: <EyeOutlined />,
  },
];

export default CustomerDetailPresentation;
