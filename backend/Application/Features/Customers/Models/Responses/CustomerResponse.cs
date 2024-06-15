namespace Application.Features.Customers.Models.Responses;

public class CustomerResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime RegisteredAt { get; set; }
}
