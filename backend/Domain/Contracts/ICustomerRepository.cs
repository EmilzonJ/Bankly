using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(ObjectId id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(ObjectId id);
}
