using Application.Features.Transactions.Extensions;
using Application.Features.Transactions.Models.Responses;
using Application.Features.Transactions.Queries;
using Application.Features.Transactions.QueryHandlers;
using Domain.Enums;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Transactions.Queries;

public class GetTransactionByIdQueryHandlerTest
{
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenTransactionDoesntExists")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenTransactionDoesntExists()
    {
        // Arrange
        var transactionId = new ObjectId();
        _transactionRepository.GetByIdAsync(transactionId).ReturnsNull();

        var query = new GetTransactionByIdQuery(transactionId);
        var handler = new GetTransactionByIdQueryHandler(_transactionRepository);

        // Act
        Result<TransactionDetailResponse> result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(TransactionErrors.NotFound(transactionId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValue_WhenTransactionExists")]
    public async Task Handle_Should_ReturnsResultValue_WhenTransactionExists()
    {
        // Arrange
        var transactionId = new ObjectId();
        var transaction = new Transaction
        {
            Id = new ObjectId(),
            Description = "Transaction 1",
            Amount = 1000,
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
        };

        _transactionRepository.GetByIdAsync(transactionId).Returns(transaction);

        var query = new GetTransactionByIdQuery(transactionId);
        var handler = new GetTransactionByIdQueryHandler(_transactionRepository);

        // Act
        Result<TransactionDetailResponse> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(transaction.ToResponse());
    }
}
