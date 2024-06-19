using Domain.Collections;

namespace Domain.Contracts;

public interface ITransferRepository
{
    Task<Transaction> CreateAsync(
        Account sourceAccount,
        Account destinationAccount,
        decimal amount,
        string description
    );
}
