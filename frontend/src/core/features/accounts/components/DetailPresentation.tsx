import {
  TransactionTypes,
  transactionTypesColors,
} from '@/core/enums/transaction-types.enum';
import { Account } from '@/core/types/account.type';
import { Transaction } from '@/core/types/transaction.type';
import {
  ArrowDownOutlined,
  ArrowUpOutlined,
  EyeOutlined,
} from '@ant-design/icons';
import { ProDescriptions, ProTable } from '@ant-design/pro-components';
import { Badge } from 'antd';
import { accountTypesMap } from '../utils/account-types-map.util';
import { transactionTypesMap } from '../utils/transaction-types-map.util';

type DetailPresentationProps = {
  data?: Account;
  accountTransactions?: Transaction[];
  loadingTransactions: boolean;
  reloadAccounts: () => void;
  onSeeTransaction: (id: string) => void;
};

function AccountDetailPresentation({
  data,
  accountTransactions,
  loadingTransactions,
  reloadAccounts,
  onSeeTransaction,
}: DetailPresentationProps) {
  return (
    <>
      <ProDescriptions
        bordered
        dataSource={data}
        columns={[
          {
            title: 'Identificador',
            dataIndex: 'id',
            valueType: 'text',
          },
          {
            title: 'Alias',
            dataIndex: 'alias',
            valueType: 'text',
          },
          {
            title: 'Cliente',
            dataIndex: 'customerName',
            valueType: 'text',
          },
          {
            title: 'Saldo',
            dataIndex: 'balance',
            fieldProps: {
              precision: 2,
              customSymbol: 'MXN ',
            },
            valueType: 'money',
          },
          {
            title: 'Tipo',
            dataIndex: 'type',
            valueType: 'text',
            renderText: (_text, record) => accountTypesMap(record?.type),
          },
          {
            title: 'Creada en',
            dataIndex: 'createdAt',
            valueType: 'date',
          },
          {
            title: 'Actualizada en',
            dataIndex: 'updatedAt',
            valueType: 'date',
          },
        ]}
      />
      <ProTable<Partial<Transaction>>
        headerTitle='Transacciones de la cuenta'
        rowKey='id'
        loading={loadingTransactions}
        dataSource={accountTransactions}
        search={false}
        options={{
          reload: reloadAccounts,
          fullScreen: true,
        }}
        columns={[
          {
            title: 'Referencia',
            dataIndex: 'id',
            editable: false,
            width: 300,
          },
          {
            title: 'Descripción',
            dataIndex: 'description',
            hideInSearch: true,
            editable: false,
            width: 300,
          },
          {
            title: 'Monto de la transacción',
            dataIndex: 'amount',
            width: 300,
            render: (_, entity) => {
              if (entity.type === undefined) {
                return null;
              }

              const isDepositOrIncoming =
                entity.type === TransactionTypes.DEPOSIT ||
                entity.type === TransactionTypes.INCOMING_TRANSFER;

              return (
                <div style={{ display: 'flex', alignItems: 'center' }}>
                  {isDepositOrIncoming ? (
                    <ArrowUpOutlined
                      style={{ color: 'green', marginRight: 8 }}
                    />
                  ) : (
                    <ArrowDownOutlined
                      style={{ color: 'red', marginRight: 8 }}
                    />
                  )}
                  <span>{`MXN ${entity.amount}`}</span>
                </div>
              );
            },
          },
          {
            editable: false,
            title: 'Tipo',
            dataIndex: 'type',
            render: (_, entity) => {
              if (entity.type === undefined) {
                return null;
              }
              return (
                <Badge
                  color={transactionTypesColors[entity.type]}
                  text={transactionTypesMap(entity.type)}
                />
              );
            },
          },
          {
            editable: false,
            title: 'Creada en',
            dataIndex: 'createdAt',
            valueType: 'date',
          },
          {
            title: 'Detalle',
            render: (_, record) => (
              <EyeOutlined
                onClick={() => {
 
                  return record.id && onSeeTransaction(record.id);
                }}
              />
            ),
          },
        ]}
      />
    </>
  );
}

export default AccountDetailPresentation;
