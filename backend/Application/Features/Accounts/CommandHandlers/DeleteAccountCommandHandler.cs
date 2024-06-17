using Application.Features.Accounts.Commands;

namespace Application.Features.Accounts.CommandHandlers;

public record DeleteAccountCommandHandler(
    IAccountRepository AccountRepository
) : ICommandHandler<DeleteAccountCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await AccountRepository.GetByIdAsync(command.Id);

        if(account is null) return Result.Failure(AccountErrors.NotFound(command.Id));

        await AccountRepository.DeleteAsync(account);

        return Result.Success();
    }
}
