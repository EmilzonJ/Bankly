using Domain.Enums;

namespace Application.Features.Transactions.Models.Requests;

public record TransactionCreateRequest(
    string SourceAccountId,
    string? DestinationAccountId,
    decimal Amount,
    string Description,
    TransactionType Type
);
