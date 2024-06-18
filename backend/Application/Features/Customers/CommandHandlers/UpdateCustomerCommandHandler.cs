using Application.Features.Customers.Commands;
using Application.Features.Customers.Events;
using Application.Messaging;

namespace Application.Features.Customers.CommandHandlers;

public record UpdateCustomerCommandHandler(
    ICustomerRepository CustomerRepository,
    IMessagePublisher MessagePublisher
) : ICommandHandler<UpdateCustomerCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(command.Id);

        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(command.Id));

        if (command.Email != customer.Email && await CustomerRepository.EmailExistsAsync(command.Email))
            return Result.Failure(CustomerErrors.EmailTaken(command.Email));

        bool isNewName = command.Name != customer.Name;

        customer.Update(command.Name, command.Email);

        await CustomerRepository.UpdateAsync(customer);

        if (isNewName)
            await MessagePublisher.Publish(new CustomerNameUpdatedEvent(command.Id.ToString(), command.Name));

        return Result.Success();
    }
}
