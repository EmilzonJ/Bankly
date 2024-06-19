using Domain.Collections;

namespace Domain.Contracts;

public interface IDepositRepository
{
    Task<Transaction> CreateAsync(
        Account account,
        decimal amount,
        string description
    );
}
