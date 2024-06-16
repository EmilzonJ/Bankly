using MongoDB.Bson;
using Shared;

namespace Domain.Errors;

public static class CustomerErrors
{
    public static Error EmailTaken(string email) => Error.Conflict(
        "Customers.EmailTaken",
        $"The email '{email}' is already taken."
    );

    public static Error NotFound(ObjectId id) => Error.NotFound(
        "Customers.NotFound",
        $"The customer with id '{id.ToString()}' was not found."
    );
}
