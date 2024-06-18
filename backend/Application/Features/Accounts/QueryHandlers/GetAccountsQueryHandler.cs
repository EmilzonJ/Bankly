using Application.Features.Accounts.Extensions;
using Application.Features.Accounts.Models.Responses;
using Application.Features.Accounts.Queries;
using Application.Shared;
using Domain.Collections;

namespace Application.Features.Accounts.QueryHandlers;

public record GetAccountsQueryHandler(
    IAccountRepository AccountRepository
) : IQueryHandler<GetAccountsQuery, Result<PaginatedList<AccountResponse>>>
{
    public async ValueTask<Result<PaginatedList<AccountResponse>>> Handle(GetAccountsQuery query,
        CancellationToken cancellationToken)
    {
        var totalAccounts = await AccountRepository.CountAsync(query.Alias, query.CustomerName, query.CreatedAt);
        IEnumerable<Account> accounts = await AccountRepository
            .GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                query.Alias,
                query.CustomerName,
                query.CreatedAt
            );

        var accountsResponse = accounts.ToResponse();
        var paginatedList = new PaginatedList<AccountResponse>(
            accountsResponse,
            totalAccounts,
            query.PageNumber,
            query.PageSize
        );

        return paginatedList;
    }
}
