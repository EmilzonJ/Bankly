namespace Application.Features.Customers.Models.Filters;

public class GetCustomersFilter
{
    public int PageNumber { get; set; } = Constants.Pagination.DefaultPageNumber;
    public int PageSize { get; set; } = Constants.Pagination.DefaultPageSize;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime? RegisteredAt { get; set; }
}
