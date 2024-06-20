using Domain.Base;

namespace Domain.Collections;

public class User : CollectionBase
{
    public const string CollectionName = "Users";

    public required string Email { get; set; }
    public required string Password { get; set; }
}
