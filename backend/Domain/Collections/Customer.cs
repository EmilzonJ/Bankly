using Domain.Base;

namespace Domain.Collections;

public class Customer : CollectionBase
{
    public const string CollectionName = "Customers";

    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime RegisteredAt { get; set; }
}
