import {
  TransactionTypes,
  TransactionTypesLabels,
} from "@/core/enums/transaction-types.enum";

export const transactionTypesMap = (type?: TransactionTypes) => {
  if (type === undefined) return "";

  return {
    [TransactionTypes.DEPOSIT]: TransactionTypesLabels.DEPOSIT,
    [TransactionTypes.WITHDRAWAL]: TransactionTypesLabels.WITHDRAWAL,
    [TransactionTypes.OUTGOING_TRANSFER]: TransactionTypesLabels.OUTGOING_TRANSFER,
    [TransactionTypes.INCOMING_TRANSFER]: TransactionTypesLabels.INCOMING_TRANSFER,
  }[type];
};
