using Domain.Base;
using Domain.Enums;
using MongoDB.Bson;

namespace Domain.Collections;

public class Transaction : CollectionBase
{
    public const string CollectionName = "Transactions";

    public ObjectId AccountId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public ObjectId? DestinationAccountId { get; set; }
}
