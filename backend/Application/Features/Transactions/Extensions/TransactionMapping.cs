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

    private static TransactionAccountCustomerResponse ToResponse(this TransactionAccountCustomer customer)
    {
        return new TransactionAccountCustomerResponse(
            customer.Id.ToString(),
            customer.Name,
            customer.Email
        );
    }

    private static TransactionAccountResponse? ToResponse(this TransactionAccount? account)
    {
        if (account is null)
        {
            return null;
        }

        return new TransactionAccountResponse(
            account.Id.ToString(),
            account.Alias,
            account.CustomerId.ToString(),
            account.Customer.ToResponse()
        );
    }

    public static TransactionDetailResponse ToDetailResponse(this Transaction transaction)
    {
        return new TransactionDetailResponse(
            transaction.Id.ToString(),
            transaction.Type,
            transaction.Description,
            transaction.Amount,
            transaction.SourceAccount.ToResponse()!,
            transaction.DestinationAccount?.ToResponse(),
            transaction.CreatedAt
        );
    }
}
