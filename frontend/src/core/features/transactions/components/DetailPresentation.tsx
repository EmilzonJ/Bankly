import {
  TransactionTypes,
  transactionTypesColors,
} from "@/core/enums/transaction-types.enum";
import { TransacttionById } from "@/core/types/transaction.type";
import {
  ProForm,
  ProFormDatePicker,
  ProFormField,
  ProFormGroup,
  ProFormMoney,
  ProFormText,
  ProFormTextArea,
} from "@ant-design/pro-components";
import { Badge, Button, Divider } from "antd";
import { useForm } from "antd/es/form/Form";
import { useCallback, useEffect } from "react";
import { transactionTypesMap } from "../../accounts/utils/transaction-types-map.util";

type DetailPresentationProps = {
  data?: TransacttionById;
  onSeeSourceAccount: () => void;
  onSeeDestinationAccount?: () => void;
};

function TransactionDetailPresentation({
  data,
  onSeeSourceAccount,
  onSeeDestinationAccount,
}: DetailPresentationProps) {
  const [form] = useForm();

  useEffect(() => {
    form.setFieldsValue({
      ...data,
      type: (
        <Badge
          color={
            data?.type != undefined ? transactionTypesColors[data?.type] : ""
          }
          text={transactionTypesMap(data?.type)}
        />
      ),
    });
  }, [data, form]);

  const getSourceAccountTitle = useCallback(() => {
    if (data?.type === TransactionTypes.DEPOSIT || data?.type === TransactionTypes.INCOMING_TRANSFER) {
      return "Depósito en cuenta:";
    }

    if (data?.type === TransactionTypes.WITHDRAWAL || data?.type === TransactionTypes.OUTGOING_TRANSFER) {
      return "Retiro en cuenta:";
    }

  }, [data]);

  const getDestinationAccountTitle = useCallback(() => {
    if (data?.type === TransactionTypes.INCOMING_TRANSFER) {
      return "Retiro en cuenta:";
    }

    if (data?.type === TransactionTypes.OUTGOING_TRANSFER) {
      return "Depósito en cuenta:";
    }
  }, [data]);

  return (
    <>
      <ProForm
        readonly
        loading={!data}
        initialValues={data}
        syncToInitialValues
        submitter={false}
        form={form}
      >
        <ProFormGroup title="Transacción">
          <ProFormField label="Referencia" name="reference" />
          <ProFormField
            label="Tipo"
            name="type"
            renderText={(r) => transactionTypesMap(r.type)}
          />
          <ProFormTextArea label="Descripción" name="description" />
          <ProFormMoney
            label="Monto"
            name="amount"
            fieldProps={{ precision: 2, customSymbol: "MXN " }}
          />
          <ProFormDatePicker label="Creada en" name="createdAt" />
        </ProFormGroup>
        <Divider />
        <ProFormGroup
          title={getSourceAccountTitle()}
          extra={
            <Button type="default" onClick={onSeeSourceAccount}>
              Ver cuenta
            </Button>
          }
        >
          <ProFormText label="Identificador" name={["sourceAccount", "id"]} />

          <ProFormField label="Alias" name={["sourceAccount", "alias"]} />
          <ProFormField
            label="Cliente"
            name={["sourceAccount", "customer", "name"]}
          />
          <ProFormField
            label="Email"
            name={["sourceAccount", "customer", "email"]}
          />
        </ProFormGroup>
        <Divider />
        {data?.destinationAccount && (
          <ProFormGroup
            title={getDestinationAccountTitle()}
            extra={
              <Button type="default" onClick={onSeeDestinationAccount}>
                Ver cuenta
              </Button>
            }
          >
            <ProFormText
              label="Identificador"
              name={["destinationAccount", "id"]}
            />

            <ProFormField
              label="Alias"
              name={["destinationAccount", "alias"]}
            />
            <ProFormField
              label="Cliente"
              name={["destinationAccount", "customer", "name"]}
            />
            <ProFormField
              label="Email"
              name={["destinationAccount", "customer", "email"]}
            />
          </ProFormGroup>
        )}
      </ProForm>
    </>
  );
}

export default TransactionDetailPresentation;
