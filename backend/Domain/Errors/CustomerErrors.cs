using MongoDB.Bson;
using Shared;

namespace Domain.Errors;

public static class CustomerErrors
{
    public static Error EmailTaken(string email) => Error.Conflict(
        "Customers.EmailTaken",
        $"El email '{email}' ya está en uso."
    );

    public static Error NotFound(ObjectId id) => Error.NotFound(
        "Customers.NotFound",
        $"El cliente con el id '{id}' no fue encontrado."
    );
}
