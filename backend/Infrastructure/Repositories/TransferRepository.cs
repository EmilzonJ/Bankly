using Application.Caching;
using Domain.Collections;
using Domain.Contracts;
using Domain.Enums;
using Infrastructure.Caching;
using Infrastructure.MongoContext;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class TransferRepository(
    MongoDbContext context,
    ICacheService cacheService
) : ITransferRepository
{
    private readonly IMongoCollection<Transaction> _transactions = context.Transactions;
    private readonly IMongoCollection<Account> _accounts = context.Accounts;

    public async Task<Transaction> CreateAsync(
        Account sourceAccount,
        Account destinationAccount,
        decimal amount,
        string description
    )
    {
        sourceAccount.Balance -= amount;
        destinationAccount.Balance += amount;

        var sourceAccountUpdate = Builders<Account>.Update
            .Set(x => x.Balance, sourceAccount.Balance);

        var destinationAccountUpdate = Builders<Account>.Update
            .Set(x => x.Balance, destinationAccount.Balance);

        await _accounts.BulkWriteAsync(new[]
        {
            new UpdateOneModel<Account>(
                Builders<Account>.Filter.Eq(x => x.Id, sourceAccount.Id),
                sourceAccountUpdate
            ),
            new UpdateOneModel<Account>(
                Builders<Account>.Filter.Eq(x => x.Id, destinationAccount.Id),
                destinationAccountUpdate
            )
        });

        var sourceTransaction = GetTransactionInstance(
            TransactionType.OutgoingTransfer,
            sourceAccount: sourceAccount,
            destinationAccount: destinationAccount,
            amount,
            description
        );

        var destinationTransaction = GetTransactionInstance(
            TransactionType.IncomingTransfer,
            sourceAccount: destinationAccount,
            destinationAccount: sourceAccount,
            amount,
            description
        );

        await _transactions.BulkWriteAsync(new[]
        {
            new InsertOneModel<Transaction>(sourceTransaction),
            new InsertOneModel<Transaction>(destinationTransaction)
        });


        await cacheService.RemoveByPrefixAsync(CacheKeyPrefixes.Transaction);

        return sourceTransaction;
    }

    private static Transaction GetTransactionInstance(
        TransactionType type,
        Account sourceAccount,
        Account destinationAccount,
        decimal amount,
        string description
    )
    {
        return new Transaction
        {
            Type = type,
            Amount = amount,
            Description = description,
            SourceAccountId = sourceAccount.Id,
            SourceAccount = new TransactionAccount
            {
                Id = sourceAccount.Id,
                Alias = sourceAccount.Alias,
                CustomerId = sourceAccount.CustomerId,
                Customer = new TransactionAccountCustomer
                {
                    Id = sourceAccount.CustomerId,
                    Name = sourceAccount.CustomerName,
                    Email = sourceAccount.CustomerEmail
                }
            },
            DestinationAccountId = destinationAccount.Id,
            DestinationAccount = new TransactionAccount
            {
                Id = destinationAccount.Id,
                Alias = destinationAccount.Alias,
                CustomerId = destinationAccount.CustomerId,
                Customer = new TransactionAccountCustomer
                {
                    Id = destinationAccount.CustomerId,
                    Name = destinationAccount.CustomerName,
                    Email = destinationAccount.CustomerEmail
                }
            }
        };
    }
}
