using Application.Caching;
using Domain.Collections;
using Domain.Contracts;
using MongoDB.Bson;

namespace Infrastructure.Repositories;

public class TransactionRepositoryCached(
    ITransactionRepository decorated,
    ICacheService cacheService
) : ITransactionRepository
{
    private const string CacheKeyPrefix = "Transaction_";

    public async Task<Transaction?> GetByIdAsync(ObjectId id)
    {
        string cacheKey = $"{CacheKeyPrefix}{id}";

        return await cacheService.GetAsync<Transaction?>(
            cacheKey,
            async () => await decorated.GetByIdAsync(id)
        );
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        string cacheKey = $"{CacheKeyPrefix}All";

        return await cacheService.GetAsync(
            cacheKey,
            async () => await decorated.GetAllAsync()
        ) ?? [];
    }

    public async Task<IEnumerable<Transaction>> GetAllByAccountAsync(ObjectId accountId)
    {
        string cacheKey = $"{CacheKeyPrefix}Account_{accountId}";

        return await cacheService.GetAsync(
            cacheKey,
            async () => await decorated.GetAllByAccountAsync(accountId)
        ) ?? [];
    }

    public async Task AddAsync(Transaction transaction)
    {
        await decorated.AddAsync(transaction);
        await cacheService.RemoveByPrefixAsync(CacheKeyPrefix);
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        await decorated.UpdateAsync(transaction);
        await cacheService.RemoveAsync($"{CacheKeyPrefix}{transaction.Id}");
        await cacheService.RemoveByPrefixAsync(CacheKeyPrefix);
    }

    public async Task DeleteAsync(ObjectId id)
    {
        await decorated.DeleteAsync(id);
        await cacheService.RemoveAsync($"{CacheKeyPrefix}{id}");
        await cacheService.RemoveByPrefixAsync(CacheKeyPrefix);
    }
}
