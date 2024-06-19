using Application.Features.Transactions.Extensions;
using Application.Features.Transactions.Models.Responses;
using Application.Features.Transactions.Queries;
using Application.Features.Transactions.QueryHandlers;
using Application.Shared;
using MongoDB.Bson;
using Domain.Enums;

namespace Application.UnitTests.Features.Transactions.Queries
{
    public class GetTransactionsQueryHandlerTest
    {
        private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();

        [Fact(DisplayName = "Handle_Should_ReturnsResultValueList_WhenTransactionsExists")]
        public async Task Handle_Should_ReturnsResultValueList_WhenTransactionsExists()
        {
            // Arrange
            List<Transaction> transactions =
            [
                new Transaction
                {
                    Id = ObjectId.GenerateNewId(), Description = "Transaction 1", Amount = 1000,
                    Type = TransactionType.Deposit
                },
                new Transaction
                {
                    Id = ObjectId.GenerateNewId(), Description = "Transaction 2", Amount = 2000,
                    Type = TransactionType.Withdrawal
                }
            ];

            _transactionRepository.GetPagedAsync(1, 10, null, null, null, null).Returns(transactions);
            _transactionRepository.CountAsync(null, null, null, null).Returns(2);

            var query = new GetTransactionsQuery(1, 10, null);
            var handler = new GetTransactionsQueryHandler(_transactionRepository);

            // Act
            Result<PaginatedList<TransactionResponse>> result = await handler.Handle(query, default);

            // Assert
            await _transactionRepository
                .Received(1)
                .GetPagedAsync(1, 10, null, null, null, null);

            await _transactionRepository
                .Received(1)
                .CountAsync(null, null, null, null);

            result.Value.Items.Should().BeEquivalentTo(transactions.Select(x => x.ToResponse()));
            result.Value.Items.Should().HaveCount(2);
        }

        [Fact(DisplayName = "Handle_Should_ReturnsResultValueEmptyList_WhenTransactionsDoesntExists")]
        public async Task Handle_Should_ReturnsResultValueEmptyList_WhenTransactionsDoesntExists()
        {
            // Arrange
            var transactions = Array.Empty<Transaction>().ToList();

            _transactionRepository.GetPagedAsync(1, 10, null, null, null, null).Returns(transactions);
            _transactionRepository.CountAsync(null, null, null, null).Returns(0);

            var query = new GetTransactionsQuery(1, 10, null);
            var handler = new GetTransactionsQueryHandler(_transactionRepository);

            // Act
            Result<PaginatedList<TransactionResponse>> result = await handler.Handle(query, default);

            // Assert
            await _transactionRepository
                .Received(1)
                .GetPagedAsync(1, 10, null, null, null, null);

            await _transactionRepository
                .Received(1)
                .CountAsync(null, null, null, null);

            result.Value.Items.Should().BeEquivalentTo(transactions.Select(x => x.ToResponse()));
            result.Value.Items.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Handle_Should_ReturnsFilteredResultValuePaginatedList_WhenFiltersAreApplied")]
        public async Task Handle_Should_ReturnsFilteredResultValuePaginatedList_WhenFiltersAreApplied()
        {
            // Arrange
            List<Transaction> transactions =
            [
                new Transaction
                {
                    Id = new ObjectId(),
                    Description = "Transaction 1",
                    Amount = 1000,
                    Type = TransactionType.Deposit
                }
            ];

            _transactionRepository.GetPagedAsync(1, 10, null, null, "Transaction 1", null).Returns(transactions);
            _transactionRepository.CountAsync(null, "Transaction 1", null, null).Returns(1);

            var query = new GetTransactionsQuery(1, 10, null, null, "Transaction 1");
            var handler = new GetTransactionsQueryHandler(_transactionRepository);

            // Act
            Result<PaginatedList<TransactionResponse>> result = await handler.Handle(query, default);

            // Assert
            await _transactionRepository
                .Received(1)
                .GetPagedAsync(1, 10, null, null, "Transaction 1", null);

            await _transactionRepository
                .Received(1)
                .CountAsync(null, null, "Transaction 1", null);

            result.Value.Items.Should().BeEquivalentTo(transactions.Select(x => x.ToResponse()));
            result.Value.Items.Should().HaveCount(1);
        }
    }
}
