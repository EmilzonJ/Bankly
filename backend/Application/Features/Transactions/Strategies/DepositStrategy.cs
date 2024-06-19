using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Models.Requests;
using Domain.Collections;

namespace Application.Features.Transactions.Strategies;

public class DepositStrategy(
    IAccountRepository accountRepository,
    IDepositRepository depositRepository
) : ITransactionStrategy
{
    public async Task<Result<Transaction>> ExecuteAsync(TransactionCreate transactionCreate)
    {
        var account = await accountRepository.GetByIdAsync(transactionCreate.SourceAccountId);

        if (account is null)
            return Result.Failure<Transaction>(
                TransactionErrors.DepositAccountNotFound(transactionCreate.SourceAccountId));

        return await depositRepository.CreateAsync(
            account,
            transactionCreate.Amount,
            transactionCreate.Description
        );
    }
}
