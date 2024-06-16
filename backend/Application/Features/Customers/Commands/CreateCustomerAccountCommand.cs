using MongoDB.Bson;

namespace Application.Features.Customers.Commands;

public record CreateCustomerAccountCommand(
    ObjectId CustomerId,
    decimal Balance
) : ICommand<Result<string>>;
