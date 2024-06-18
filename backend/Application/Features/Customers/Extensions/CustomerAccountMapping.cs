using Application.Features.Customers.Commands;
using Application.Features.Customers.Models.Responses;
using Domain.Collections;

namespace Application.Features.Customers.Extensions;

public static class CustomerAccountMapping
{
    public static CustomerAccountResponse ToResponse(this Account account)
    {
        return new CustomerAccountResponse
        {
            Id = account.Id.ToString(),
            CustomerId = account.CustomerId.ToString(),
            Alias = account.Alias,
            Balance = account.Balance,
            Type = account.Type,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };
    }

    public static List<CustomerAccountResponse> ToResponse(this IEnumerable<Account> accounts)
    {
        return accounts.Select(a => a.ToResponse()).ToList();
    }

    public static Account ToEntity(this CreateCustomerAccountCommand command)
    {
        return new Account
        {
            CustomerId = command.CustomerId,
            Balance = command.Balance,
            Alias = command.Alias
        };
    }
}
