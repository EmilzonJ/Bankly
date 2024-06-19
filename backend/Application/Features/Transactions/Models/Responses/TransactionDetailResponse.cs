using Domain.Enums;

namespace Application.Features.Transactions.Models.Responses;

public record TransactionDetailResponse(
    string Reference,
    TransactionType Type,
    string Description,
    decimal Amount,
    TransactionAccountResponse SourceAccount,
    TransactionAccountResponse? DestinationAccount,
    DateTime CreatedAt
);

public record TransactionAccountResponse(
    string Id,
    string Alias,
    string CustomerId,
    TransactionAccountCustomerResponse Customer
);

public record TransactionAccountCustomerResponse(
    string Id,
    string Name,
    string Email
);
