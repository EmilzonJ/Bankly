using Application.Extensions;
using Application.Features.Transactions.Commands;
using Application.Features.Transactions.Models.Filters;
using Application.Features.Transactions.Models.Requests;
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

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionDetailResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetAsync(string id)
    {
        var idParsed = id.ToObjectId();
        if (!idParsed.IsSuccess) return idParsed.ToProblemDetails();

        var result = await sender.Send(new GetTransactionByIdQuery(idParsed.Value));

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> PostAsync(TransactionCreateRequest request)
    {
        var sourceAccountId = request.SourceAccountId.ToObjectId();
        if (!sourceAccountId.IsSuccess) return sourceAccountId.ToProblemDetails();

        var destinationAccountId = request.DestinationAccountId.ToNullableObjectId();
        if (!destinationAccountId.IsSuccess) return destinationAccountId.ToProblemDetails();

        var command = new CreateTransactionCommand(
            sourceAccountId.Value,
            destinationAccountId.Value,
            request.Amount,
            request.Description,
            request.Type
        );

        var result = await sender.Send(command);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}
