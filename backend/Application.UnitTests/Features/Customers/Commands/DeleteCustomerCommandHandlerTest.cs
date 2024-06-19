using Application.Features.Customers.CommandHandlers;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Events;
using Application.Messaging;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Customers.Commands;

public class DeleteCustomerCommandHandlerTest
{
    private readonly ICustomerReadRepository _customerReadRepository = Substitute.For<ICustomerReadRepository>();
    private readonly ICustomerWriteRepository _customerWriteRepository = Substitute.For<ICustomerWriteRepository>();
    private readonly IMessagePublisher _messagePublisher = Substitute.For<IMessagePublisher>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotfound_WhenCustomerDoesNotExist")]
    public async Task Handle_Should_ReturnsResultNotfound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = new ObjectId();
        var command = new DeleteCustomerCommand(customerId);
        _customerReadRepository.GetByIdAsync(command.Id).ReturnsNull();
        var handler = new DeleteCustomerCommandHandler(_customerReadRepository, _customerWriteRepository, _messagePublisher);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        await _messagePublisher
            .DidNotReceive()
            .Publish(Arg.Any<CustomerDeletedEvent>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CustomerErrors.NotFound(customerId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultSuccess_WhenCustomerExists")]
    public async Task Handle_Should_ReturnsResultSuccess_WhenCustomerExists()
    {
        // Arrange
        var customerId = new ObjectId();
        var command = new DeleteCustomerCommand(customerId);
        var customer = new Customer
        {
            Id = customerId,
            Name = "John Doe",
            Email = "jhon.doe@mail.com"
        };

        _customerReadRepository.GetByIdAsync(command.Id).Returns(customer);
        var handler = new DeleteCustomerCommandHandler(_customerReadRepository, _customerWriteRepository, _messagePublisher);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await _messagePublisher
            .Received(1)
            .Publish(Arg.Any<CustomerDeletedEvent>());

        await _customerWriteRepository
            .Received(1)
            .DeleteAsync(Arg.Is<Customer>(x => x == customer));
    }
}
