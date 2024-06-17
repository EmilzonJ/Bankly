import { AccountTypes } from '../enums/accountTypes';

export type Account = {
  id: string;
  customerId: string;
  balance: number;
  type: AccountTypes;
  createdAt: string;
  updatedAt: string;
};

export type CreateAccount = {
  balance: number;
  customerId: string;
};
