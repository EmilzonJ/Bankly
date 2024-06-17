using Application.Features.Accounts.Extensions;
using Application.Features.Accounts.Models.Responses;
using Application.Features.Accounts.Queries;
using Application.Features.Accounts.QueryHandlers;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Accounts.Queries;

public class GetAccountByIdQueryHandlerTest
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenAccountDoesNotExist")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = new ObjectId();
        var query = new GetAccountByIdQuery(accountId);

        _accountRepository.GetByIdAsync(query.Id).ReturnsNull();

        var handler = new GetAccountByIdQueryHandler(_accountRepository);

        // Act
        Result<AccountResponse> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(AccountErrors.NotFound(accountId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsAccount_WhenAccountExists")]
    public async Task Handle_Should_ReturnsAccount_WhenAccountExists()
    {
        // Arrange
        var accountId = new ObjectId();
        var query = new GetAccountByIdQuery(accountId);
        var account = new Account
        {
            Id = accountId,
            CustomerId = new ObjectId(),
            Balance = 1000
        };

        _accountRepository.GetByIdAsync(query.Id).Returns(account);

        var handler = new GetAccountByIdQueryHandler(_accountRepository);

        // Act
        Result<AccountResponse> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(account.ToResponse());
    }
}
