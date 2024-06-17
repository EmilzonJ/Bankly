using Domain.Enums;

namespace Application.Features.Accounts.Models.Responses;

public record AccountTransactionResponse
{
    public required string Id { get; init; }
    public required string Description { get; init; }
    public decimal Amount { get; init; }
    public TransactionType Type { get; init; }
    public DateTime CreatedAt { get; init; }
}
