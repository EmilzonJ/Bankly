using Domain.Enums;

namespace Application.Features.Transactions.Contracts;

public interface ITransactionStrategyFactory
{
    public ITransactionStrategy GetStrategy(TransactionType type);
}
