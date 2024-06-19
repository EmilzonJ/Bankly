using Application.Features.Transactions.Models.Responses;
using Application.Shared;
using Domain.Enums;

namespace Application.Features.Transactions.Queries;

public record GetTransactionsQuery(
    int PageNumber,
    int PageSize,
    TransactionType? Type,
    string? Reference = null,
    string? Description = null,
    DateOnly? CreatedAt = null
) : IQuery<Result<PaginatedList<TransactionResponse>>>;
