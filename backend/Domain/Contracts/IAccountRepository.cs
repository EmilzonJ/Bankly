using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(ObjectId id);
    Task<bool> ExistsAsync(ObjectId id);
    Task<IEnumerable<Account>> GetAllAsync();
    Task<IEnumerable<Account>> GetAllByCustomerAsync(ObjectId customerId);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(Account account);
    Task<bool> SameAliasExistsAsync(ObjectId customerId, string alias);

    Task<int> CountAsync(
        ObjectId? identifier,
        string? alias,
        string? customerName,
        DateOnly? createdAt
    );

    Task<IEnumerable<Account>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        ObjectId? identifier,
        string? alias,
        string? customerName,
        DateOnly? createdAt
    );

    Task UpdateManyAsync(List<Account> accounts);
}
