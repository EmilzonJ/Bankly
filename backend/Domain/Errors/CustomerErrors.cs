using Shared;

namespace Domain.Errors;

public static class CustomerErrors
{
    public static Error EmailTaken(string email) => Error.Conflict(
        "Customers.EmailTaken",
        $"The email '{email}' is already taken."
    );

    public static Error NotFound(string id) => Error.NotFound(
        "Customers.NotFound",
        $"The customer with id '{id}' was not found."
    );
}
