using Application.Features.Customers.Commands;

namespace Application.Features.Customers.CommandHandlers;

public record UpdateCustomerCommandHandler(
    ICustomerRepository CustomerRepository
) : ICommandHandler<UpdateCustomerCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(command.Id);

        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(command.Id));

        if (command.Email != customer.Email && await CustomerRepository.EmailExistsAsync(command.Email))
            return Result.Failure(CustomerErrors.EmailTaken(command.Email));

        customer.Update(command.Name, command.Email);

        await CustomerRepository.UpdateAsync(customer);

        return Result.Success();
    }
}
