using Application.Features.Transactions.Models.Requests;
using Domain.Collections;

namespace Application.Features.Transactions.Contracts;

public interface ITransactionStrategy
{
    Task<Result<Transaction>> ExecuteAsync(TransactionCreate transactionCreate);
}
