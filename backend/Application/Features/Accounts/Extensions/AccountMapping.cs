using Application.Features.Accounts.Models.Responses;
using Domain.Collections;

namespace Application.Features.Accounts.Extensions;

public static class AccountMapping
{
    public static AccountResponse ToResponse(this Account account)
    {
        return new AccountResponse
        {
            Id = account.Id.ToString(),
            Alias = account.Alias,
            CustomerId = account.CustomerId.ToString(),
            CustomerName = account.CustomerName,
            Balance = account.Balance,
            Type = account.Type,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };
    }

    public static List<AccountResponse> ToResponse(this IEnumerable<Account> accounts)
    {
        return accounts.Select(ToResponse).ToList();
    }
}
