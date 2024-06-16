using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Application.Shared;

namespace Application.Features.Customers.QueryHandlers;

public record GetCustomersQueryHandler(
    ICustomerRepository Repository
) : IQueryHandler<GetCustomersQuery, Result<PaginatedList<CustomerResponse>>>
{
    public async ValueTask<Result<PaginatedList<CustomerResponse>>> Handle(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var totalCustomers = await Repository.CountAsync(query.Name, query.Email, query.RegisteredAt);
        var customers = await Repository.GetPagedAsync(query.PageNumber, query.PageSize, query.Name, query.Email, query.RegisteredAt);

        var customerResponses = customers.Select(customer => customer.ToResponse()).ToList();
        var paginatedList = new PaginatedList<CustomerResponse>(
            customerResponses,
            totalCustomers,
            query.PageNumber,
            query.PageSize
        );

        return paginatedList;
    }
}


