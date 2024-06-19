using MongoDB.Bson;
using Shared;

namespace Domain.Errors;

public static class TransactionErrors
{
    public static Error NotFound(ObjectId id) => Error.NotFound(
        "Transactions.NotFound",
        $"The transaction with id {id} was not found."
    );
}
