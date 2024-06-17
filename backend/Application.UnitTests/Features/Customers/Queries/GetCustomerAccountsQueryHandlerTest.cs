using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Application.Features.Customers.QueryHandlers;
using Domain.Enums;
using MongoDB.Bson;

namespace Application.UnitTests.Features.Customers.Queries;

public class GetCustomerAccountsQueryHandlerTest
{
    private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();

    [Fact(DisplayName = "Handle_Should_ReturnsResultNotFound_WhenCustomerDoesNotExist")]
    public async Task Handle_Should_ReturnsResultNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = new ObjectId();
        var query = new GetCustomerAccountsQuery(customerId);

        _customerRepository.ExistsAsync(query.CustomerId).Returns(false);

        var handler = new GetCustomerAccountsQueryHandler(_customerRepository, _accountRepository);

        // Act
        Result<List<CustomerAccountResponse>> result = await handler.Handle(query, default);

        // Assert
        await _accountRepository
            .Received(0)
            .GetAllByCustomerAsync(Arg.Any<ObjectId>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CustomerErrors.NotFound(customerId));
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueListEmpty_WhenCustomerExists")]
    public async Task Handle_Should_ReturnsResultValueListEmpty_WhenCustomerExists()
    {
        // Arrange
        var query = new GetCustomerAccountsQuery(new ObjectId());

        _customerRepository.ExistsAsync(query.CustomerId).Returns(true);
        _accountRepository.GetAllByCustomerAsync(query.CustomerId).Returns([]);

        var handler = new GetCustomerAccountsQueryHandler(_customerRepository, _accountRepository);

        // Act
        Result<List<CustomerAccountResponse>> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact(DisplayName = "Handle_Should_ReturnsResultValueList_WhenCustomerExists")]
    public async Task Handle_Should_ReturnsResultValueList_WhenCustomerExists()
    {
        // Arrange
        var query = new GetCustomerAccountsQuery(new ObjectId());
        var accounts = new List<Account>
        {
            new()
            {
                Id = ObjectId.GenerateNewId(),
                CustomerId = query.CustomerId,
                Balance = 1000,
                Type = AccountType.Savings,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _customerRepository.ExistsAsync(query.CustomerId).Returns(true);
        _accountRepository.GetAllByCustomerAsync(query.CustomerId).Returns(accounts);

        var handler = new GetCustomerAccountsQueryHandler(_customerRepository, _accountRepository);

        // Act
        Result<List<CustomerAccountResponse>> result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Id.Should().Be(accounts.First().Id.ToString());
        result.Value.First().CustomerId.Should().Be(accounts.First().CustomerId.ToString());
        result.Value.First().Balance.Should().Be(accounts.First().Balance);
        result.Value.First().Type.Should().Be(accounts.First().Type);
        result.Value.First().CreatedAt.Should().Be(accounts.First().CreatedAt);
        result.Value.First().UpdatedAt.Should().Be(accounts.First().UpdatedAt);
    }

}
