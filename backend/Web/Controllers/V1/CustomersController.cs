using Application.Features.Customers.Commands;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.V1;

public class CustomersController(ISender sender) : BaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerResponse>))]
    public async Task<IResult> GetAsync()
    {
        var result = await sender.Send(new GetCustomersQuery());
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IResult> PostAsync(CreateCustomerCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}
