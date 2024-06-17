using Domain.Collections;
using Domain.Contracts;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class TransactionRepository(MongoDbContext context) : ITransactionRepository
{
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;

    public async Task<Transaction> GetByIdAsync(ObjectId id)
    {
        return await _transactions.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _transactions.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllByAccountAsync(ObjectId accountId)
        => await _transactions
            .Find(t => t.SourceAccountId == accountId || t.DestinationAccountId == accountId)
            .ToListAsync();

    public async Task AddAsync(Transaction transaction)
    {
        await _transactions.InsertOneAsync(transaction);
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        await _transactions.ReplaceOneAsync(t => t.Id == transaction.Id, transaction);
    }

    public async Task DeleteAsync(ObjectId id)
    {
        await _transactions.DeleteOneAsync(t => t.Id == id);
    }
}
