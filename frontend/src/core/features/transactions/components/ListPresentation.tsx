import {
  TransactionTypes,
  transactionTypesColors,
} from "@/core/enums/transaction-types.enum";
import { Paginated } from "@/core/types/paginated.type";
import { Transaction } from "@/core/types/transaction.type";
import {
  ArrowDownOutlined,
  ArrowUpOutlined,
  EllipsisOutlined
} from "@ant-design/icons";
import {
  EditableProTable
} from "@ant-design/pro-components";
import { Badge } from "antd";
import { transactionTypesMap } from "../../accounts/utils/transaction-types-map.util";

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
  setFilters: (data: FilterState) => void;
  setPagination: (data: LocalPagination) => void;
  pagination: LocalPagination;
  data?: Paginated<Transaction>;
  onSelectRow: (id: string) => void;
  refetch: () => void;
};

function TransactionListPresentation({
  isLoading,
  setFilters,
  setPagination,
  pagination,
  data,
  onSelectRow,
  refetch
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
          type: values.type ?? "",
          reference: values.reference ?? "",
          createdAt: values.createdAt ?? "",
        });
      }}
      value={data?.items || []}
      columns={[
        {
          title: "Referencia",
          dataIndex: "id",
          editable: false,
          width: 300,
        },
        {
          title: "Descripción",
          dataIndex: "description",
          hideInSearch: true,
          editable: false,
          width: 300,
        },
        {
          title: "Monto de la transacción",
          dataIndex: "amount",
          hideInSearch: true,
          width: 300,
          render: (_, entity) => {
            if (entity.type === undefined) {
              return null;
            }

            const isDepositOrIncoming =
              entity.type === TransactionTypes.DEPOSIT ||
              entity.type === TransactionTypes.INCOMING_TRANSFER;

            return (
              <div style={{ display: "flex", alignItems: "center" }}>
                {isDepositOrIncoming ? (
                  <ArrowUpOutlined style={{ color: "green", marginRight: 8 }} />
                ) : (
                  <ArrowDownOutlined style={{ color: "red", marginRight: 8 }} />
                )}
                <span>{`MXN ${entity.amount}`}</span>
              </div>
            );
          },
        },
        {
          editable: false,
          title: "Tipo",
          valueType: "select",
          fieldProps: {
            options: [
              {
                label: "Depósito",
                value: TransactionTypes.DEPOSIT,
              },
              {
                label: "Retiro",
                value: TransactionTypes.WITHDRAWAL,
              },
              {
                label: "Transferencia saliente",
                value: TransactionTypes.OUTGOING_TRANSFER,
              },
              {
                label: "Transferencia entrante",
                value: TransactionTypes.INCOMING_TRANSFER,
              },
            ],
          },
          dataIndex: "type",
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
          title: "Creada en",
          dataIndex: "createdAt",
          valueType: "date",
        },
        {
          title: "Acciones",
          dataIndex: "option",
          valueType: "option",
          width: 200,
          render: (_, entity) => (
            <EllipsisOutlined
              style={{
                cursor: "pointer",
              }}
              onClick={() => onSelectRow(entity.id!)}
            />
          ),
        },
      ]}
    />
  );
}

export default TransactionListPresentation;
