using Domain.Enums;

namespace Application.Features.Transactions.Models.Responses;

public record TransactionResponse(
    string Reference,
    string Description,
    decimal Amount,
    TransactionType Type,
    DateTime CreatedAt
);
