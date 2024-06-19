import { TransactionTypes } from "../enums/transaction-types.enum";

export type Transaction = {
    id: string;
    type: TransactionTypes;
    amount: number;
    description: string;
    sourceAccountId: string;
    destinationAccountId: string;
    createdAt: string;
}

export type TransactionParams = {
    type?: string;
    sourceAccountId?: string;
    destinationAccountId?: string;
    reference?: string;
    customerName?: string;
}

export type CreateTransaction = {
    type: TransactionTypes;
    amount: number;
    description: string;
    sourceAccountId: string;
    destinationAccountId?: string;
}