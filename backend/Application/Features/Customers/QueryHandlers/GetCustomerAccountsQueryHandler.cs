using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;

namespace Application.Features.Customers.QueryHandlers;

public record GetCustomerAccountsQueryHandler(
    ICustomerReadRepository CustomerReadRepository,
    IAccountRepository AccountRepository
) : IQueryHandler<GetCustomerAccountsQuery, Result<List<CustomerAccountResponse>>>
{
    public async ValueTask<Result<List<CustomerAccountResponse>>> Handle(GetCustomerAccountsQuery query,
        CancellationToken cancellationToken)
    {
        if (!await CustomerReadRepository.ExistsAsync(query.CustomerId))
            return Result.Failure<List<CustomerAccountResponse>>(CustomerErrors.NotFound(query.CustomerId));

        var accounts = await AccountRepository.GetAllByCustomerAsync(query.CustomerId);

        return accounts.ToResponse();
    }
}
