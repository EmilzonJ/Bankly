namespace Application.Shared;

public class PaginatedList<T>(
    List<T> items,
    int count,
    int pageNumber,
    int pageSize
)
{
    public List<T> Items { get; set; } = items;
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = count;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
