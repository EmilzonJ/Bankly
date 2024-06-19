using Application.Features.Customers.Extensions;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Application.Features.Customers.QueryHandlers;
using Application.Shared;
using MongoDB.Bson;

namespace Application.UnitTests.Features.Customers.Queries;

public class GetCustomersQueryHandlerTest
{
    private readonly ICustomerReadRepository _customerReadRepository = Substitute.For<ICustomerReadRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueList_WhenCustomersExists")]
    public async Task Handle_Should_ReturnsResultValueList_WhenCustomersExists()
    {
        // Arrange
        List<Customer> customers =
        [
            new Customer {Id = new ObjectId(), Name = "John Doe", Email = "jhon.doe@mail.com"},
            new Customer {Id = new ObjectId(), Name = "Jane Doe", Email = "jane.doe@mail.com"}
        ];

        _customerReadRepository.GetPagedAsync(1, 10, null, null, null).Returns(customers);
        _customerReadRepository.CountAsync(null, null, null).Returns(2);

        var query = new GetCustomersQuery(1, 10);
        var handler = new GetCustomersQueryHandler(_customerReadRepository);

        // Act
        Result<PaginatedList<CustomerResponse>> result = await handler.Handle(query, default);

        // Assert
        result.Value.Items.Should().BeEquivalentTo(customers.Select(x => x.ToResponse()));
        result.Value.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueEmptyList_WhenCustomersDoesntExists")]
    public async Task Handle_Should_ReturnsResultValueEmptyList_WhenCustomersDoesntExists()
    {
        // Arrange
        var customers = Array.Empty<Customer>().ToList();

        _customerReadRepository.GetPagedAsync(1, 10, null, null, null).Returns(customers);
        _customerReadRepository.CountAsync(null, null, null).Returns(0);

        var query = new GetCustomersQuery(1, 10);
        var handler = new GetCustomersQueryHandler(_customerReadRepository);

        // Act
        Result<PaginatedList<CustomerResponse>> result = await handler.Handle(query, default);

        // Assert
        result.Value.Items.Should().BeEquivalentTo(customers.Select(x => x.ToResponse()));
        result.Value.Items.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Handle_Should_ReturnsFilteredResultValuePaginatedList_WhenFiltersAreApplied")]
    public async Task Handle_Should_ReturnsFilteredResultValuePaginatedList_WhenFiltersAreApplied()
    {
        // Arrange
        List<Customer> customers =
        [
            new Customer
            {
                Id = new ObjectId(),
                Name = "John Doe",
                Email = "john.doe@mail.com"
            }
        ];

        _customerReadRepository.GetPagedAsync(1, 10, "John Doe", null, null).Returns(customers);
        _customerReadRepository.CountAsync("John Doe", null, null).Returns(1);

        var query = new GetCustomersQuery(1, 10, "John Doe");
        var handler = new GetCustomersQueryHandler(_customerReadRepository);

        // Act
        Result<PaginatedList<CustomerResponse>> result = await handler.Handle(query, default);

        // Assert
        result.Value.Items.Should().BeEquivalentTo(customers.Select(x => x.ToResponse()));
        result.Value.Items.Should().HaveCount(1);
    }
}
