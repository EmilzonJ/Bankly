using Application.Features.Accounts.Extensions;
using Application.Features.Accounts.Models.Responses;
using Application.Features.Accounts.Queries;

namespace Application.Features.Accounts.QueryHandlers;

public record GetAccountTransactionsQueryHandler(
    IAccountRepository AccountRepository,
    ITransactionRepository TransactionRepository
) : IQueryHandler<GetAccountTransactionsQuery, Result<List<AccountTransactionResponse>>>
{
    public async ValueTask<Result<List<AccountTransactionResponse>>> Handle(
        GetAccountTransactionsQuery query,
        CancellationToken cancellationToken
    )
    {
        if (!await AccountRepository.ExistsAsync(query.Id))
            return Result.Failure<List<AccountTransactionResponse>>(AccountErrors.NotFound(query.Id));

        var transactions = await TransactionRepository.GetAllByAccountAsync(query.Id);

        return transactions.ToResponse();
    }
}
