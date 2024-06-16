using Application.Features.Customers.Commands;

namespace Application.Features.Customers.CommandHandlers;

public record DeleteCustomerCommandHandler(
    ICustomerRepository CustomerRepository
) : ICommandHandler<DeleteCustomerCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(command.Id);

        if(customer is null)
            return Result.Failure(CustomerErrors.NotFound(command.Id));

        await CustomerRepository.DeleteAsync(command.Id);

        return Result.Success();
    }
}
