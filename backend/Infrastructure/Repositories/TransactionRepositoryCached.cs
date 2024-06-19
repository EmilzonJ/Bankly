using Application.Caching;
using Domain.Collections;
using Domain.Contracts;
using Domain.Enums;
using Infrastructure.Caching;
using MongoDB.Bson;

namespace Infrastructure.Repositories;

public class TransactionRepositoryCached(
    ITransactionRepository decorated,
    ICacheService cacheService
) : ITransactionRepository
{
    private const string CacheKeyPrefix = CacheKeyPrefixes.Transaction;

    public async Task<Transaction?> GetByIdAsync(ObjectId id)
    {
        string cacheKey = $"{CacheKeyPrefix}{id}";

        return await cacheService.GetAsync(
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

    public async Task DeleteAsync(ObjectId id)
    {
        await decorated.DeleteAsync(id);
        await cacheService.RemoveByPrefixAsync(CacheKeyPrefix);
    }

    public async Task<long> CountAsync(
        TransactionType? type,
        string? reference,
        string? description,
        DateOnly? createdAt
    ) => await decorated.CountAsync(type, reference, description, createdAt);

    public async Task<IEnumerable<Transaction>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        TransactionType? type,
        string? reference,
        string? description,
        DateOnly? createdAt
    )
    {
        string cacheKey = $"{CacheKeyPrefix}Paged_{pageNumber}_{pageSize}_{type}_{reference}_{description}_{createdAt}";

        return await cacheService.GetAsync(
            cacheKey,
            async () => await decorated.GetPagedAsync(pageNumber, pageSize, type, reference, description, createdAt)
        ) ?? [];
    }
}
