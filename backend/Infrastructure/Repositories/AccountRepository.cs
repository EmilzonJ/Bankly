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

    public async Task<Account> GetByIdAsync(ObjectId id)
        => await _accounts.Find(a => a.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Account>> GetAllAsync()
        => await _accounts.Find(_ => true).ToListAsync();

    public async Task<IEnumerable<Account>> GetAllByCustomerAsync(ObjectId customerId)
        => await _accounts.Find(a => a.CustomerId == customerId).ToListAsync();

    public async Task AddAsync(Account account)
    {
        account.Type = AccountType.Savings;
        account.CreatedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;

        await _accounts.InsertOneAsync(account);
    }

    public async Task UpdateAsync(Account account)
        => await _accounts.ReplaceOneAsync(a => a.Id == account.Id, account);

    public async Task DeleteAsync(Account account)
    {
        account.IsActive = false;
        await UpdateAsync(account);

        // Mark all transactions as inactive
        var filter = Builders<Transaction>.Filter.Eq(t => t.SourceAccountId, account.Id) &
                     Builders<Transaction>.Filter.Eq(t => t.IsActive, true);
        var update = Builders<Transaction>.Update.Set(t => t.IsActive, false);
        await _transactions.UpdateManyAsync(filter, update);
    }
}
