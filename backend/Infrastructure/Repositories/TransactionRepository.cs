using Domain.Collections;
using Domain.Contracts;
using Domain.Enums;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class TransactionRepository(MongoDbContext context) : ITransactionRepository
{
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;

    public async Task<Transaction?> GetByIdAsync(ObjectId id)
        => await _transactions
            .Find(CreateActiveFilter(Builders<Transaction>.Filter.Eq(c => c.Id, id)))
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _transactions
            .Find(CreateActiveFilter(Builders<Transaction>.Filter.Empty))
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllByAccountAsync(ObjectId accountId)
        => await _transactions
            .Find(CreateActiveFilter(Builders<Transaction>.Filter.Eq(t => t.SourceAccountId, accountId)))
            .SortByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task DeleteAsync(ObjectId id)
    {
        await _transactions.DeleteOneAsync(t => t.Id == id);
    }

    public async Task<long> CountAsync(
        TransactionType? type,
        string? reference,
        string? description,
        DateOnly? createdAt
    )
    {
        var filter = CreateFilter(type, reference, description, createdAt);

        return await _transactions.CountDocumentsAsync(CreateActiveFilter(filter));
    }

    public async Task<IEnumerable<Transaction>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        TransactionType? type,
        string? reference,
        string? description,
        DateOnly? createdAt
    )
    {
        var filter = CreateFilter(type, reference, description, createdAt);

        return await _transactions
            .Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    private static FilterDefinition<Transaction> CreateFilter(
        TransactionType? type,
        string? reference,
        string? description,
        DateOnly? createdAt
    )
    {
        var builder = Builders<Transaction>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(reference))
            filter &= builder.Where(t => t.Id == ObjectId.Parse(reference));

        if (type.HasValue)
            filter &= builder.Eq(t => t.Type, type);

        if (!string.IsNullOrWhiteSpace(description))
            filter &= builder.Regex(t => t.Description, new BsonRegularExpression(description, "i"));

        if (!createdAt.HasValue) return filter;

        var startOfDay = createdAt.Value.ToDateTime(TimeOnly.MinValue);
        var endOfDay = createdAt.Value.ToDateTime(TimeOnly.MaxValue);

        filter &= builder.Gte(c => c.CreatedAt, startOfDay) & builder.Lte(c => c.CreatedAt, endOfDay);

        return CreateActiveFilter(filter);
    }

    private static FilterDefinition<Transaction> CreateActiveFilter(FilterDefinition<Transaction>? additionalFilter = null)
    {
        var builder = Builders<Transaction>.Filter;
        var filter = builder.Eq(t => t.IsActive, true);

        return additionalFilter == null ? filter : filter & additionalFilter;
    }
}
