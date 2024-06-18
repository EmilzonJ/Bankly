using Application.Shared;

namespace Application.Features.Accounts.Models.Filters;

public record GetAccountsFilter : PaginationFilter
{
    public string? Alias { get; set; }
    public string? CustomerName { get; set; }
    public DateOnly? CreatedAt { get; set; }
}
