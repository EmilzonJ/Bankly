using Shared;

namespace Domain.Errors;

public class AccountErrors
{
    public static Error NegativeBalance(decimal balance) => Error.Conflict(
        "Accounts.NegativeBalance",
        $"The account balance can't be negative: {balance}."
    );
}
