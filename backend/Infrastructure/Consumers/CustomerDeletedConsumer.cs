using Application.Features.Customers.Events;
using Domain.Contracts;
using MassTransit;
using MongoDB.Bson;

namespace Infrastructure.Consumers;

public class CustomerDeletedConsumer(
    ICustomerWriteRepository customerWriteRepository
) : IConsumer<CustomerDeletedEvent>
{
    public async Task Consume(ConsumeContext<CustomerDeletedEvent> context)
    {
        var customerId = context.Message.CustomerId;

        await customerWriteRepository.DeleteRelatedDataAsync(ObjectId.Parse(customerId));
    }
}
