using MongoDB.Bson;
using Shared;

namespace Domain.Errors;

public static class TransactionErrors
{
    public static Error InvalidAmount(decimal transactionAmount) => Error.Failure(
        "Transactions.InvalidAmount",
        $"The transaction amount {transactionAmount} is invalid."
    );

    public static Error InsufficientBalance(decimal amount, decimal accountBalance) => Error.Failure(
        "Transactions.InsufficientBalance",
        $"The transaction amount {amount} is greater than the account balance {accountBalance}."
    );

    public static Error InvalidDescription => Error.Failure(
        "Transactions.InvalidDescription",
        "The transaction description is invalid."
    );

    public static Error NotFound(ObjectId id) => Error.NotFound(
        "Transactions.NotFound",
        $"The transaction with id {id} was not found."
    );

    public static Error DepositAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.DepositAccountNotFound",
        $"The account for the deposit transaction with id {accountId} was not found."
    );

    public static Error WithdrawalAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.WithdrawalAccountNotFound",
        $"The account for the withdrawal transaction with id {accountId} was not found."
    );

    public static Error TransferSourceAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.TransferSourceAccountNotFound",
        $"The source account for the transfer transaction with id {accountId} was not found."
    );

    public static Error TransferDestinationAccountNotFound(ObjectId accountId) => Error.NotFound(
        "Transactions.TransferDestinationAccountNotFound",
        $"The destination account for the transfer transaction with id {accountId} was not found."
    );

    public static Error TransferToSameAccount() => Error.Conflict(
        "Transactions.TransferToSameAccount",
        "The source and destination accounts for the transfer transaction are the same."
    );
}
