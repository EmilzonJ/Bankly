using Application.Features.Customers.Models.Responses;
using MongoDB.Bson;

namespace Application.Features.Customers.Queries;

public record GetCustomerAccountsQuery(
    ObjectId CustomerId
) : IQuery<Result<List<CustomerAccountResponse>>>;
