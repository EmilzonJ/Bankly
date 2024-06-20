using Application.Features.Auth.Contracts;
using BCrypt.Net;

namespace Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor: 12);

        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool VerifyHashedPassword(string providedPassword, string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword, hashType: HashType.SHA256);
}
