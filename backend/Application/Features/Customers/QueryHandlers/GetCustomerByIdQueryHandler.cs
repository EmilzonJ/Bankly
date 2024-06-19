using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;

namespace Application.Features.Customers.QueryHandlers;

public record GetCustomerByIdQueryHandler(
    ICustomerReadRepository CustomerReadRepository
) : IQueryHandler<GetCustomerByIdQuery, Result<CustomerResponse>>
{
    public async ValueTask<Result<CustomerResponse>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await CustomerReadRepository.GetByIdAsync(query.Id);

        return customer?.ToResponse() ?? Result.Failure<CustomerResponse>(CustomerErrors.NotFound(query.Id));
    }
}
