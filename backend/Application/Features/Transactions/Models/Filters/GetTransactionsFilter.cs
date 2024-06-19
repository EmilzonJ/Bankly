using Application.Shared;
using Domain.Enums;

namespace Application.Features.Transactions.Models.Filters;

public record GetTransactionsFilter(
    TransactionType? Type,
    string? Reference = null,
    string? Description = null,
    DateOnly? CreatedAt = null
) : PaginationFilter;
