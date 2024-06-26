using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Application.Shared;
using Domain.Collections;

namespace Application.Features.Customers.QueryHandlers;

public record GetCustomersQueryHandler(
    ICustomerReadRepository CustomerReadRepository
) : IQueryHandler<GetCustomersQuery, Result<PaginatedList<CustomerResponse>>>
{
    public async ValueTask<Result<PaginatedList<CustomerResponse>>> Handle(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var totalCustomers = await CustomerReadRepository.CountAsync(query.Name, query.Email, query.RegisteredAt);
        IEnumerable<Customer> customers = await CustomerReadRepository
            .GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                query.Name,
                query.Email,
                query.RegisteredAt
            );

        var customerResponses = customers.ToResponse();
        var paginatedList = new PaginatedList<CustomerResponse>(
            customerResponses,
            totalCustomers,
            query.PageNumber,
            query.PageSize
        );

        return paginatedList;
    }
}


