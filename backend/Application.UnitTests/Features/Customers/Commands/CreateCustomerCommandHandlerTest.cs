using Application.Features.Customers.CommandHandlers;
using Application.Features.Customers.Commands;

namespace Application.UnitTests.Features.Customers.Commands;

public class CreateCustomerCommandHandlerTest
{
    private readonly ICustomerReadRepository _customerReadRepository = Substitute.For<ICustomerReadRepository>();
    private readonly ICustomerWriteRepository _customerWriteRepository = Substitute.For<ICustomerWriteRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsConflictResult_WhenEmailIsAlreadyTaken")]
    public async Task Handle_Should_ReturnsConflictResult_WhenEmailIsAlreadyTaken()
    {
        // Arrange
        const string email = "jhon.doe@email.com";
        var command = new CreateCustomerCommand("Jhon Doe", email);

        _customerReadRepository.EmailExistsAsync(Arg.Any<string>()).Returns(true);

        var handler = new CreateCustomerCommandHandler(_customerWriteRepository, _customerReadRepository);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        await _customerWriteRepository.Received(0).AddAsync(Arg.Any<Customer>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomerErrors.EmailTaken(email));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueId_WhenCustomerIsValid")]
    public async Task Handle_Should_ReturnsResultValueId_WhenCustomerIsValid()
    {
        // Arrange
        var command = new CreateCustomerCommand("Jhon Doe", "jhon.doe@email.com");

        _customerReadRepository.EmailExistsAsync(Arg.Any<string>()).Returns(false);

        var handler = new CreateCustomerCommandHandler(_customerWriteRepository, _customerReadRepository);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        await _customerWriteRepository.Received(1).AddAsync(Arg.Any<Customer>());

        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeOfType<string>();
        result.Value.Should().NotBeNullOrWhiteSpace();
    }
}
