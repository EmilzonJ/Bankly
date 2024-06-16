using Application.Features.Customers.CommandHandlers;
using Application.Features.Customers.Commands;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Customers.Commands;

public class UpdateCustomerCommandHandlerTest
{
    private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenCustomerDoesntExists")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenCustomerDoesntExists()
    {
        // Arrange
        var customerId = new ObjectId();

        var handler = new UpdateCustomerCommandHandler(_customerRepository);

        var command = new UpdateCustomerCommand(
            customerId,
            "John Doe Updated",
            "jhon.due.updated@mail.com"
        );

        _customerRepository.GetByIdAsync(customerId).ReturnsNull();

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CustomerErrors.NotFound(customerId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultConflict_WhenEmailAlreadyExists")]
    public async Task Handle_Should_ReturnsResultConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        var customerId = new ObjectId();
        var customer = new Customer
        {
            Id = customerId,
            Name = "Jhon Doe",
            Email = "jhon.doe@mail.com"
        };

        var command = new UpdateCustomerCommand(
            customerId,
            "John Doe Updated",
            "jhon.updated@mail"
        );

        var handler = new UpdateCustomerCommandHandler(_customerRepository);

        _customerRepository.GetByIdAsync(customerId).Returns(customer);

        _customerRepository.EmailExistsAsync(command.Email).Returns(true);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CustomerErrors.EmailTaken(command.Email));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultSuccess_WhenCustomerUpdated")]
    public async Task Handle_Should_ReturnsResultSuccess_WhenCustomerUpdated()
    {
        // Arrange
        var customerId = new ObjectId();
        var customer = new Customer
        {
            Id = customerId,
            Name = "Jhon Doe",
            Email = "jhon.doe@mail.com"
        };

        var command = new UpdateCustomerCommand(
            customerId,
            "John Doe Updated",
            "jhon.updated@mail"
        );

        var handler = new UpdateCustomerCommandHandler(_customerRepository);

        _customerRepository.GetByIdAsync(customerId).Returns(customer);

        _customerRepository.EmailExistsAsync(command.Email).Returns(false);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await _customerRepository
            .Received(1)
            .UpdateAsync(Arg.Is<Customer>(x =>
                x.Id == customerId &&
                x.Name == command.Name &&
                x.Email == command.Email
            ));
    }
}
