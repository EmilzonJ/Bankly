import { Col, Row, Table } from "antd";

import React from "react";
import { Statistic, Card } from "antd";
import { Line, Pie } from "@ant-design/charts";
import { TransactionTypes } from "@/core/enums/transaction-types.enum";

interface TotalDataProps {
  title: string;
  totalCount: number;
}

const TotalDataStats: React.FC<TotalDataProps> = ({ title, totalCount }) => (
  <Card>
    <Statistic title={title} value={totalCount} />
  </Card>
);

// ------------------------------
export interface TransactionData {
  reference: string;
  type: TransactionTypes;
  amount: number;
  description: string;
  createdAt: string;
}

interface RecentTransactionsProps {
  transactions: TransactionData[];
}

const columns = [
  { title: "Referencia", dataIndex: "reference", key: "reference" },
  { title: "Tipo", dataIndex: "type", key: "type" },
  { title: "Monto", dataIndex: "amount", key: "amount" },
  { title: "Descripción", dataIndex: "description", key: "description" },
  { title: "Fecha", dataIndex: "createdAt", key: "createdAt" },
];

const RecentTransactions: React.FC<RecentTransactionsProps> = ({
  transactions,
}) => (
  <Card title="Recent Transactions">
    <Table columns={columns} dataSource={transactions} rowKey="id" />
  </Card>
);

// ------------------------------
export interface TransactionTypeData {
  type: string;
  value: number;
}

interface TransactionTypeDistributionProps {
  data: TransactionTypeData[];
}

const TransactionTypeDistribution: React.FC<
  TransactionTypeDistributionProps
> = ({ data }) => {
  const config = {
    data,
    angleField: "value",
    colorField: "type",
    label: {
      text: "value",
      style: {
        fontWeight: "bold",
      },
    },
    legend: {
      color: {
        title: false,
        position: "right",
        rowPadding: 5,
      },
    },
  };

  return (
    <Card title="Distribución de transacciones">
      <Pie {...config} />
    </Card>
  );
};

// ------------------------------

export interface DailyTransactionData {
  fecha: string;
  transacciones: number;
}

interface DailyTransactionsProps {
  data: DailyTransactionData[];
}

const DailyTransactions: React.FC<DailyTransactionsProps> = ({ data }) => {
  const config = {
    data,
    xField: "fecha",
    yField: "transacciones",
    point: {
      size: 5,
      shape: "diamond",
    },
  };

  return (
    <Card title="Daily Transactions">
      <Line {...config}/>
    </Card>
  );
};

// ------------------------------

interface DashboardProps {
  totalCustomers: number;
  transactions: TransactionData[];
  transactionTypeData: TransactionTypeData[];
  dailyTransactionData: DailyTransactionData[];
}

function DashboardPresentation({
  totalCustomers,
  transactions,
  transactionTypeData,
  dailyTransactionData,
}: DashboardProps) {
  return (
    <Row gutter={[16, 16]}>
      <Col span={8}>
        <TotalDataStats
          title="Total de transacciones"
          totalCount={totalCustomers}
        />
      </Col>
      <Col span={8}>
        <TotalDataStats title="Total de cuentas" totalCount={totalCustomers} />
      </Col>
      <Col span={8}>
        <TotalDataStats title="Total de clientes" totalCount={totalCustomers} />
      </Col>
      <Col span={12}>
        <TransactionTypeDistribution data={transactionTypeData} />
      </Col>
      <Col span={12}>
        <DailyTransactions data={dailyTransactionData} />
      </Col>
      <Col span={24}>
        <RecentTransactions transactions={transactions} />
      </Col>
    </Row>
  );
}

export default DashboardPresentation;
