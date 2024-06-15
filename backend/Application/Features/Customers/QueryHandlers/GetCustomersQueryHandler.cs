using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Domain.Contracts;
using Mediator;

namespace Application.Features.Customers.QueryHandlers;

public record GetCustomersQueryHandler(ICustomerRepository Repository) : IQueryHandler<GetCustomersQuery, List<CustomerResponse>>
{
    public async ValueTask<List<CustomerResponse>> Handle(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var customers = await Repository.GetAllAsync();

        return customers.ToResponse();
    }
}
