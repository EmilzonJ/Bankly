using Domain.Collections;
using MongoDB.Bson;

namespace Domain.Contracts;

public interface ICustomerWriteRepository
{
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
    Task DeleteRelatedDataAsync(ObjectId customerId);
}
