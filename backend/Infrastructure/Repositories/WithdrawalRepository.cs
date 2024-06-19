using Application.Caching;
using Domain.Collections;
using Domain.Contracts;
using Domain.Enums;
using Infrastructure.Caching;
using Infrastructure.MongoContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class WithdrawalRepository(
    MongoDbContext context,
    ICacheService cacheService
) : IWithdrawalRepository
{
    private readonly IMongoCollection<Account> _accounts = context.Accounts;
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;

    public async Task<Transaction> CreateAsync(Account account, decimal amount, string description)
    {
        await _accounts.UpdateOneAsync(
            Builders<Account>.Filter.Eq(x => x.Id, account.Id),
            Builders<Account>.Update.Set(x => x.Balance, account.Balance)
        );

        var transaction = new Transaction
        {
            Type = TransactionType.Withdrawal,
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
