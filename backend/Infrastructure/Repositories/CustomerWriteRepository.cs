using Application.Caching;
using Domain.Collections;
using Domain.Contracts;
using Infrastructure.Caching;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CustomerWriteRepository(
    MongoDbContext context,
    ICacheService cacheService
) : ICustomerWriteRepository
{
    private readonly IMongoCollection<Customer> _customers = context.Customers;
    private readonly IMongoCollection<Account> _accounts = context.Accounts;
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;

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
    }

    public async Task DeleteRelatedDataAsync(ObjectId customerId)
    {
        var accounts = await _accounts.Find(a => a.CustomerId == customerId && a.IsActive).ToListAsync();

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

            // Invalidate cache
            await cacheService.RemoveByPrefixAsync(CacheKeyPrefixes.Transaction);
        }
    }
}
