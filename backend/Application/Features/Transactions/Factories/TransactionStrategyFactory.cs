using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Strategies;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Features.Transactions.Factories;

public class TransactionStrategyFactory(IServiceProvider serviceProvider) : ITransactionStrategyFactory
{
    public ITransactionStrategy GetStrategy(TransactionType type)
    {
        return type switch
        {
            TransactionType.Deposit => serviceProvider.GetRequiredService<DepositStrategy>(),
            TransactionType.Withdrawal => serviceProvider.GetRequiredService<WithdrawalStrategy>(),
            TransactionType.OutgoingTransfer => serviceProvider.GetRequiredService<TransferStrategy>(),
            TransactionType.IncomingTransfer => serviceProvider.GetRequiredService<TransferStrategy>(),
            _ => throw new ArgumentException("Invalid transaction type", nameof(type)),
        };
    }
}
