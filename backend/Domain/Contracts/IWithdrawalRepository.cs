using Domain.Collections;

namespace Domain.Contracts;

public interface IWithdrawalRepository
{
    Task<Transaction> CreateAsync(
        Account account,
        decimal amount,
        string description
    );
}
