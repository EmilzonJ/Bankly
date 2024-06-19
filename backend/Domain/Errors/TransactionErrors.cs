using MongoDB.Bson;
using Shared;

namespace Domain.Errors;

public static class TransactionErrors
{
    public static Error InvalidAmount(decimal transactionAmount) => Error.Validation(
        "Transactions.InvalidAmount",
        $"El monto de la transacción {transactionAmount} es inválido."
    );

    public static Error InsufficientBalance(decimal amount, decimal accountBalance) => Error.Validation(
        "Transactions.InsufficientBalance",
        $"El monto de la transacción {amount} es mayor al saldo de la cuenta {accountBalance}."
    );

    public static Error InvalidDescription => Error.Validation(
        "Transactions.InvalidDescription",
        "La descripción de la transacción es inválida."
    );

    public static Error NotFound(ObjectId id) => Error.NotFound(
        "Transactions.NotFound",
        $"La transacción con id {id} no fue encontrada."
    );

    public static Error DepositAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.DepositAccountNotFound",
        $"La cuenta para la transacción de depósito con id {accountId} no fue encontrada."
    );

    public static Error WithdrawalAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.WithdrawalAccountNotFound",
        $"La cuenta para la transacción de retiro con id {accountId} no fue encontrada."
    );

    public static Error TransferSourceAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.TransferSourceAccountNotFound",
        $"La cuenta de origen para la transacción de transferencia con id {accountId} no fue encontrada."
    );

    public static Error TransferDestinationAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.TransferDestinationAccountNotFound",
        $"La cuenta de destino para la transacción de transferencia con id {accountId} no fue encontrada."
    );

    public static Error TransferToSameAccount() => Error.Conflict(
        "Transactions.TransferToSameAccount",
        "La cuenta de origen y destino de la transacción de transferencia son iguales."
    );
}
