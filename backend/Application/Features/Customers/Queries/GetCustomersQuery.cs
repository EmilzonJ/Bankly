using Application.Features.Customers.Models.Responses;

namespace Application.Features.Customers.Queries;

public record GetCustomersQuery : IQuery<Result<List<CustomerResponse>>>;