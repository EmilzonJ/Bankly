namespace Application.Messaging;

public interface IMessagePublisher
{
    Task Publish<T>(T message) where T : class;
}
