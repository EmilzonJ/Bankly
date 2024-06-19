using Application.Caching;
using Domain.Collections;
using Domain.Contracts;
using Domain.Enums;
using Infrastructure.Caching;
using Infrastructure.MongoContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class DepositRepository(
    MongoDbContext context,
    ICacheService cacheService
) : IDepositRepository
{
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;
    private readonly IMongoCollection<Account> _accounts = context.Accounts;

    public async Task<Transaction> CreateAsync(Account account, decimal amount, string description)
    {
        account.Balance += amount;
        await _accounts.UpdateOneAsync(
            Builders<Account>.Filter.Eq(x => x.Id, account.Id),
            Builders<Account>.Update.Set(x => x.Balance, account.Balance)
        );

        var transaction = new Transaction
        {
            Type = TransactionType.Deposit,
            Amount = amount,
            Description = description,
            SourceAccountId = account.Id,
            SourceAccount = new TransactionAccount
            {
                Id = account.Id,
                Alias = account.Alias,
                CustomerId = account.CustomerId,
                Customer = new TransactionAccountCustomer
                {
                    Id = account.CustomerId,
                    Name = account.CustomerName,
                    Email = account.CustomerEmail
                }
            }
        };

        await _transactions.InsertOneAsync(transaction);

        await cacheService.RemoveByPrefixAsync(CacheKeyPrefixes.Transaction);

        return transaction;
    }
}
