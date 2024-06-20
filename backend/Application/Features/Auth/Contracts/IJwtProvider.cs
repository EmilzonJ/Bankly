namespace Application.Features.Auth.Contracts;

public interface IJwtProvider
{
    public string GenerateToken(string userId, string email);
}
