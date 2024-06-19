import { TransactionTypes } from '@/core/enums/transaction-types.enum';
import { Account } from '@/core/types/account.type';
import { Customer } from '@/core/types/customer.type';
import {
  ProForm,
  ProFormDigit,
  ProFormGroup,
  ProFormSelect,
} from '@ant-design/pro-components';
import { useForm, useWatch } from 'antd/es/form/Form';
import { FormInstance } from 'antd/lib';
import { useState } from 'react';

export type FilterState = {
  type?: string;
  reference?: string;
  createdAt?: string;
};

export type LocalPagination = {
  current: number;
  pageSize: number;
};

type ListPresentationProps = {
  isLoading: boolean;
  onSubmit: () => void;
  customerMeta: {
    handleGetCustomers: (name: string) => Promise<Customer[]>;
    loading: boolean;
  };
  handleGetAccounts: (customerId: string) => Promise<Account[]>;
};

const useOnChangeFielParent = (form: FormInstance) => {
  const type = useWatch('type', form);
  const customerId = useWatch('customerId', form);
  const accountId = useWatch('accountId', form);
  const amount = useWatch('amount', form);

  const customerToId = useWatch('customerToId', form);
  const accounTotId = useWatch('accounTotId', form);

  return {
    type,
    customerId,
    accountId,
    amount,
    customerToId,
    accounTotId,
  };
};

function CreateTransactionPresentation({
  customerMeta: { handleGetCustomers, loading },
  handleGetAccounts,
}: ListPresentationProps) {
  const [form] = useForm();

  const { type, customerId, accountId, amount, customerToId, accounTotId } =
    useOnChangeFielParent(form);
  const [search, setSearch] = useState('');
  const [search2, setSearch2] = useState('');
  const [SelectedAccount1, setSelectedAccount1] = useState(0);
  // const signByType = () => {
  //   return {
  //     [TransactionTypes.DEPOSIT]: {
  //       sign: '+',
  //     },
  //     [TransactionTypes.WITHDRAWAL]: {
  //       sign: '-',
  //     },
  //   };
  // };

  return (
    <ProForm
      form={form}
      onFinish={(data) => {
        console.log(data);
      }}
    >
      <ProFormGroup>
        <ProFormSelect
          name='type'
          onChange={() => {
            form.resetFields([
              'customerId',
              'accountId',
              'amount',
              'customerToId',
              'accounTotId',
            ]);
          }}
          label='Tipo de transacción'
          valueEnum={{
            [TransactionTypes.DEPOSIT]: {
              text: 'Depósito',
            },
            [TransactionTypes.WITHDRAWAL]: {
              text: 'Retiro',
            },
            [TransactionTypes.INCOMING_TRANSFER]: {
              text: 'Transferencia',
            },
          }}
        />
      </ProFormGroup>
      <ProFormGroup title='Cuenta 1'>
        <ProFormSelect
          showSearch
          disabled={!type}
          placeholder='Selecciona un cliente'
          label='Cliente'
          request={async (params) => {
            const res = await handleGetCustomers(params.text);
            return res?.map((c) => ({ label: c.name, value: c.id }));
          }}
          name='customerId'
          debounceTime={500}
          params={{ text: search }}
          fieldProps={{
            allowClear: true,
            width: '100%',
            loading: loading,
            searchValue: search,
            onSearch: async (e) => {
              setSearch(e);
            },
            onChange: () => {
              form.resetFields(['amount', 'accountId']);
            },
          }}
          width='md'
        />
        <ProFormSelect
          placeholder='Selecciona una cuenta'
          label='Cuenta'
          request={async (params) => {
            if (!params.text) return [];
            const res = await handleGetAccounts(params.text);
            return res?.map((c) => ({
              label: `(${c.balance}) ${c.alias}`,
              value: c.id,
              balance: c.balance,
            }));
          }}
          name='accountId'
          debounceTime={500}
          params={{ text: customerId }}
          fieldProps={{
            width: '100%',
            loading: loading,
            onChange: () => {
              form.resetFields(['amount']);
            },
            onSelect: (_, opt) => {
              setSelectedAccount1(opt.balance);
            },
          }}
          width='md'
          disabled={!customerId}
          dependencies={['customerId']}
        />
        <ProFormDigit
          name='amount'
          rules={[
            {
              type: 'number',
              max: SelectedAccount1,
            },
          ]}
          label='Monto'
          disabled={!accountId}
          fieldProps={{ min: 0 }}
        />
      </ProFormGroup>
      {+type === TransactionTypes.INCOMING_TRANSFER && (
        <ProFormGroup title='Cuenta 2'>
          <ProFormSelect
            showSearch
            disabled={!type || !amount}
            placeholder='Selecciona un cliente'
            label='Cliente'
            request={async (params) => {
              const res = await handleGetCustomers(params.text);
              return res?.map((c) => ({ label: c.name, value: c.id }));
            }}
            name='customerToId'
            debounceTime={500}
            params={{ text: search }}
            fieldProps={{
              allowClear: true,
              width: '100%',
              loading: loading,
              searchValue: search2,
              onSearch: async (e) => {
                setSearch2(e);
              },
            }}
            width='md'
          />
          <ProFormSelect
            placeholder='Selecciona una cuenta'
            label='Cuenta'
            request={async (params) => {
              if (!params.text) return [];
              const res = await handleGetAccounts(params.text);
              return res?.map((c) => ({
                label: c.alias,
                value: c.id,
                balance: c.balance,
              }));
            }}
            name='accounTotId'
            debounceTime={500}
            params={{ text: customerToId }}
            fieldProps={{
              width: '100%',
              loading: loading,
              onSelect: (_v, opt) => {
                console.log(opt);
              },
            }}
            width='md'
            disabled={!customerToId}
            dependencies={['customerToId']}
          />
          <ProFormDigit
            name='amountTo'
            label='Monto'
            disabled={!accounTotId}
            fieldProps={{ min: 0 }}
          />
        </ProFormGroup>
      )}
    </ProForm>
  );
}

export default CreateTransactionPresentation;
