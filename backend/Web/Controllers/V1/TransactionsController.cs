using Application.Features.Transactions.Models.Filters;
using Application.Features.Transactions.Models.Responses;
using Application.Features.Transactions.Queries;
using Application.Shared;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.V1;

public class TransactionsController(ISender sender) : BaseController
{
    [HttpGet]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<TransactionResponse>))]
    public async Task<IResult> GetAsync([FromQuery] GetTransactionsFilter filters)
    {
        var query = new GetTransactionsQuery(
            filters.PageNumber,
            filters.PageSize,
            filters.Type,
            filters.Reference,
            filters.Description,
            filters.CreatedAt
        );

        var result = await sender.Send(query);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}
