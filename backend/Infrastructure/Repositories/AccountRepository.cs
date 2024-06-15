using Domain.Collections;
using Domain.Contracts;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class AccountRepository(MongoDbContext context) : IAccountRepository
{
    private readonly IMongoCollection<Account> _accounts = context.Accounts;

    public async Task<Account> GetByIdAsync(ObjectId id)
    {
        return await _accounts.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _accounts.Find(_ => true).ToListAsync();
    }

    public async Task AddAsync(Account account)
    {
        await _accounts.InsertOneAsync(account);
    }

    public async Task UpdateAsync(Account account)
    {
        await _accounts.ReplaceOneAsync(a => a.Id == account.Id, account);
    }

    public async Task DeleteAsync(ObjectId id)
    {
        await _accounts.DeleteOneAsync(a => a.Id == id);
    }
}
