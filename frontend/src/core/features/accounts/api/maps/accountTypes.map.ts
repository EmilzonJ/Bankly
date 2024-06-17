import { AccountTypes } from '@/core/enums/accountTypes';

export const accountTpeMap = (type?: AccountTypes) => {
  if (type === undefined) return '';
  return {
    [AccountTypes.SAVINGS]: 'Ahorros',
  }[type];
};
