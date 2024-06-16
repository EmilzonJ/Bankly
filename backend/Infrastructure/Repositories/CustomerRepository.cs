using Domain.Collections;
using Domain.Contracts;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoCollection<Customer> _customers;
    private readonly IMongoCollection<Account> _accounts;

    public CustomerRepository(MongoDbContext context)
    {
        _customers = context.Customers;
        _accounts = context.Accounts;
        var indexKeys = Builders<Customer>.IndexKeys.Text(c => c.Name).Text(c => c.Email);
        _customers.Indexes.CreateOne(new CreateIndexModel<Customer>(indexKeys));
    }

    public async Task<Customer?> GetByIdAsync(ObjectId id)
        => await _customers.Find(c => c.Id == id).FirstOrDefaultAsync();

    public async Task<bool> EmailExistsAsync(string email)
        => await _customers.Find(c => c.Email == email).AnyAsync();

    public async Task<IEnumerable<Customer>> GetAllAsync()
        => await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();

    public async Task AddAsync(Customer customer)
        => await _customers.InsertOneAsync(customer);

    public async Task UpdateAsync(Customer customer)
        => await _customers.ReplaceOneAsync(c => c.Id == customer.Id, customer);

    public async Task DeleteAsync(ObjectId id)
    {
        await _customers.DeleteOneAsync(c => c.Id == id);
        await _accounts.DeleteManyAsync(a => a.CustomerId == id);
    }

    public async Task<int> CountAsync(
        string? name,
        string? email,
        DateTime? registeredAt
    )
    {
        var filter = CreateFilter(name, email, registeredAt);
        return (int) await _customers.CountDocumentsAsync(filter);
    }

    public async Task<IEnumerable<Customer>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        string? email,
        DateTime? registeredAt
    )
    {
        var filter = CreateFilter(name, email, registeredAt);
        return await _customers.Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    private static FilterDefinition<Customer> CreateFilter(string? name, string? email, DateTime? registeredAt)
    {
        var builder = Builders<Customer>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(email))
        {
            var searchTerms = new List<string>();
            if (!string.IsNullOrWhiteSpace(name)) searchTerms.Add(name);
            if (!string.IsNullOrWhiteSpace(email)) searchTerms.Add(email);
            filter &= builder.Text(string.Join(" ", searchTerms));
        }

        if (!registeredAt.HasValue) return filter;

        var start = registeredAt.Value.Date;
        var end = start.AddDays(1).AddTicks(-1);
        filter &= builder.Gte(c => c.RegisteredAt, start) & builder.Lte(c => c.RegisteredAt, end);

        return filter;
    }
}
