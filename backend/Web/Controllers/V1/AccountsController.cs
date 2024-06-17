using Application.Extensions;
using Application.Features.Accounts.Commands;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.V1;

public class AccountsController(ISender sender) : BaseController
{
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
