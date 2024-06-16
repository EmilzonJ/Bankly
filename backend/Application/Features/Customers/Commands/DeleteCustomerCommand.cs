using MongoDB.Bson;

namespace Application.Features.Customers.Commands;

public record DeleteCustomerCommand(ObjectId Id) : ICommand<Result>;
