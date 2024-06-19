using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface ICustomerReadRepository
{
    Task<Customer?> GetByIdAsync(ObjectId id);
    Task<bool> ExistsAsync(ObjectId id);
    Task<bool> EmailExistsAsync(string email);

    Task<int> CountAsync(
        string? name,
        string? email,
        DateOnly? registeredAt
    );

    Task<IEnumerable<Customer>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        string? email,
        DateOnly? registeredAt
    );
}
