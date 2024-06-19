using Domain.Collections;
using Domain.Enums;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(ObjectId id);
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<IEnumerable<Transaction>> GetAllByAccountAsync(ObjectId accountId);
    Task DeleteAsync(ObjectId id);

    Task<long> CountAsync(
        TransactionType? type,
        string? reference,
        string? description,
        DateOnly? createdAt
    );

    Task<IEnumerable<Transaction>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        TransactionType? type,
        string? reference,
        string? description,
        DateOnly? createdAt
    );
}
