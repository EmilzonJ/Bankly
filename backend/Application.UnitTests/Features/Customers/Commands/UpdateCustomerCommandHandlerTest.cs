using Application.Features.Customers.CommandHandlers;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Events;
using Application.Messaging;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Customers.Commands;

public class UpdateCustomerCommandHandlerTest
{
    private readonly ICustomerReadRepository _customerReadRepository = Substitute.For<ICustomerReadRepository>();
    private readonly ICustomerWriteRepository _customerWriteRepository = Substitute.For<ICustomerWriteRepository>();
    private readonly IMessagePublisher _messagePublisher = Substitute.For<IMessagePublisher>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenCustomerDoesntExists")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenCustomerDoesntExists()
    {
        // Arrange
        var customerId = new ObjectId();

        var handler = new UpdateCustomerCommandHandler(_customerWriteRepository, _customerReadRepository, _messagePublisher);

        var command = new UpdateCustomerCommand(
            customerId,
            "John Doe Updated",
            "jhon.due.updated@mail.com"
        );

        _customerReadRepository.GetByIdAsync(customerId).ReturnsNull();

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _messagePublisher
            .DidNotReceive()
            .Publish(Arg.Any<CustomerNameUpdatedEvent>());

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

        var handler = new UpdateCustomerCommandHandler(_customerWriteRepository, _customerReadRepository, _messagePublisher);

        _customerReadRepository.GetByIdAsync(customerId).Returns(customer);

        _customerReadRepository.EmailExistsAsync(command.Email).Returns(true);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _messagePublisher
            .DidNotReceive()
            .Publish(Arg.Any<CustomerNameUpdatedEvent>());

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

        var handler = new UpdateCustomerCommandHandler(_customerWriteRepository, _customerReadRepository, _messagePublisher);

        _customerReadRepository.GetByIdAsync(customerId).Returns(customer);

        _customerReadRepository.EmailExistsAsync(command.Email).Returns(false);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _customerWriteRepository
            .Received(1)
            .UpdateAsync(Arg.Is<Customer>(x =>
                x.Id == customerId &&
                x.Name == command.Name &&
                x.Email == command.Email
            ));

        await _messagePublisher
            .Received(1)
            .Publish(Arg.Is<CustomerNameUpdatedEvent>(x =>
                x.CustomerId == customerId.ToString() &&
                x.NewName == command.Name
            ));

        await _messagePublisher
            .Received(1)
            .Publish(Arg.Is<CustomerEmailUpdatedEvent>(x =>
                x.CustomerId == customerId.ToString() &&
                x.NewEmail == command.Email
            ));

        result.IsSuccess.Should().BeTrue();
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultSuccessAndDontPublishEvent_WhenNameIsTheSame")]
    public async Task Handle_Should_ReturnsResultSuccessAndDontPublishEvent_WhenNameIsTheSame()
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
            "Jhon Doe",
            "jhon.updated@mail"
        );

        var handler = new UpdateCustomerCommandHandler(_customerWriteRepository, _customerReadRepository, _messagePublisher);

        _customerReadRepository.GetByIdAsync(customerId).Returns(customer);
        _customerReadRepository.EmailExistsAsync(command.Email).Returns(false);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _customerWriteRepository
            .Received(1)
            .UpdateAsync(Arg.Is<Customer>(x =>
                x.Id == customerId &&
                x.Name == command.Name &&
                x.Email == command.Email
            ));

        await _messagePublisher
            .DidNotReceive()
            .Publish(Arg.Any<CustomerNameUpdatedEvent>());

        result.IsSuccess.Should().BeTrue();
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultSuccessAndDontPublishEvent_WhenEmailIsTheSame")]
    public async Task Handle_Should_ReturnsResultSuccessAndDontPublishEvent_WhenEmailIsTheSame()
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
            "Jhon Doe",
            "jhon.updated@mail"
        );

        var handler = new UpdateCustomerCommandHandler(_customerWriteRepository, _customerReadRepository, _messagePublisher);

        _customerReadRepository.GetByIdAsync(customerId).Returns(customer);
        _customerReadRepository.EmailExistsAsync(command.Email).Returns(false);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _customerWriteRepository
            .Received(1)
            .UpdateAsync(Arg.Is<Customer>(x =>
                x.Id == customerId &&
                x.Name == command.Name &&
                x.Email == command.Email
            ));

        await _messagePublisher
            .DidNotReceive()
            .Publish(Arg.Any<CustomerNameUpdatedEvent>());

        result.IsSuccess.Should().BeTrue();
    }
}
