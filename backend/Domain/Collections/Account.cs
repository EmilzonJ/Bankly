using Domain.Base;
using Domain.Enums;
using MongoDB.Bson;

namespace Domain.Collections;

public class Account : CollectionBase
{
    public const string CollectionName = "Accounts";

    public ObjectId CustomerId { get; set; }
    public double Number { get; set; }
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }

}
