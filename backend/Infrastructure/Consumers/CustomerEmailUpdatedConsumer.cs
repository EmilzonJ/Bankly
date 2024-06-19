using Application.Features.Customers.Events;
using Domain.Contracts;
using MassTransit;
using MongoDB.Bson;

namespace Infrastructure.Consumers;

public class CustomerEmailUpdatedConsumer(
    IAccountRepository accountRepository
) : IConsumer<CustomerEmailUpdatedEvent>
{
    public async Task Consume(ConsumeContext<CustomerEmailUpdatedEvent> context)
    {
        var message = context.Message;
        var accounts = await accountRepository.GetAllByCustomerAsync(ObjectId.Parse(message.CustomerId));

        var enumerable = accounts.ToList();

        enumerable.ForEach(account => account.CustomerName = message.NewEmail);

        await accountRepository.UpdateManyAsync(enumerable);
    }
}
