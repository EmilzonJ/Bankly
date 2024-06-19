export enum TransactionTypes {
  DEPOSIT = 0,
  WITHDRAWAL = 1,
  OUTGOING_TRANSFER = 2,
  INCOMING_TRANSFER = 3,
}

export enum TransactionTypesLabels {
  DEPOSIT = "Depósito",
  WITHDRAWAL = "Retiro",
  OUTGOING_TRANSFER = "Transferencia saliente",
  INCOMING_TRANSFER = "Transferencia entrante",
}

export const transactionTypesColors = {
  [TransactionTypes.DEPOSIT]: "green",
  [TransactionTypes.WITHDRAWAL]: "red",
  [TransactionTypes.OUTGOING_TRANSFER]: "red",
  [TransactionTypes.INCOMING_TRANSFER]: "green",
};
