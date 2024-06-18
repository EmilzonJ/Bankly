using Application.Features.Accounts.CommandHandlers;
using Application.Features.Accounts.Commands;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Accounts.Commands;

public class DeleteAccountCommandHandlerTest
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenAccountDoesntExists")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenAccountDoesntExists()
    {
        // Arrange
        var accountId = new ObjectId();
        var command = new DeleteAccountCommand(accountId);

        _accountRepository.GetByIdAsync(command.Id).ReturnsNull();

        var handler = new DeleteAccountCommandHandler(_accountRepository);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _accountRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<Account>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(AccountErrors.NotFound(accountId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultSuccess_WhenAccountExists")]
    public async Task Handle_Should_ReturnsResultSuccess_WhenAccountExists()
    {
        // Arrange
        var accountId = new ObjectId();
        var command = new DeleteAccountCommand(accountId);

        _accountRepository.GetByIdAsync(command.Id).Returns(new Account
        {
            Alias = "Alias", CustomerName = "Jhon Doe"
        });

        var handler = new DeleteAccountCommandHandler(_accountRepository);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _accountRepository
            .Received(1)
            .DeleteAsync(Arg.Any<Account>());

        result.IsSuccess.Should().BeTrue();
    }
}
