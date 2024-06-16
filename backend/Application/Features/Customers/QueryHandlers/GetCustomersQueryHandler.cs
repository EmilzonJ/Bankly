using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;

namespace Application.Features.Customers.QueryHandlers;

public record GetCustomersQueryHandler(ICustomerRepository Repository) : IQueryHandler<GetCustomersQuery, Result<List<CustomerResponse>>>
{
    public async ValueTask<Result<List<CustomerResponse>>> Handle(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var customers = await Repository.GetAllAsync();

        return customers.ToResponse();
    }
}
