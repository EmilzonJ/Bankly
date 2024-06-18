using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(ObjectId id);
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<IEnumerable<Transaction>> GetAllByAccountAsync(ObjectId accountId);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(ObjectId id);
}
