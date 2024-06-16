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
    private readonly IMongoCollection<AccountSequence> _accountSequence = context.AccountSequences;

    public async Task<Account> GetByIdAsync(ObjectId id)
        => await _accounts.Find(a => a.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Account>> GetAllAsync()
        => await _accounts.Find(_ => true).ToListAsync();

    public async Task<IEnumerable<Account>> GetAllByCustomerAsync(ObjectId customerId)
        => await _accounts.Find(a => a.CustomerId == customerId).ToListAsync();

    public async Task AddAsync(Account account)
    {
        account.Number = await GenerateAccountNumber();
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

    /// <summary>
    /// Generate a unique account number by combining the current timestamp and a sequence number
    /// stored in the database and incremented when a new Account is created
    /// </summary>
    /// <returns>Next Unique Account Number</returns>
    /// <exception cref="Exception"></exception>
    private async Task<double> GenerateAccountNumber()
    {
        var filter = Builders<AccountSequence>.Filter.Eq(c => c.Id, AccountSequence.SequenceId);
        var update = Builders<AccountSequence>.Update.Inc(c => c.Seq, 1);
        var options = new FindOneAndUpdateOptions<AccountSequence>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };

        var sequenceDocument = await _accountSequence.FindOneAndUpdateAsync(filter, update, options);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var sequence = sequenceDocument.Seq.ToString("D4");

        return double.TryParse($"{timestamp}{sequence}", out var number)
            ? number
            : throw new Exception("Error generating account number");
    }
}
