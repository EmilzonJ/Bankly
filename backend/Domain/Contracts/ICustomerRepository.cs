using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(ObjectId id);
    Task<bool> EmailExistsAsync(string email);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(ObjectId id);

    Task<int> CountAsync(
        string? name,
        string? email,
        DateTime? registeredAt
    );

    Task<IEnumerable<Customer>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        string? email,
        DateTime? registeredAt
    );
}
