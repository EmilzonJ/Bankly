namespace Application.Features.Customers.Models.Responses;

public class CustomerResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime RegisteredAt { get; set; }
}
