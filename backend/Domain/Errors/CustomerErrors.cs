using Shared;

namespace Domain.Errors;

public static class CustomerErrors
{
    public static Error EmailTaken(string email) => Error.Conflict(
        "Customers.EmailTaken",
        $"The email '{email}' is already taken."
    );
}
