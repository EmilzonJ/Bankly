using Application.Features.Customers.Commands;
using Application.Features.Customers.Extensions;
using Domain.Contracts;
using Mediator;

namespace Application.Features.Customers.CommandHandlers;

public record CreateCustomerCommandHandler(ICustomerRepository CustomerRepository) : ICommandHandler<CreateCustomerCommand, string>
{
    public async ValueTask<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = request.ToEntity();

        await CustomerRepository.AddAsync(customer);

        return customer.Id.ToString();
    }
}
