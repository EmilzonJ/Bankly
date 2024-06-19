using Application.Features.Customers.Commands;
using Application.Features.Customers.Extensions;

namespace Application.Features.Customers.CommandHandlers;

public record CreateCustomerAccountCommandHandler(
    ICustomerRepository CustomerRepository,
    IAccountRepository AccountRepository
) : ICommandHandler<CreateCustomerAccountCommand, Result<string>>
{
    public async ValueTask<Result<string>> Handle(CreateCustomerAccountCommand command,
        CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(command.CustomerId);
        if (customer is null)
            return Result.Failure<string>(CustomerErrors.NotFound(command.CustomerId));

        var account = command.ToEntity(customer.Name, customer.Email);

        if (account.Balance < 0)
            return Result.Failure<string>(AccountErrors.NegativeBalance(account.Balance));

        if (await AccountRepository.SameAliasExistsAsync(command.CustomerId, command.Alias))
            return Result.Failure<string>(AccountErrors.SameAliasExsists(command.Alias));

        await AccountRepository.AddAsync(account);

        return account.Id.ToString();
    }
}
