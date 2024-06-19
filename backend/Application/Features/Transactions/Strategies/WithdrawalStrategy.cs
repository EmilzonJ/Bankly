using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Models.Requests;
using Domain.Collections;

namespace Application.Features.Transactions.Strategies;

public class WithdrawalStrategy(
    IAccountRepository accountRepository,
    IWithdrawalRepository withdrawalRepository
) : ITransactionStrategy
{
    public async Task<Result<Transaction>> ExecuteAsync(TransactionCreate transactionCreate)
    {
        var account = await accountRepository.GetByIdAsync(transactionCreate.SourceAccountId);

        if (account is null)
            return Result.Failure<Transaction>(
                TransactionErrors.WithdrawalAccountNotFound(transactionCreate.SourceAccountId));

        if (account.Balance < transactionCreate.Amount)
            return Result.Failure<Transaction>(
                TransactionErrors.InsufficientBalance(transactionCreate.Amount, account.Balance));

        return await withdrawalRepository.CreateAsync(
            account,
            transactionCreate.Amount,
            transactionCreate.Description
        );
    }
}
