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
    private readonly IMongoCollection<Transaction> _transactions;

    public CustomerRepository(MongoDbContext context)
    {
        _customers = context.Customers;
        _accounts = context.Accounts;
        _transactions = context.Transactions;

        var nameIndexKeys = Builders<Customer>.IndexKeys.Ascending(c => c.Name);
        var emailIndexKeys = Builders<Customer>.IndexKeys.Ascending(c => c.Email);

        _customers.Indexes.CreateOne(new CreateIndexModel<Customer>(nameIndexKeys));
        _customers.Indexes.CreateOne(new CreateIndexModel<Customer>(emailIndexKeys));
    }

    public async Task<Customer?> GetByIdAsync(ObjectId id)
        => await _customers.Find(CreateActiveFilter(Builders<Customer>.Filter.Eq(c => c.Id, id))).FirstOrDefaultAsync();

    public async Task<bool> ExistsAsync(ObjectId id)
        => await _customers.Find(CreateActiveFilter(Builders<Customer>.Filter.Eq(c => c.Id, id))).AnyAsync();

    public async Task<bool> EmailExistsAsync(string email)
        => await _customers.Find(CreateActiveFilter(Builders<Customer>.Filter.Eq(c => c.Email, email))).AnyAsync();

    public async Task<IEnumerable<Customer>> GetAllAsync()
        => await _customers.Find(CreateActiveFilter()).ToListAsync();

    public async Task AddAsync(Customer customer)
        => await _customers.InsertOneAsync(customer);

    public async Task UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        await _customers.ReplaceOneAsync(c => c.Id == customer.Id, customer);
    }

    public async Task DeleteAsync(Customer customer)
    {
        customer.IsActive = false;
        await UpdateAsync(customer);

        var accounts = await _accounts.Find(a => a.CustomerId == customer.Id && a.IsActive).ToListAsync();

        if (accounts.Count > 0)
        {
            var accountUpdates = new List<WriteModel<Account>>();
            var transactionUpdates = new List<WriteModel<Transaction>>();

            foreach (var account in accounts)
            {
                var accountFilter = Builders<Account>.Filter.Eq(a => a.Id, account.Id);
                var accountUpdate = Builders<Account>.Update.Set(a => a.IsActive, false);
                accountUpdates.Add(new UpdateOneModel<Account>(accountFilter, accountUpdate));

                var transactionFilter = Builders<Transaction>.Filter.Where(t =>
                    (t.SourceAccountId == account.Id ||
                     t.DestinationAccountId == account.Id) &&
                    t.IsActive
                );

                var transactions = await _transactions.Find(transactionFilter).ToListAsync();

                foreach (var transaction in transactions)
                {
                    var sourceAccountActive = await _accounts
                        .Find(a => a.Id == transaction.SourceAccountId && a.IsActive)
                        .AnyAsync();

                    var destinationAccountActive = transaction.DestinationAccountId.HasValue &&
                                                   await _accounts
                                                       .Find(a =>
                                                           a.Id == transaction.DestinationAccountId &&
                                                           a.IsActive
                                                       ).AnyAsync();

                    if (sourceAccountActive ||
                        (transaction.DestinationAccountId.HasValue && destinationAccountActive)) continue;

                    var transactionUpdate = Builders<Transaction>.Update.Set(t => t.IsActive, false);

                    transactionUpdates.Add(new UpdateOneModel<Transaction>(
                        Builders<Transaction>.Filter.Eq(t => t.Id, transaction.Id), transactionUpdate)
                    );
                }
            }

            // Execute bulk update accounts
            if (accountUpdates.Count > 0)
            {
                await _accounts.BulkWriteAsync(accountUpdates);
            }

            // Execute bulk updates transactions
            if (transactionUpdates.Count > 0)
            {
                await _transactions.BulkWriteAsync(transactionUpdates);
            }
        }
    }

    public async Task<int> CountAsync(
        string? name,
        string? email,
        DateOnly? registeredAt
    )
    {
        var filter = CreateFilter(name, email, registeredAt);
        return (int)await _customers.CountDocumentsAsync(CreateActiveFilter(filter));
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
        return await _customers.Find(CreateActiveFilter(filter))
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    private static FilterDefinition<Customer> CreateFilter(string? name, string? email, DateOnly? registeredAt)
    {
        var builder = Builders<Customer>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(name))
        {
            filter &= builder.Regex(c => c.Name, new BsonRegularExpression($"/{name}/i"));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            filter &= builder.Regex(c => c.Email, new BsonRegularExpression($"/{email}/i"));
        }

        if (registeredAt.HasValue)
        {
            filter &= builder.Eq(c => DateOnly.FromDateTime(c.CreatedAt), registeredAt.Value);
        }

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
