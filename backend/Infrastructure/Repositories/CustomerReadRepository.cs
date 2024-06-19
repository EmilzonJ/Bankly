using Domain.Collections;
using Domain.Contracts;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CustomerReadRepository(MongoDbContext context) : ICustomerReadRepository
{
    private readonly IMongoCollection<Customer> _customers = context.Customers;

    public async Task<Customer?> GetByIdAsync(ObjectId id)
        => await _customers.Find(CreateActiveFilter(Builders<Customer>.Filter.Eq(c => c.Id, id))).FirstOrDefaultAsync();

    public async Task<bool> ExistsAsync(ObjectId id)
        => await _customers.Find(CreateActiveFilter(Builders<Customer>.Filter.Eq(c => c.Id, id))).AnyAsync();

    public async Task<bool> EmailExistsAsync(string email)
        => await _customers.Find(CreateActiveFilter(Builders<Customer>.Filter.Eq(c => c.Email, email))).AnyAsync();

    public async Task<IEnumerable<Customer>> GetAllAsync()
        => await _customers.Find(CreateActiveFilter()).SortByDescending(c => c.CreatedAt).ToListAsync();
    
    public async Task<int> CountAsync(
        string? name,
        string? email,
        DateOnly? registeredAt
    )
    {
        var filter = CreateFilter(name, email, registeredAt);
        return (int) await _customers.CountDocumentsAsync(CreateActiveFilter(filter));
    }

    public async Task<IEnumerable<Customer>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        string? email,
        DateOnly? registeredAt
    )
    {
        var filter = CreateFilter(name, email, registeredAt);
        return await _customers
            .Find(filter)
            .SortByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    private static FilterDefinition<Customer> CreateFilter(string? name, string? email, DateOnly? registeredAt)
    {
        var builder = Builders<Customer>.Filter;
        var filter = CreateActiveFilter(builder.Empty);

        if (!string.IsNullOrWhiteSpace(name))
        {
            filter &= builder.Regex(c => c.Name, new BsonRegularExpression($"/{name}/i"));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            filter &= builder.Regex(c => c.Email, new BsonRegularExpression($"/{email}/i"));
        }

        if (!registeredAt.HasValue) return filter;

        var startOfDay = registeredAt.Value.ToDateTime(TimeOnly.MinValue);
        var endOfDay = registeredAt.Value.ToDateTime(TimeOnly.MaxValue);

        filter &= builder.Gte(c => c.CreatedAt, startOfDay) & builder.Lte(c => c.CreatedAt, endOfDay);

        return filter;
    }

    private static FilterDefinition<Customer> CreateActiveFilter(FilterDefinition<Customer>? additionalFilter = null)
    {
        var builder = Builders<Customer>.Filter;
        var filter = builder.Eq(c => c.IsActive, true);

        if (additionalFilter != null)
        {
            filter &= additionalFilter;
        }

        return filter;
    }
}
