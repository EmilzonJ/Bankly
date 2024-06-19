import { TransactionTypes } from "@/core/enums/transaction-types.enum";
import { Account } from "@/core/types/account.type";
import { Customer } from "@/core/types/customer.type";
import { CreateTransaction } from "@/core/types/transaction.type";
import {
  ProForm,
  ProFormDigit,
  ProFormGroup,
  ProFormSelect,
  ProFormTextArea,
} from "@ant-design/pro-components";
import { useForm, useWatch } from "antd/es/form/Form";
import { FormInstance } from "antd/lib";
import { useState } from "react";

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
  onSubmit: (values: CreateTransaction) => void;
  customerMeta: {
    handleGetCustomers: (name: string) => Promise<Customer[]>;
    loading: boolean;
  };
  handleGetAccounts: (customerId: string) => Promise<Account[]>;
};

const useOnChangeFieldParent = (form: FormInstance) => {
  const type = useWatch("type", form);
  const customerId = useWatch("customerId", form);
  const sourceAccountId = useWatch("sourceAccountId", form);
  const amount = useWatch("amount", form);
  const customerToId = useWatch("customerToId", form);

  return {
    type,
    customerId,
    sourceAccountId,
    amount,
    customerToId,
  };
};

function CreateTransactionPresentation({
  customerMeta: { handleGetCustomers, loading },
  handleGetAccounts,
  onSubmit,
}: ListPresentationProps) {
  const [form] = useForm();

  const { type, customerId, sourceAccountId, amount, customerToId } =
    useOnChangeFieldParent(form);
  const [souceCustomerSearch, setSourceCustomerSearch] = useState("");
  const [destinationCustomerSearch, setDestinationCustomerSearch] =
    useState("");
  const [selectedSourceAccount, setSelectedSourceAccount] = useState(0);

  return (
    <ProForm
      submitter={{
        searchConfig: {
          submitText: "Guardar",
        },
      }}
      form={form}
      onFinish={onSubmit}
    >
      <ProFormGroup>
        <ProFormSelect
          name="type"
          onChange={() => {
            form.resetFields([
              "customerId",
              "sourceAccountId",
              "amount",
              "customerToId",
              "destinationAccountId",
            ]);
          }}
          label="Tipo de transacción"
          valueEnum={{
            [TransactionTypes.DEPOSIT]: {
              text: "Depósito",
            },
            [TransactionTypes.WITHDRAWAL]: {
              text: "Retiro",
            },
            [TransactionTypes.OUTGOING_TRANSFER]: {
              text: "Transferencia",
            },
          }}
        />
      </ProFormGroup>
      <ProFormGroup title="Cuenta Origen">
        <ProFormSelect
          showSearch
          disabled={!type}
          placeholder="Selecciona un cliente"
          label="Cliente"
          request={async (params) => {
            const res = await handleGetCustomers(params.text);
            return res?.map((c) => ({ label: c.name, value: c.id }));
          }}
          name="customerId"
          debounceTime={500}
          params={{ text: souceCustomerSearch }}
          fieldProps={{
            allowClear: true,
            width: "100%",
            loading: loading,
            searchValue: souceCustomerSearch,
            onSearch: async (e) => {
              setSourceCustomerSearch(e);
            },
            onChange: () => {
              form.resetFields(["amount", "sourceAccountId"]);
            },
          }}
          width="md"
        />
        <ProFormSelect
          placeholder="Selecciona una cuenta"
          label="Cuenta"
          request={async (params) => {
            if (!params.text) return [];
            const res = await handleGetAccounts(params.text);
            return res?.map((c) => ({
              label: `(MXN ${c.balance}) ${c.alias}`,
              value: c.id,
              balance: c.balance,
            }));
          }}
          name="sourceAccountId"
          debounceTime={500}
          params={{ text: customerId }}
          fieldProps={{
            width: "100%",
            loading: loading,
            onChange: () => {
              form.resetFields(["amount"]);
            },
            onSelect: (_, opt) => {
              setSelectedSourceAccount(opt.balance);
            },
          }}
          width="md"
          disabled={!customerId}
          dependencies={["customerId"]}
        />
        <ProFormDigit
          name="amount"
          rules={
            +type === TransactionTypes.OUTGOING_TRANSFER ||
            +type === TransactionTypes.WITHDRAWAL
              ? [
                  {
                    type: "number",
                    max: selectedSourceAccount,
                    message: "No puede ser mayor al saldo de la cuenta",
                  },
                ]
              : undefined
          }
          label="Monto"
          disabled={!sourceAccountId}
          fieldProps={{ min: 0 }}
        />
        <ProFormTextArea
          required
          rules={[{ required: true }]}
          disabled={!sourceAccountId}
          name="description"
          label="Descripción"
        />
      </ProFormGroup>
      {+type === TransactionTypes.OUTGOING_TRANSFER && (
        <ProFormGroup title="Cuenta Destino">
          <ProFormSelect
            showSearch
            disabled={!type || !amount}
            placeholder="Selecciona un cliente"
            label="Cliente"
            request={async (params) => {
              const res = await handleGetCustomers(params.text);
              return res?.map((c) => ({ label: c.name, value: c.id }));
            }}
            name="customerToId"
            debounceTime={500}
            params={{ text: destinationCustomerSearch }}
            fieldProps={{
              allowClear: true,
              width: "100%",
              loading: loading,
              searchValue: destinationCustomerSearch,
              onSearch: async (e) => {
                setDestinationCustomerSearch(e);
              },
              onChange: () => {
                form.resetFields(["destinationAccountId"]);
              },
            }}
            width="md"
          />
          <ProFormSelect
            placeholder="Selecciona una cuenta"
            label="Cuenta"
            request={async (params) => {
              if (!params.text) return [];
              const res = await handleGetAccounts(params.text);
              return res
                ?.filter((c) => c.id !== sourceAccountId)
                .map((c) => ({
                  label: c.alias,
                  value: c.id,
                  balance: c.balance,
                }));
            }}
            name="destinationAccountId"
            debounceTime={500}
            params={{ text: customerToId }}
            fieldProps={{
              width: "100%",
              loading: loading,
            }}
            width="md"
            disabled={!customerToId}
            dependencies={["customerToId"]}
          />
        </ProFormGroup>
      )}
    </ProForm>
  );
}

export default CreateTransactionPresentation;
