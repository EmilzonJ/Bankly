using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface IAccountRepository
{
    Task<Account> GetByIdAsync(ObjectId id);
    Task<IEnumerable<Account>> GetAllAsync();
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(ObjectId id);
}
