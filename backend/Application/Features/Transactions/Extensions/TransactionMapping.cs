using Application.Features.Transactions.Models.Responses;
using Domain.Collections;

namespace Application.Features.Transactions.Extensions;

public static class TransactionMapping
{
    public static TransactionResponse ToResponse(this Transaction transaction)
    {
        return new TransactionResponse(
            transaction.Id.ToString(),
            transaction.Description,
            transaction.Amount,
            transaction.Type,
            transaction.CreatedAt
        );
    }

    public static List<TransactionResponse> ToResponse(this IEnumerable<Transaction> transactions)
    {
        return transactions.Select(t => t.ToResponse()).ToList();
    }
}
