using Application.Features.Transactions.Extensions;
using Application.Features.Transactions.Models.Responses;
using Application.Features.Transactions.Queries;
using Application.Shared;
using Domain.Collections;

namespace Application.Features.Transactions.QueryHandlers;

public record GetTransactionsQueryHandler(
    ITransactionRepository TransactionRepository
) : IQueryHandler<GetTransactionsQuery, Result<PaginatedList<TransactionResponse>>>
{
    public async ValueTask<Result<PaginatedList<TransactionResponse>>> Handle(
        GetTransactionsQuery query,
        CancellationToken cancellationToken
    )
    {
        var totalTransactions = await TransactionRepository.CountAsync(
            query.Type,
            query.Reference,
            query.Description,
            query.CreatedAt
        );

        IEnumerable<Transaction> transactions = await TransactionRepository.GetPagedAsync(
            query.PageNumber,
            query.PageSize,
            query.Type,
            query.Reference,
            query.Description,
            query.CreatedAt
        );

        var transactionsResponse = transactions.ToResponse();

        var paginatedList = new PaginatedList<TransactionResponse>(
            transactionsResponse,
            totalTransactions,
            query.PageNumber,
            query.PageSize
        );

        return paginatedList;
    }
}
