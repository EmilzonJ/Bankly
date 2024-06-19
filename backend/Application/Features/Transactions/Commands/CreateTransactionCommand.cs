using Domain.Enums;
using MongoDB.Bson;

namespace Application.Features.Transactions.Commands;

public record CreateTransactionCommand(
    ObjectId SourceAccountId,
    ObjectId? DestinationAccountId,
    decimal Amount,
    string Description,
    TransactionType Type
) : ICommand<Result<string>>;
