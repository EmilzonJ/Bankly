namespace Application.Shared;

public record PaginationFilter
{
    public int PageNumber { get; set; } = Constants.Pagination.DefaultPageNumber;
    public int PageSize { get; set; } = Constants.Pagination.DefaultPageSize;
}
