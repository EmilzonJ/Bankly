using Domain.Base;

namespace Domain.Collections;

public class Customer : CollectionBase
{
    public const string CollectionName = "Customers";

    public required string Name { get; set; }
    public required string Email { get; set; }

    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
