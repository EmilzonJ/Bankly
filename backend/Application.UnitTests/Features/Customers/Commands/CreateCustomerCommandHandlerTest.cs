using Application.Features.Customers.CommandHandlers;
using Application.Features.Customers.Commands;

namespace Application.UnitTests.Features.Customers.Commands;

public class CreateCustomerCommandHandlerTest
{
    private readonly ICustomerRepository _customerRepositoryMock = Substitute.For<ICustomerRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsConflictResult_WhenEmailIsAlreadyTaken")]
    public async Task Handle_Should_ReturnsConflictResult_WhenEmailIsAlreadyTaken()
    {
        // Arrange
        const string email = "jhon.doe@email.com";
        var command = new CreateCustomerCommand("Jhon Doe", email);

        _customerRepositoryMock.EmailExistsAsync(Arg.Any<string>()).Returns(true);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        await _customerRepositoryMock.Received(0).AddAsync(Arg.Any<Customer>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomerErrors.EmailTaken(email));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueId_WhenCustomerIsValid")]
    public async Task Handle_Should_ReturnsResultValueId_WhenCustomerIsValid()
    {
        // Arrange
        var command = new CreateCustomerCommand("Jhon Doe", "jhon.doe@email.com");

        _customerRepositoryMock.EmailExistsAsync(Arg.Any<string>()).Returns(false);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        await _customerRepositoryMock.Received(1).AddAsync(Arg.Any<Customer>());

        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeOfType<string>();
        result.Value.Should().NotBeNullOrWhiteSpace();
    }
}
