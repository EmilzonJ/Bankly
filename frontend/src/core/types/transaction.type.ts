import { TransactionTypes } from '../enums/transaction-types.enum';

export type Transaction = {
  reference: string;
  type: TransactionTypes;
  amount: number;
  description: string;
  sourceAccountId: string;
  destinationAccountId: string;
  createdAt: string;
  id: string;
};

export type TransacttionById = {
  reference: string;
  type: TransactionTypes;
  description: string;
  amount: number;
  sourceAccount: SourceAccount;
  destinationAccount: SourceAccount;
  createdAt: string;
};

export type SourceAccount = {
  id: string;
  alias: string;
  customerId: string;
  customer: Customer;
};

export type Customer = {
  id: string;
  name: string;
  email: string;
};

export type TransactionParams = {
  type?: string;
  sourceAccountId?: string;
  destinationAccountId?: string;
  reference?: string;
  customerName?: string;
};

export type CreateTransaction = {
  type: TransactionTypes;
  amount: number;
  description: string;
  sourceAccountId: string;
  destinationAccountId?: string;
};
