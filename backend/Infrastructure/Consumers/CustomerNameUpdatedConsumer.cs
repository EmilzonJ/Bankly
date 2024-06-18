using Application.Features.Customers.Events;
using Domain.Contracts;
using MassTransit;
using MongoDB.Bson;

namespace Infrastructure.Consumers;

public class CustomerNameUpdatedConsumer(
    IAccountRepository accountRepository
) : IConsumer<CustomerNameUpdatedEvent>
{
    public async Task Consume(ConsumeContext<CustomerNameUpdatedEvent> context)
    {
        var message = context.Message;
        var accounts = await accountRepository.GetAllByCustomerAsync(ObjectId.Parse(message.CustomerId));

        var enumerable = accounts.ToList();

        enumerable.ForEach(account => account.CustomerName = message.NewName);

        await accountRepository.UpdateManyAsync(enumerable);
    }
}
