using Mediator;
using Shared;

namespace Application.Features.Customers.Commands;

public record CreateCustomerCommand(
    string Name,
    string Email
) : ICommand<Result<string>>;
