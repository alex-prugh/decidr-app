using Decidr.Api.Dtos;
using Decidr.Api.Extensions;
using Decidr.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Decidr.Api.Controllers;

[ApiController]
[Route("api/cards")]
[RequiresUserContext]
public class CardsController : ControllerBase
{
    private readonly ISetsOperation _setsOperation;

    public CardsController(
        ISetsOperation setsOperation)
    {
        _setsOperation = setsOperation;
    }

    [HttpPost("{id:int}/like")]
    [Authorize]
    public async Task<ActionResult<bool>> Like(long id, CancellationToken cancellationToken = default)
    {
        return true;
    }

    [HttpPost("{id:long}/dislike")]
    [Authorize]
    public async Task<ActionResult<bool>> Dislike(long id, CancellationToken cancellationToken = default)
    {
        return true;
    }
}
