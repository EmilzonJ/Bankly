using Application.Caching;
using Domain.Collections;
using Domain.Contracts;
using Domain.Enums;
using Infrastructure.Caching;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class AccountRepository(
    MongoDbContext context,
    ICacheService cacheService
) : IAccountRepository
{
    private readonly IMongoCollection<Account> _accounts = context.Accounts;
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;

    public async Task<Account?> GetByIdAsync(ObjectId id)
        => await _accounts.Find(CreateActiveFilter(Builders<Account>.Filter.Eq(a => a.Id, id))).FirstOrDefaultAsync();

    public async Task<bool> ExistsAsync(ObjectId id)
        => await _accounts.Find(CreateActiveFilter(Builders<Account>.Filter.Eq(a => a.Id, id))).AnyAsync();

    public async Task<IEnumerable<Account>> GetAllAsync()
        => await _accounts
            .Find(CreateActiveFilter())
            .SortByDescending(a => a.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Account>> GetAllByCustomerAsync(ObjectId customerId)
        => await _accounts
            .Find(CreateActiveFilter(Builders<Account>.Filter.Eq(a => a.CustomerId, customerId)))
            .SortByDescending(a => a.CreatedAt)
            .ToListAsync();

    public async Task AddAsync(Account account)
    {
        account.Type = AccountType.Savings;
        await _accounts.InsertOneAsync(account);
    }

    public async Task UpdateAsync(Account account)
    {
        account.UpdatedAt = DateTime.UtcNow;
        await _accounts.ReplaceOneAsync(a => a.Id == account.Id, account);
    }

    public async Task DeleteAsync(Account account)
    {
        account.IsActive = false;
        await UpdateAsync(account);

        // Mark all transactions as inactive
        var filter = Builders<Transaction>.Filter.Where(t =>
            t.SourceAccountId == account.Id && t.IsActive);
        var update = Builders<Transaction>.Update.Set(t => t.IsActive, false);
        await _transactions.UpdateManyAsync(filter, update);

        await cacheService.RemoveByPrefixAsync(CacheKeyPrefixes.Transaction);
    }

    public async Task<bool> SameAliasExistsAsync(ObjectId customerId, string alias)
        => await _accounts.Find(
            CreateActiveFilter(
                Builders<Account>
                    .Filter
                    .Where(
                        a => a.CustomerId == customerId &&
                             a.Alias.Equals(alias, StringComparison.CurrentCultureIgnoreCase)
                    ))).AnyAsync();

    public async Task<int> CountAsync(ObjectId? identifier, string? alias, string? customerName, DateOnly? createdAt)
    {
        var filter = CreateFilter(identifier, alias, customerName, createdAt);
        return (int) await _accounts.CountDocumentsAsync(CreateActiveFilter(filter));
    }

    public async Task<IEnumerable<Account>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        ObjectId? identifier,
        string? alias,
        string? customerName,
        DateOnly? createdAt
    )
    {
        var filter = CreateFilter(identifier, alias, customerName, createdAt);

        return await _accounts.Find(filter)
            .SortByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task UpdateManyAsync(List<Account> accounts)
    {
        var bulk = new List<WriteModel<Account>>();

        accounts.ForEach(account =>
        {
            account.UpdatedAt = DateTime.UtcNow;
            bulk.Add(new ReplaceOneModel<Account>(Builders<Account>.Filter.Eq(a => a.Id, account.Id), account));
        });

        await _accounts.BulkWriteAsync(bulk);
    }

    private static FilterDefinition<Account> CreateActiveFilter(FilterDefinition<Account>? additionalFilter = null)
    {
        var builder = Builders<Account>.Filter;
        var filter = builder.Eq(a => a.IsActive, true);

        if (additionalFilter != null)
        {
            filter &= additionalFilter;
        }

        return filter;
    }

    private static FilterDefinition<Account> CreateFilter(ObjectId? identifier, string? alias, string? customerName, DateOnly? createAt)
    {
        var builder = Builders<Account>.Filter;
        var filter = CreateActiveFilter(builder.Empty);

        if (identifier is not null)
            filter &= builder.Eq(c => c.Id, identifier);

        if (!string.IsNullOrWhiteSpace(alias))
        {
            filter &= builder.Regex(c => c.Alias, new BsonRegularExpression($"/{alias}/i"));
        }

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            filter &= builder.Regex(c => c.CustomerName, new BsonRegularExpression($"/{customerName}/i"));
        }

        if (!createAt.HasValue) return filter;

        var startOfDay = createAt.Value.ToDateTime(TimeOnly.MinValue);
        var endOfDay = createAt.Value.ToDateTime(TimeOnly.MaxValue);

        filter &= builder.Gte(c => c.CreatedAt, startOfDay) & builder.Lte(c => c.CreatedAt, endOfDay);

        return filter;
    }
}
