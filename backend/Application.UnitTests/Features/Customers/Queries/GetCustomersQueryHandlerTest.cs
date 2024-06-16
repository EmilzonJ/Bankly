using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Application.Features.Customers.QueryHandlers;
using MongoDB.Bson;

namespace Application.UnitTests.Features.Customers.Queries;

public class GetCustomersQueryHandlerTest
{
    private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueList_WhenCustomersExists")]
    public async Task Handle_Should_ReturnsResultValueList_WhenCustomersExists()
    {
        // Arrange
        List<Customer> customers =
        [
            new Customer {Id = new ObjectId(), Name = "John Doe", Email = "jhon.doe@mail.com"},
            new Customer {Id = new ObjectId(), Name = "Jane Doe", Email = "jane.doe@mail.com"}
        ];

        _customerRepository.GetAllAsync().Returns(customers);

        var query = new GetCustomersQuery();
        var handler = new GetCustomersQueryHandler(_customerRepository);

        // Act
        Result<List<CustomerResponse>> result = await handler.Handle(query, default);

        // Assert
        result.Should().BeOfType<Result<List<CustomerResponse>>>();
        result.Value.Should().BeEquivalentTo(customers.Select(x => x.ToResponse()));
        result.Value.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueEmptyList_WhenCustomersDoesntExists")]
    public async Task Handle_Should_ReturnsResultValueEmptyList_WhenCustomersDoesntExists()
    {
        // Arrange
        var customers = Array.Empty<Customer>().ToList();

        _customerRepository.GetAllAsync().Returns(customers);

        var query = new GetCustomersQuery();
        var handler = new GetCustomersQueryHandler(_customerRepository);

        // Act
        Result<List<CustomerResponse>> result = await handler.Handle(query, default);

        // Assert
        result.Should().BeOfType<Result<List<CustomerResponse>>>();
        result.Value.Should().BeEquivalentTo(customers.Select(x => x.ToResponse()));
        result.Value.Should().HaveCount(0);
    }
}
