using Application.Features.Auth.Commands;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.V1;

[AllowAnonymous]
public class AuthController(ISender sender) : BaseController
{
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> LoginAsync([FromBody] LoginCommand command)
    {
        var result = await sender.Send(command);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}
