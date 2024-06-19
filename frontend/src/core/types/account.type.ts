import { AccountTypes } from '../enums/account-types.enum';

export type Account = {
  id: string;
  customerName: string;
  alias: string;
  balance: number;
  type: AccountTypes;
  createdAt: string;
  updatedAt: string;
};

export type AccountParams = {
  alias?: string;
  createdAt?: string;
}

export type CreateAccount = {
  balance: number;
  alias: string;
  customerId: string;
};

export type UpdateAccount = {
  id: string;
} & CreateAccount;
