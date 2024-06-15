using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(ObjectId id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(ObjectId id);
}
