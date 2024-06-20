using Shared;

namespace Domain.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials() => Error.Unauthorized(
        "Auth.InvalidCredentials",
        "Email o contraseña incorrectos."
    );
}
