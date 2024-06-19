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

    public required ObjectId SourceAccountId { get; set; }
    public required TransactionAccount SourceAccount { get; set; }

    public ObjectId? DestinationAccountId { get; set; }
    public TransactionAccount? DestinationAccount { get; set; }
}

public class TransactionAccount
{
    public required ObjectId Id { get; set; }
    public required string Alias { get; set; }

    public required ObjectId CustomerId { get; set; }
    public required TransactionAccountCustomer Customer { get; set; }
}

public class TransactionAccountCustomer
{
    public required ObjectId Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
