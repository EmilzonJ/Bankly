using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Models.Requests;
using Domain.Collections;

namespace Application.Features.Transactions.Strategies;

public class TransferStrategy(
    IAccountRepository accountRepository,
    ITransferRepository transferRepository
) : ITransactionStrategy
{
    public async Task<Result<Transaction>> ExecuteAsync(TransactionCreate transactionCreate)
    {
        var sourceAccount = await accountRepository.GetByIdAsync(transactionCreate.SourceAccountId);
        var destinationAccount = await accountRepository.GetByIdAsync(transactionCreate.DestinationAccountId!.Value);

        if (sourceAccount is null)
            return Result.Failure<Transaction>(
                TransactionErrors.TransferSourceAccountNotFound(transactionCreate.SourceAccountId));

        if (destinationAccount is null)
            return Result.Failure<Transaction>(
                TransactionErrors.TransferDestinationAccountNotFound(transactionCreate.DestinationAccountId.Value));

        if (sourceAccount.Balance < transactionCreate.Amount)
            return Result.Failure<Transaction>(
                TransactionErrors.InsufficientBalance(transactionCreate.Amount, sourceAccount.Balance));

        if (sourceAccount.Id == destinationAccount.Id)
            return Result.Failure<Transaction>(TransactionErrors.TransferToSameAccount());

        return await transferRepository.CreateAsync(
            sourceAccount,
            destinationAccount,
            transactionCreate.Amount,
            transactionCreate.Description
        );
    }
}
