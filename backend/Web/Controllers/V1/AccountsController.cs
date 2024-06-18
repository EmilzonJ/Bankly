using Application.Extensions;
using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Models.Filters;
using Application.Features.Accounts.Models.Responses;
using Application.Features.Accounts.Queries;
using Application.Shared;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.V1;

public class AccountsController(ISender sender) : BaseController
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<AccountResponse>))]
    public async Task<IResult> GetAsync([FromQuery] GetAccountsFilter filters)
    {
        var query = new GetAccountsQuery(
            filters.PageNumber,
            filters.PageSize,
            filters.Alias,
            filters.CustomerName,
            filters.CreatedAt
        );

        var result = await sender.Send(query);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetAsync(string id)
    {
        var idParsed = id.ToObjectId();
        if (!idParsed.IsSuccess) return idParsed.ToProblemDetails();

        var result = await sender.Send(new GetAccountByIdQuery(idParsed.Value));

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet("{id}/transactions")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetTransactionsAsync(string id)
    {
        var idParsed = id.ToObjectId();
        if (!idParsed.IsSuccess) return idParsed.ToProblemDetails();

        var result = await sender.Send(new GetAccountTransactionsQuery(idParsed.Value));

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> DeleteAsync(string id)
    {
        var idParsed = id.ToObjectId();
        if (!idParsed.IsSuccess) return idParsed.ToProblemDetails();

        var result = await sender.Send(new DeleteAccountCommand(idParsed.Value));

        return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
    }
}
