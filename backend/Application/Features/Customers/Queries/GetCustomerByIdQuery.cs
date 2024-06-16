using Application.Features.Customers.Models.Responses;
using MongoDB.Bson;

namespace Application.Features.Customers.Queries;

public record GetCustomerByIdQuery(ObjectId Id) : IQuery<Result<CustomerResponse>>;
