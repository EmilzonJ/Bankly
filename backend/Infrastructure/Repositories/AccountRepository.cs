using Domain.Collections;
using Domain.Contracts;
using Domain.Enums;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class AccountRepository(MongoDbContext context) : IAccountRepository
{
    private readonly IMongoCollection<Account> _accounts = context.Accounts;
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;

    public async Task<Account?> GetByIdAsync(ObjectId id)
        => await _accounts.Find(CreateActiveFilter(Builders<Account>.Filter.Eq(a => a.Id, id))).FirstOrDefaultAsync();

    public async Task<IEnumerable<Account>> GetAllAsync()
        => await _accounts.Find(CreateActiveFilter()).ToListAsync();

    public async Task<IEnumerable<Account>> GetAllByCustomerAsync(ObjectId customerId)
        => await _accounts.Find(CreateActiveFilter(Builders<Account>.Filter.Eq(a => a.CustomerId, customerId))).ToListAsync();

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
}

