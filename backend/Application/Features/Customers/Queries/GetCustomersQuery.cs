using Application.Features.Customers.Models.Responses;
using Mediator;

namespace Application.Features.Customers.Queries;

public record GetCustomersQuery : IQuery<List<CustomerResponse>>;
