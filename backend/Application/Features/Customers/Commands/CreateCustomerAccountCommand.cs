using MongoDB.Bson;

namespace Application.Features.Customers.Commands;

public record CreateCustomerAccountCommand(
    ObjectId CustomerId,
    string Alias,
    decimal Balance
) : ICommand<Result<string>>;
