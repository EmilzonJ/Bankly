using Application.Features.Transactions.Commands;
using Application.Features.Transactions.Models.Requests;
using Application.Features.Transactions.CommandHandlers;
using Application.Features.Transactions.Contracts;
using Domain.Enums;
using MongoDB.Bson;

namespace Application.UnitTests.Features.Transactions.Commands
{
    public class CreateTransactionCommandHandlerTest
    {
        private readonly ITransactionStrategyFactory _strategyFactory = Substitute.For<ITransactionStrategyFactory>();
        private readonly ITransactionStrategy _strategy = Substitute.For<ITransactionStrategy>();

        [Fact(DisplayName = "Handle_Should_ReturnsResultFailure_WhenDescriptionIsInvalid")]
        public async Task Handle_Should_ReturnsResultFailure_WhenDescriptionIsInvalid()
        {
            // Arrange
            var command = new CreateTransactionCommand(
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId(),
                100,
                string.Empty,
                TransactionType.OutgoingTransfer
            );

            var handler = new CreateTransactionCommandHandler(_strategyFactory);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TransactionErrors.InvalidDescription);
        }

        [Fact(DisplayName = "Handle_Should_ReturnsResultFailure_WhenAmountIsInvalid")]
        public async Task Handle_Should_ReturnsResultFailure_WhenAmountIsInvalid()
        {
            // Arrange
            var command = new CreateTransactionCommand(
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId(),
                0,
                "Description",
                TransactionType.OutgoingTransfer
            );
            var handler = new CreateTransactionCommandHandler(_strategyFactory);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TransactionErrors.InvalidAmount(command.Amount));
        }

        [Fact]
        public async Task Handle_Should_ReturnsResultSucsess_WithValidCommandAndSuccessfulStrategy()
        {
            var command = new CreateTransactionCommand(
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId(),
                100,
                "Description",
                TransactionType.OutgoingTransfer
            );

            var transaction = new Transaction
            {
                Id = ObjectId.GenerateNewId(),
                Description = command.Description,
                SourceAccountId = command.SourceAccountId,
                SourceAccount = new TransactionAccount
                {
                    Id = ObjectId.GenerateNewId(),
                    Alias = "Test",
                    CustomerId = ObjectId.GenerateNewId(),
                    Customer = new TransactionAccountCustomer
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = "Jhon Doe",
                        Email = "jhon.doe@mail.com",
                    }
                },
                DestinationAccountId = command.DestinationAccountId,
                Amount = command.Amount,
                Type = command.Type,
            };

            _strategyFactory.GetStrategy(command.Type).Returns(_strategy);
            _strategy.ExecuteAsync(Arg.Any<TransactionCreate>())
                .Returns(transaction);

            var handler = new CreateTransactionCommandHandler(_strategyFactory);

            // Act
            var result = await handler.Handle(command, default);


            result.IsSuccess.Should().BeTrue();
        }
    }
}
