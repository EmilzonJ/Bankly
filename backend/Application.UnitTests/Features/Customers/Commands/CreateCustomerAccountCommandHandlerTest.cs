using Application.Features.Customers.CommandHandlers;
using Application.Features.Customers.Commands;
using MongoDB.Bson;

namespace Application.UnitTests.Features.Customers.Commands;

public class CreateCustomerAccountCommandHandlerTest
{
    private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenCustomerDoesntExists")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenCustomerDoesntExists()
    {
        // Arrange
        var customerId = new ObjectId();
        var command = new CreateCustomerAccountCommand(customerId, 0);
        var handler = new CreateCustomerAccountCommandHandler(_customerRepository, _accountRepository);

        _customerRepository.ExistsAsync(Arg.Any<ObjectId>()).Returns(false);

        // Act
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        await _accountRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<Account>());

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(CustomerErrors.NotFound(customerId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultConflict_WhenBalanceIsNegative")]
    public async Task Handle_Should_ReturnsResultConflict_WhenBalanceIsNegative()
    {
        // Arrange
        const int invalidBalance = -1;
        var customerId = new ObjectId();
        var command = new CreateCustomerAccountCommand(customerId, invalidBalance);
        var handler = new CreateCustomerAccountCommandHandler(_customerRepository, _accountRepository);

        _customerRepository.ExistsAsync(Arg.Any<ObjectId>()).Returns(true);

        // Act
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        await _accountRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<Account>());

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(AccountErrors.NegativeBalance(invalidBalance));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueId_WhenAccountIsValid")]
    public async Task Handle_Should_ReturnsResultValueId_WhenAccountIsValid()
    {
        // Arrange
        var customerId = new ObjectId();
        var command = new CreateCustomerAccountCommand(customerId, 0);
        var handler = new CreateCustomerAccountCommandHandler(_customerRepository, _accountRepository);

        _customerRepository.ExistsAsync(Arg.Any<ObjectId>()).Returns(true);

        // Act
        Result<string> result = await handler.Handle(command, default);

        // Assert
        await _accountRepository
            .Received(1)
            .AddAsync(Arg.Any<Account>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
    }
}
