using Application.Features.Customers.Commands;
using Application.Features.Customers.Extensions;

namespace Application.Features.Customers.CommandHandlers;

public record CreateCustomerCommandHandler(
    ICustomerWriteRepository CustomerWriteRepository,
    ICustomerReadRepository CustomerReadRepository
) : ICommandHandler<CreateCustomerCommand, Result<string>>
{
    public async ValueTask<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = request.ToEntity();

        var emailExists = await CustomerReadRepository.EmailExistsAsync(customer.Email);

        if (emailExists)
            return Result.Failure<string>(CustomerErrors.EmailTaken(customer.Email));

        await CustomerWriteRepository.AddAsync(customer);

        return customer.Id.ToString();
    }
}
