using Application.Features.Customers.Commands;
using Application.Features.Customers.Models.Responses;
using Domain.Collections;

namespace Application.Features.Customers.Extensions;

public static class CustomerMapping
{
    public static CustomerResponse ToResponse(this Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id.ToString(),
            Name = customer.Name,
            Email = customer.Email,
            RegisteredAt = customer.RegisteredAt
        };
    }

    public static List<CustomerResponse> ToResponse(this IEnumerable<Customer> customers)
    {
        return customers.Select(c => c.ToResponse()).ToList();
    }

    public static Customer ToEntity(this CreateCustomerCommand command)
    {
        return new Customer
        {
            Name = command.Name,
            Email = command.Email,
            RegisteredAt = DateTime.UtcNow
        };
    }
}
