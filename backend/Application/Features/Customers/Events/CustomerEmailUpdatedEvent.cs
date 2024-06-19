namespace Application.Features.Customers.Events;

public record CustomerEmailUpdatedEvent(string CustomerId, string NewEmail);
