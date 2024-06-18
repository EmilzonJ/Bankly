namespace Application.Features.Customers.Models.Requests;

public record CreateCustomerAccountRequest(decimal Balance, string Alias);
