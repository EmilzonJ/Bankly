using Application.Shared;

namespace Application.Features.Customers.Models.Filters;

public record GetCustomersFilter : PaginationFilter
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateOnly? RegisteredAt { get; set; }
}
