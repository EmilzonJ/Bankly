using Application.Features.Customers.Commands;
using Application.Features.Customers.Events;
using Application.Messaging;

namespace Application.Features.Customers.CommandHandlers;

public record UpdateCustomerCommandHandler(
    ICustomerWriteRepository CustomerWriteRepository,
    ICustomerReadRepository CustomerReadRepository,
    IMessagePublisher MessagePublisher
) : ICommandHandler<UpdateCustomerCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await CustomerReadRepository.GetByIdAsync(command.Id);

        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(command.Id));

        if (command.Email != customer.Email && await CustomerReadRepository.EmailExistsAsync(command.Email))
            return Result.Failure(CustomerErrors.EmailTaken(command.Email));

        bool isNewName = command.Name != customer.Name;
        bool isNewEmail = command.Email != customer.Email;

        customer.Update(command.Name, command.Email);

        await CustomerWriteRepository.UpdateAsync(customer);

        if (isNewName)
            await MessagePublisher.Publish(new CustomerNameUpdatedEvent(command.Id.ToString(), command.Name));

        if (isNewEmail)
            await MessagePublisher.Publish(new CustomerEmailUpdatedEvent(command.Id.ToString(), command.Email));

        return Result.Success();
    }
}
