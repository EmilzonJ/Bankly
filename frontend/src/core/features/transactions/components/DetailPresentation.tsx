import { TransacttionById } from '@/core/types/transaction.type';
import {
  ProForm,
  ProFormDatePicker,
  ProFormField,
  ProFormGroup,
  ProFormMoney,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { transactionTypesMap } from '../../accounts/utils/transaction-types-map.util';
import { useForm } from 'antd/es/form/Form';
import { useEffect } from 'react';
import { Badge, Button, Divider } from 'antd';
import { transactionTypesColors } from '@/core/enums/transaction-types.enum';

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
            data?.type != undefined ? transactionTypesColors[data?.type] : ''
          }
          text={transactionTypesMap(data?.type)}
        />
      ),
    });
  }, [data, form]);

  return (
    <>
      <ProForm
        readonly
        initialValues={data}
        syncToInitialValues
        submitter={false}
        form={form}
      >
        <ProFormGroup title='Transacción'>
          <ProFormField label='Identificador' name='reference' />
          <ProFormField
            label='Tipo'
            name='type'
            renderText={(r) => transactionTypesMap(r.type)}
          />
          <ProFormTextArea label='Descripción' name='description' />
          <ProFormMoney
            label='Monto'
            name='amount'
            fieldProps={{ precision: 2, customSymbol: 'MXN ' }}
          />
          <ProFormDatePicker label='Creada en' name='createdAt' />
        </ProFormGroup>
        <Divider />
        <ProFormGroup
          title='Cuenta Origen'
          extra={
            <Button type='primary' onClick={onSeeSourceAccount}>
              Ver cuenta origen
            </Button>
          }
        >
          <ProFormText label='Identificador' name={['sourceAccount', 'id']} />

          <ProFormField label='Alias' name={['sourceAccount', 'alias']} />
          <ProFormField
            label='Cliente'
            name={['sourceAccount', 'customer', 'name']}
          />
          <ProFormField
            label='Email'
            name={['sourceAccount', 'customer', 'email']}
          />
        </ProFormGroup>
        {data?.destinationAccount && (
          <ProFormGroup
            title='Cuenta Destino'
            extra={
              <Button type='primary' onClick={onSeeDestinationAccount}>
                Ver cuenta destino
              </Button>
            }
          >
            <ProFormText
              label='Identificador'
              name={['destinationAccount', 'id']}
            />

            <ProFormField
              label='Alias'
              name={['destinationAccount', 'alias']}
            />
            <ProFormField
              label='Cliente'
              name={['destinationAccount', 'customer', 'name']}
            />
            <ProFormField
              label='Email'
              name={['destinationAccount', 'customer', 'email']}
            />
          </ProFormGroup>
        )}
      </ProForm>
    </>
  );
}

export default TransactionDetailPresentation;
