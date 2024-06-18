using Domain.Enums;

namespace Application.Features.Accounts.Models.Responses;

public record AccountResponse
{
    public required string Id { get; init; }
    public required string Alias { get; set; }
    public required string CustomerId { get; init; }
    public required string CustomerName { get; init; }
    public decimal Balance { get; init; }
    public AccountType Type { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
