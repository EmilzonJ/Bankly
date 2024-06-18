namespace Application.Features.Customers.Events;

public record CustomerNameUpdatedEvent(string CustomerId, string NewName);
