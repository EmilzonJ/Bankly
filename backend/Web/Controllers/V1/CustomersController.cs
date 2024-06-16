using Application.Extensions;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Models.Requests;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.V1;

public class CustomersController(ISender sender) : BaseController
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerResponse>))]
    public async Task<IResult> GetAsync()
    {
        var result = await sender.Send(new GetCustomersQuery());

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponse))]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetAsync(string id)
    {
        var idParsed = id.ToObjectId();
        if (!idParsed.IsSuccess) return idParsed.ToProblemDetails();

        var result = await sender.Send(new GetCustomerByIdQuery(idParsed.Value));

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> PostAsync(CreateCustomerCommand command)
    {
        var result = await sender.Send(command);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(string id, UpdateCustomerRequest request)
    {
        var idParsed = id.ToObjectId();
        if (!idParsed.IsSuccess) return idParsed.ToProblemDetails();

        var command = new UpdateCustomerCommand(idParsed.Value, request.Name, request.Email);

        var result = await sender.Send(command);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync(string id)
    {
        var idParsed = id.ToObjectId();
        if (!idParsed.IsSuccess) return idParsed.ToProblemDetails();

        var command = new DeleteCustomerCommand(idParsed.Value);

        var result = await sender.Send(command);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }
}
