using MongoDB.Bson;

namespace Application.Features.Customers.Commands;

public record UpdateCustomerCommand(
    ObjectId Id,
    string Name,
    string Email
) : ICommand<Result>;
