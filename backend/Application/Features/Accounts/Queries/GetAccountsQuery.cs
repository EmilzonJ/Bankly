using Application.Features.Accounts.Models.Responses;
using Application.Shared;
using MongoDB.Bson;

namespace Application.Features.Accounts.Queries;

public record GetAccountsQuery(
    int PageNumber,
    int PageSize,
    ObjectId? Identifier,
    string? Alias = null,
    string? CustomerName = null,
    DateOnly? CreatedAt = null
) : IQuery<Result<PaginatedList<AccountResponse>>>;
