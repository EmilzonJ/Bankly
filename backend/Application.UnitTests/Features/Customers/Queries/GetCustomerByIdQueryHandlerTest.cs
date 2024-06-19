using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Application.Features.Customers.QueryHandlers;
using MongoDB.Bson;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Features.Customers.Queries;

public class GetCustomerByIdQueryHandlerTest
{
    private readonly ICustomerReadRepository _customerReadRepository = Substitute.For<ICustomerReadRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultValue_WhenCustomerExists")]
    public async Task Handle_Should_ReturnsResultValue_WhenCustomerExists()
    {
        // Arrange
        var customerId = new ObjectId();
        var customer = new Customer {Id = customerId, Name = "John Doe", Email = "jhon.due@mailc.com"};

        _customerReadRepository.GetByIdAsync(customerId).Returns(customer);

        var query = new GetCustomerByIdQuery(customerId);
        var handler = new GetCustomerByIdQueryHandler(_customerReadRepository);

        // Act
        Result<CustomerResponse> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(customer.ToResponse());
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenCustomerDoesNotExist")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = new ObjectId();

        _customerReadRepository.GetByIdAsync(customerId).ReturnsNull();

        var query = new GetCustomerByIdQuery(customerId);
        var handler = new GetCustomerByIdQueryHandler(_customerReadRepository);

        // Act
        Result<CustomerResponse> result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CustomerErrors.NotFound(customerId));
    }
}
