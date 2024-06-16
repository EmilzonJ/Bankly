using Domain.Collections;
using Domain.Contracts;
using Infrastructure.MongoContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CustomerRepository(MongoDbContext context) : ICustomerRepository
{
    private readonly IMongoCollection<Customer> _customers = context.Customers;
    private readonly IMongoCollection<Account> _accounts = context.Accounts;

    public async Task<Customer?> GetByIdAsync(ObjectId id)
    {
        return await _customers.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _customers.Find(c => c.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _customers.Find(c => c.Email == email).AnyAsync();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        await _customers.InsertOneAsync(customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        await _customers.ReplaceOneAsync(c => c.Id == customer.Id, customer);
    }

    public async Task DeleteAsync(ObjectId id)
    {
        await _customers.DeleteOneAsync(c => c.Id == id);
        await _accounts.DeleteManyAsync(a => a.CustomerId == id);
    }
}