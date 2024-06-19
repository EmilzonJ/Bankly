using Application.Features.Transactions.Extensions;
using Application.Features.Transactions.Models.Responses;
using Application.Features.Transactions.Queries;

namespace Application.Features.Transactions.QueryHandlers;

public record GetTransactionByIdQueryHandler(
    ITransactionRepository TransactionRepository
) : IQueryHandler<GetTransactionByIdQuery, Result<TransactionDetailResponse>>
{
    public async ValueTask<Result<TransactionDetailResponse>> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        var transaction = await TransactionRepository.GetByIdAsync(query.Id);

        if (transaction is null)
            return Result.Failure<TransactionDetailResponse>(TransactionErrors.NotFound(query.Id));

        return transaction.ToDetailResponse();
    }
}
