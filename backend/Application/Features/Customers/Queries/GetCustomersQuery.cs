using Application.Features.Customers.Models.Responses;
using Mediator;
using Shared;

namespace Application.Features.Customers.Queries;

public record GetCustomersQuery : IQuery<Result<List<CustomerResponse>>>;
