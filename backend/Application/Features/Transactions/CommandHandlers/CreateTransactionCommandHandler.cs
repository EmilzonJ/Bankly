using Application.Features.Transactions.Commands;
using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Models.Requests;

namespace Application.Features.Transactions.CommandHandlers;

public record CreateTransactionCommandHandler(
    ITransactionStrategyFactory StrategyFactory
) : ICommandHandler<CreateTransactionCommand, Result<string>>
{
    public async ValueTask<Result<string>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(command.Description))
            return Result.Failure<string>(TransactionErrors.InvalidDescription);

        if (command.Amount <= 0)
            return Result.Failure<string>(TransactionErrors.InvalidAmount(command.Amount));

        var strategy = StrategyFactory.GetStrategy(command.Type);

        var transactionCreate = new TransactionCreate
        {
            SourceAccountId = command.SourceAccountId,
            DestinationAccountId = command.DestinationAccountId,
            Amount = command.Amount,
            Description = command.Description,
            Type = command.Type
        };

        var transactionResult = await strategy.ExecuteAsync(transactionCreate);

        return transactionResult.IsFailure
            ? Result.Failure<string>(transactionResult.Error)
            : transactionResult.Value.Id.ToString();
    }
}
