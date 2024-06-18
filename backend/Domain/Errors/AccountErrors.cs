using MongoDB.Bson;
using Shared;

namespace Domain.Errors;

public static class AccountErrors
{
    public static Error NegativeBalance(decimal balance) => Error.Conflict(
        "Accounts.NegativeBalance",
        $"The account balance can't be negative: {balance}."
    );

    public static Error NotFound(ObjectId id) => Error.NotFound(
        "Accounts.NotFound",
        $"The account with id {id} was not found."
    );

    public static Error SameAliasExsists(string alias) => Error.Conflict(
        "Accounts.SameAliasExists",
        $"You have an account with the same alias {alias}"
    );
}
