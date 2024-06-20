using Application.Features.Auth.Contracts;
using Domain.Collections;
using Domain.Enums;
using Infrastructure.Settings;
using Infrastructure.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure.MongoContext;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly IPasswordHasher _passwordHasher;
    private const string BasePathSeeder = "Seeders";

    public MongoDbContext(IOptions<MongoDbSettings> settings, IPasswordHasher passwordHasher)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);

        ConfigureMongoDbMappings();

        _passwordHasher = passwordHasher;
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

    public void CreateIndexes()
    {
        CreateCustomerIndexes();
        CreateAccountIndexes();
        CreateTransactionIndexes();
    }

    private void CreateCustomerIndexes()
    {
        var nameIndexKeys = Builders<Customer>.IndexKeys.Ascending(c => c.Name);
        var emailIndexKeys = Builders<Customer>.IndexKeys.Ascending(c => c.Email);
        var createdAtIndexKeys = Builders<Customer>.IndexKeys.Ascending(c => c.CreatedAt);

        Customers.Indexes.CreateOne(new CreateIndexModel<Customer>(nameIndexKeys));
        Customers.Indexes.CreateOne(new CreateIndexModel<Customer>(emailIndexKeys));
        Customers.Indexes.CreateOne(new CreateIndexModel<Customer>(createdAtIndexKeys));
    }

    private void CreateAccountIndexes()
    {
        var customerIdIndexKeys = Builders<Account>.IndexKeys.Ascending(a => a.CustomerId);
        var aliasIndexKeys = Builders<Account>.IndexKeys.Ascending(a => a.Alias);
        var createdAtIndexKeys = Builders<Account>.IndexKeys.Ascending(a => a.CreatedAt);

        Accounts.Indexes.CreateOne(new CreateIndexModel<Account>(customerIdIndexKeys));
        Accounts.Indexes.CreateOne(new CreateIndexModel<Account>(aliasIndexKeys));
        Accounts.Indexes.CreateOne(new CreateIndexModel<Account>(createdAtIndexKeys));
    }

    private void CreateTransactionIndexes()
    {
        var typeIndexKeys = Builders<Transaction>.IndexKeys.Ascending(t => t.Type);
        var sourceAccountIndexKeys = Builders<Transaction>.IndexKeys.Ascending(t => t.SourceAccount);
        var destinationAccountIndexKeys = Builders<Transaction>.IndexKeys.Ascending(t => t.DestinationAccount);
        var createdAtIndexKeys = Builders<Transaction>.IndexKeys.Ascending(t => t.CreatedAt);

        Transactions.Indexes.CreateOne(new CreateIndexModel<Transaction>(typeIndexKeys));
        Transactions.Indexes.CreateOne(new CreateIndexModel<Transaction>(sourceAccountIndexKeys));
        Transactions.Indexes.CreateOne(new CreateIndexModel<Transaction>(destinationAccountIndexKeys));
        Transactions.Indexes.CreateOne(new CreateIndexModel<Transaction>(createdAtIndexKeys));
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>(User.CollectionName);
    public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>(Customer.CollectionName);
    public IMongoCollection<Account> Accounts => _database.GetCollection<Account>(Account.CollectionName);
    public IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>(Transaction.CollectionName);

    public async Task SeedCollectionsAsync()
    {
        if (await Customers.Find(FilterDefinition<Customer>.Empty).AnyAsync()) return;

        var customerSeeder = new Seeder<Customer>(Customers);
        var customerRecords = customerSeeder.GetRecordsAsync($"{BasePathSeeder}/Customers.csv");

        if (customerRecords.Count == 0) return;

        var user = new User
        {
            Id = ObjectId.GenerateNewId(),
            Email = "example@email.com",
            Password = _passwordHasher.HashPassword("password123")
        };

        await Users.InsertOneAsync(user);

        customerRecords.ForEach(c => c.Id = ObjectId.GenerateNewId());

        await customerSeeder.SeedAsync(customerRecords);

        var accounts = customerRecords
            .Select(c => new Account
            {
                Id = ObjectId.GenerateNewId(),
                Alias = "Cuenta principal",
                CustomerId = c.Id,
                CustomerName = c.Name,
                CustomerEmail = c.Email,
                Balance = 400,
                Type = AccountType.Savings
            }).ToList();

        var accountSeeder = new Seeder<Account>(Accounts);
        await accountSeeder.SeedAsync(accounts);

        if (accounts.Count == 0) return;

        List<Transaction> transactions = [];

        accounts.ForEach(account =>
        {
            transactions.Add(new Transaction
            {
                Id = ObjectId.GenerateNewId(),
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
                },
                Description = "Depósito inicial",
                Amount = 500,
                Type = TransactionType.Deposit
            });

            transactions.Add(new Transaction
            {
                Id = ObjectId.GenerateNewId(),
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
                },
                Description = "Retiro inicial",
                Amount = 100,
                Type = TransactionType.Withdrawal
            });
        });

        var transactionSeeder = new Seeder<Transaction>(Transactions);
        await transactionSeeder.SeedAsync(transactions);
    }
}
