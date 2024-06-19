using Domain.Base;
using Domain.Enums;
using MongoDB.Bson;

namespace Domain.Collections;

public class Account : CollectionBase
{
    public const string CollectionName = "Accounts";

    public ObjectId CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public required string CustomerEmail { get; set; }
    public required string Alias { get; set; }
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
}
