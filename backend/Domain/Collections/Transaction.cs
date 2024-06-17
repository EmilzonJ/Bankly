using Domain.Base;
using Domain.Enums;
using MongoDB.Bson;

namespace Domain.Collections;

public class Transaction : CollectionBase
{
    public const string CollectionName = "Transactions";

    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public required string Description { get; set; }

    public ObjectId SourceAccountId { get; set; }
    public ObjectId? DestinationAccountId { get; set; }
}
