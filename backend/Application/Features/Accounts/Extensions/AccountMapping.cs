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
            CustomerId = account.CustomerId.ToString(),
            Balance = account.Balance,
            Type = account.Type,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };
    }
}
