using Domain.Collections;

namespace Domain.Contracts;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}
