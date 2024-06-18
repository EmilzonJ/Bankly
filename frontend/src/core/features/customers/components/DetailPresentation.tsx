import { Account, CreateAccount } from '@/core/data/accounts.type';
import { Customer } from '@/core/data/customer.type';
import { EditableProTable, ProDescriptions } from '@ant-design/pro-components';
import { useForm } from 'antd/es/form/Form';
import { accountTpeMap } from '../../accounts/api/maps/accountTypes.map';

type DetailPresentationProps = {
  data?: Customer;
  customerAccounts?: Account[];
  loadingAccounts: boolean;
  reloadAccounts: () => void;
  onCreateAccount: (data: Omit<CreateAccount, 'customerId'>) => Promise<void>;
};

function DetailPresentation({
  data,
  customerAccounts,
  loadingAccounts,
  reloadAccounts,
  onCreateAccount,
}: DetailPresentationProps) {
  const [form] = useForm();
  return (
    <>
      <ProDescriptions
        dataSource={data}
        columns={[
          {
            title: 'Nombre',
            dataIndex: 'name',
          },
          {
            title: 'Email',
            dataIndex: 'email',
          },
          {
            title: 'Fecha de Registro',
            dataIndex: 'registeredAt',
            valueType: 'date',
          },
        ]}
      />
      <EditableProTable<Partial<Account>>
        headerTitle='Cuentas del cliente'
        rowKey='id'
        recordCreatorProps={{
          position: 'bottom',
          record: () => ({
            id: 'Nueva',
            balance: 0,
          }),
          creatorButtonText: 'Crear nueva cuenta',
        }}
        editable={{
          form,
          type: 'multiple',
          onSave: async (rowKey, data) => {
            if (rowKey === 'Nueva') {
              await onCreateAccount({
                balance: data.balance as number,
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
            title: 'Identificador',
            dataIndex: 'id',
            hideInSearch: true,
            editable: false,
            width: 300,
          },
          {
            title: 'Alias',
            dataIndex: 'alias',
            width: 300,
          },
          {
            title: 'Balance',
            dataIndex: 'balance',
          },
          {
            editable: false,
            title: 'Tipo',
            dataIndex: 'type',
            renderText: (_text, record) => accountTpeMap(record?.type),
          },
          {
            valueType: 'option',
          },
        ]}
      />
    </>
  );
}

export default DetailPresentation;
