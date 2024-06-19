import { AccountTypes, AccountTypesLabels } from "@/core/enums/account-types.enum";

export const accountTypesMap = (type?: AccountTypes) => {
  if (type === undefined) return '';
  return {
    [AccountTypes.SAVINGS]: AccountTypesLabels.SAVINGS,
  }[type];
};
