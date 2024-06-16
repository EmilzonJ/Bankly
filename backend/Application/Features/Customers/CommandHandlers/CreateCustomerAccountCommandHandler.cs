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
        var account = command.ToEntity();

        if (!await CustomerRepository.ExistsAsync(account.CustomerId))
            return Result.Failure<string>(CustomerErrors.NotFound(account.CustomerId));

        if (account.Balance < 0)
            return Result.Failure<string>(AccountErrors.NegativeBalance(account.Balance));

        await AccountRepository.AddAsync(account);

        return account.Id.ToString();
    }
}
