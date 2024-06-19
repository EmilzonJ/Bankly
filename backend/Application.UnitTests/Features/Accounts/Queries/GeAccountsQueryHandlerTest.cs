using Application.Features.Accounts.Extensions;
using Application.Features.Accounts.Models.Responses;
using Application.Features.Accounts.Queries;
using Application.Features.Accounts.QueryHandlers;
using Application.Shared;
using MongoDB.Bson;
using Domain.Enums;

namespace Application.UnitTests.Features.Accounts.Queries
{
    public class GetAccountsQueryHandlerTest
    {
        private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();

        [Fact(DisplayName = "Handle_Should_ReturnsResultValueList_WhenAccountsExists")]
        public async Task Handle_Should_ReturnsResultValueList_WhenAccountsExists()
        {
            // Arrange
            List<Account> accounts =
            [
                new Account
                {
                    Id = new ObjectId(), Alias = "Account 1", Balance = 1000, Type = AccountType.Savings,
                    CustomerName = "Jhon Doe", CustomerEmail = "jhon.doe@mail.com"
                },
                new Account
                {
                    Id = new ObjectId(), Alias = "Account 2", Balance = 2000, Type = AccountType.Savings,
                    CustomerName = "Jhon Doe", CustomerEmail = "jhon.doe@mail.com"
                }
            ];

            _accountRepository.GetPagedAsync(1, 10, null, null, null).Returns(accounts);
            _accountRepository.CountAsync(null, null, null).Returns(2);

            var query = new GetAccountsQuery(1, 10);
            var handler = new GetAccountsQueryHandler(_accountRepository);

            // Act
            Result<PaginatedList<AccountResponse>> result = await handler.Handle(query, default);

            // Assert
            await _accountRepository
                .Received(1)
                .GetPagedAsync(1, 10, null, null, null);

            await _accountRepository
                .Received(1)
                .CountAsync(null, null, null);

            result.Value.Items.Should().BeEquivalentTo(accounts.Select(x => x.ToResponse()));
            result.Value.Items.Should().HaveCount(2);
        }

        [Fact(DisplayName = "Handle_Should_ReturnsResultValueEmptyList_WhenAccountsDoesntExists")]
        public async Task Handle_Should_ReturnsResultValueEmptyList_WhenAccountsDoesntExists()
        {
            // Arrange
            var accounts = Array.Empty<AccountWithCustomer>().ToList();

            _accountRepository.GetPagedAsync(1, 10, null, null, null).Returns(accounts);
            _accountRepository.CountAsync(null, null, null).Returns(0);

            var query = new GetAccountsQuery(1, 10);
            var handler = new GetAccountsQueryHandler(_accountRepository);

            // Act
            Result<PaginatedList<AccountResponse>> result = await handler.Handle(query, default);

            // Assert
            await _accountRepository
                .Received(1)
                .GetPagedAsync(1, 10, null, null, null);

            await _accountRepository
                .Received(1)
                .CountAsync(null, null, null);

            result.Value.Items.Should().BeEquivalentTo(accounts.Select(x => x.ToResponse()));
            result.Value.Items.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Handle_Should_ReturnsFilteredResultValuePaginatedList_WhenFiltersAreApplied")]
        public async Task Handle_Should_ReturnsFilteredResultValuePaginatedList_WhenFiltersAreApplied()
        {
            // Arrange
            List<Account> accounts =
            [
                new Account
                {
                    Id = new ObjectId(),
                    Alias = "Account 1",
                    Balance = 1000,
                    Type = AccountType.Savings,
                    CustomerName = "Jhon Doe",
                    CustomerEmail = "jhon.doe@mail.com"
                }
            ];

            _accountRepository.GetPagedAsync(1, 10, "Account 1", null, null).Returns(accounts);
            _accountRepository.CountAsync("Account 1", null, null).Returns(1);

            var query = new GetAccountsQuery(1, 10, "Account 1");
            var handler = new GetAccountsQueryHandler(_accountRepository);

            // Act
            Result<PaginatedList<AccountResponse>> result = await handler.Handle(query, default);

            // Assert
            await _accountRepository
                .Received(1)
                .GetPagedAsync(1, 10, "Account 1", null, null);

            await _accountRepository
                .Received(1)
                .CountAsync("Account 1", null, null);

            result.Value.Items.Should().BeEquivalentTo(accounts.Select(x => x.ToResponse()));
            result.Value.Items.Should().HaveCount(1);
        }
    }
}
