using Domain.Collections;
using Domain.Enums;
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
    }

    public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>(Customer.CollectionName);
    public IMongoCollection<Account> Accounts => _database.GetCollection<Account>(Account.CollectionName);
    public IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>(Transaction.CollectionName);
}
