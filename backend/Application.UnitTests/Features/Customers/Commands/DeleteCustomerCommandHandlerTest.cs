using Application.Features.Customers.CommandHandlers;
using Application.Features.Customers.Commands;
using MongoDB.Bson;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Customers.Commands;

public class DeleteCustomerCommandHandlerTest
{
    private readonly ICustomerRepository _customerRepositoryMock = Substitute.For<ICustomerRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotfound_WhenCustomerDoesNotExist")]
    public async Task Handle_Should_ReturnsResultNotfound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = new ObjectId();
        var command = new DeleteCustomerCommand(customerId);
        _customerRepositoryMock.GetByIdAsync(command.Id).ReturnsNull();
        var handler = new DeleteCustomerCommandHandler(_customerRepositoryMock);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
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

        _customerRepositoryMock.GetByIdAsync(command.Id).Returns(customer);
        var handler = new DeleteCustomerCommandHandler(_customerRepositoryMock);

        // Act
        Result result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await _customerRepositoryMock
            .Received(1)
            .DeleteAsync(Arg.Is<ObjectId>(x => x == customerId));
    }
}
