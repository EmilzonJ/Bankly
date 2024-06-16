using Domain.Base;
using MongoDB.Bson;

namespace Domain.Collections;

public class Account : CollectionBase
{
    public const string CollectionName = "Accounts";

    public ObjectId CustomerId { get; set; }
    public decimal Balance { get; set; }
    public DateTime OpenedAt { get; set; }
}