using Application.Features.Accounts.Models.Responses;
using Domain.Collections;

namespace Application.Features.Accounts.Extensions;

public static class AccountTransactionMapping
{
    public static AccountTransactionResponse ToResponse(this Transaction transaction)
    {
        return new AccountTransactionResponse
        {
            Id = transaction.Id.ToString(),
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }

    public static List<AccountTransactionResponse> ToResponse(this IEnumerable<Transaction> transactions)
        => transactions.Select(ToResponse).ToList();
}
