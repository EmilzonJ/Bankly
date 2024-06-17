using Application.Features.Accounts.Extensions;
using Application.Features.Accounts.Models.Responses;
using Application.Features.Accounts.Queries;

namespace Application.Features.Accounts.QueryHandlers;

public record GetAccountByIdQueryHandler(
    IAccountRepository AccountRepository
) : IQueryHandler<GetAccountByIdQuery, Result<AccountResponse>>
{
    public async ValueTask<Result<AccountResponse>> Handle(GetAccountByIdQuery query, CancellationToken cancellationToken)
    {
        var account = await AccountRepository.GetByIdAsync(query.Id);

        if(account is null) return Result.Failure<AccountResponse>(AccountErrors.NotFound(query.Id));

        return account.ToResponse();
    }
}
