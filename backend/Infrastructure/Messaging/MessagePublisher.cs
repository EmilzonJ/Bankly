using Application.Messaging;
using MassTransit;

namespace Infrastructure.Messaging;

public class MessagePublisher(IPublishEndpoint publishEndpoint) : IMessagePublisher
{
    public async Task Publish<T>(T message) where T : class
    {
        await publishEndpoint.Publish(message);
    }
}
