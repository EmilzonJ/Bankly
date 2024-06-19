using Application.Features.Accounts.Extensions;
using Application.Features.Accounts.Models.Responses;
using Application.Features.Accounts.Queries;
using Application.Features.Accounts.QueryHandlers;
using Domain.Enums;
using MongoDB.Bson;

namespace Application.UnitTests.Features.Accounts.Queries;

public class GetAccountTransactionsQueryHandlerTest
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenAccountDoesNotExist")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = new ObjectId();
        var query = new GetAccountTransactionsQuery(accountId);
        _accountRepository.ExistsAsync(query.Id).Returns(false);

        var handler = new GetAccountTransactionsQueryHandler(_accountRepository, _transactionRepository);

        // Act
        Result<List<AccountTransactionResponse>> result = await handler.Handle(query, default);

        // Assert
        await _transactionRepository
            .DidNotReceive()
            .GetAllByAccountAsync(Arg.Any<ObjectId>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(AccountErrors.NotFound(accountId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueListEmpty_WhenAccountExists")]
    public async Task Handle_Should_ReturnsResultValueListEmpty_WhenAccountExists()
    {
        // Arrange
        var query = new GetAccountTransactionsQuery(new ObjectId());
        _accountRepository.ExistsAsync(query.Id).Returns(true);
        _transactionRepository.GetAllByAccountAsync(query.Id).Returns([]);

        var handler = new GetAccountTransactionsQueryHandler(_accountRepository, _transactionRepository);

        // Act
        Result<List<AccountTransactionResponse>> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueList_WhenAccountExists")]
    public async Task Handle_Should_ReturnsResultValueList_WhenAccountExists()
    {
        // Arrange
        var query = new GetAccountTransactionsQuery(new ObjectId());
        var transactions = new List<Transaction>
        {
            new()
            {
                Id = new ObjectId(),
                Description = "Description",
                Amount = 100,
                Type = TransactionType.Deposit,
                SourceAccountId = ObjectId.GenerateNewId(),
                SourceAccount = new TransactionAccount
                {
                    Id = ObjectId.GenerateNewId(),
                    Alias = "Account 1",
                    CustomerId = ObjectId.GenerateNewId(),
                    Customer = new TransactionAccountCustomer
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = "Jhon Doe",
                        Email = "jhon.doe@mail.com"
                    }
                }
            },
            new()
            {
                Id = new ObjectId(),
                Description = "Description",
                Amount = 50,
                Type = TransactionType.Withdrawal,
                SourceAccountId = ObjectId.GenerateNewId(),
                SourceAccount = new TransactionAccount
                {
                    Id = ObjectId.GenerateNewId(),
                    Alias = "Account 1",
                    CustomerId = ObjectId.GenerateNewId(),
                    Customer = new TransactionAccountCustomer
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = "Jhon Doe",
                        Email = "jhon.doe@mail.com"
                    }
                }
            }
        };

        _accountRepository.ExistsAsync(query.Id).Returns(true);
        _transactionRepository.GetAllByAccountAsync(query.Id).Returns(transactions);

        var handler = new GetAccountTransactionsQueryHandler(_accountRepository, _transactionRepository);

        // Act
        Result<List<AccountTransactionResponse>> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(transactions.Select(x => x.ToResponse()));
        result.Value.Should().HaveCount(2);
        result.Value.Should().ContainSingle(x => x.Amount == 100);
        result.Value.Should().ContainSingle(x => x.Amount == 50);
        result.Value.Should().ContainSingle(x => x.Type == TransactionType.Deposit);
        result.Value.Should().ContainSingle(x => x.Type == TransactionType.Withdrawal);
    }
}
