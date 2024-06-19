using MongoDB.Bson;
using Shared;

namespace Domain.Errors;

public static class AccountErrors
{
    public static Error NegativeBalance(decimal balance) => Error.Conflict(
        "Accounts.NegativeBalance",
        $"El saldo de la cuenta no puede ser negativo. Saldo actual: {balance}"
    );

    public static Error NotFound(ObjectId id) => Error.NotFound(
        "Accounts.NotFound",
        $"La cuenta con id {id} no fue encontrada."
    );

    public static Error SameAliasExsists(string alias) => Error.Conflict(
        "Accounts.SameAliasExists",
        $"El cliente ya tiene una cuenta con el alias {alias}."
    );
}
