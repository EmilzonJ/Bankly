using Domain.Collections;
using Domain.Enums;
using Infrastructure.Base;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure.MongoContext;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private const string BasePathSeeder = "Seeders";

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);

        ConfigureMongoDbMappings();
    }

    private static void ConfigureMongoDbMappings()
    {
        BsonClassMap.RegisterClassMap<Transaction>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.Type)
                .SetSerializer(new EnumSerializer<TransactionType>(BsonType.String));
        });

        BsonClassMap.RegisterClassMap<Account>(acc =>
        {
            acc.AutoMap();
            acc.GetMemberMap(c => c.Type)
                .SetSerializer(new EnumSerializer<AccountType>(BsonType.String));
        });
    }

    public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>(Customer.CollectionName);
    public IMongoCollection<Account> Accounts => _database.GetCollection<Account>(Account.CollectionName);

    public IMongoCollection<Transaction> Transactions =>
        _database.GetCollection<Transaction>(Transaction.CollectionName);

    public async Task SeedCollectionsAsync()
    {
        if(await Customers.Find(FilterDefinition<Customer>.Empty).AnyAsync()) return;

        var customerSeeder = new Seeder<Customer>(Customers);
        var customerRecords = customerSeeder.GetRecordsAsync($"{BasePathSeeder}/Customers.csv");

        if (customerRecords.Count == 0) return;

        customerRecords.ForEach(c => c.Id = ObjectId.GenerateNewId());

        await customerSeeder.SeedAsync(customerRecords);

        var accounts = customerRecords
            .Select(c => new Account
            {
                Id = ObjectId.GenerateNewId(),
                CustomerId = c.Id,
                Balance = 500,
                Type = AccountType.Savings
            }).ToList();

        var accountSeeder = new Seeder<Account>(Accounts);
        await accountSeeder.SeedAsync(accounts);

        if(accounts.Count == 0) return;

        List<Transaction> transactions = [];

        accounts.ForEach(account =>
        {
            transactions.Add(new Transaction
            {
                Id = ObjectId.GenerateNewId(),
                SourceAccountId = account.Id,
                Description = "Depósito inicial",
                Amount = 500,
                Type = TransactionType.Deposit
            });

            transactions.Add(new Transaction
            {
                Id = ObjectId.GenerateNewId(),
                SourceAccountId = account.Id,
                Description = "Retiro inicial",
                Amount = 100,
                Type = TransactionType.Withdrawal
            });
        });

        var transactionSeeder = new Seeder<Transaction>(Transactions);
        await transactionSeeder.SeedAsync(transactions);
    }
}
