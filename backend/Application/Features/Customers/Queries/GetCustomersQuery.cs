using Application.Features.Customers.Models.Responses;
using Application.Shared;

namespace Application.Features.Customers.Queries;

public record GetCustomersQuery(
    int PageNumber,
    int PageSize,
    string? Name = null,
    string? Email = null,
    DateOnly? RegisteredAt = null
) : IQuery<Result<PaginatedList<CustomerResponse>>>;

