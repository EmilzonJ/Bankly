using Application.Features.Customers.Commands;
using Application.Features.Customers.Events;
using Application.Messaging;

namespace Application.Features.Customers.CommandHandlers;

public record DeleteCustomerCommandHandler(
    ICustomerReadRepository CustomerReadRepository,
    ICustomerWriteRepository CustomerWriteRepository,
    IMessagePublisher MessagePublisher
) : ICommandHandler<DeleteCustomerCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await CustomerReadRepository.GetByIdAsync(command.Id);

        if(customer is null)
            return Result.Failure(CustomerErrors.NotFound(command.Id));

        await CustomerWriteRepository.DeleteAsync(customer);

        await MessagePublisher.Publish(new CustomerDeletedEvent(customer.Id.ToString()));

        return Result.Success();
    }
}
