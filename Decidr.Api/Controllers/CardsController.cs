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
    private readonly ICardsOperation _cardsOperation;

    public CardsController(
        ICardsOperation cardsOperation)
    {
        _cardsOperation = cardsOperation;
    }

    /// <summary>
    /// Likes a card for the logged in user.
    /// </summary>
    [HttpPost("{id:int}/like")]
    [Authorize]
    public async Task<ActionResult<bool>> Like(long id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return BadRequest("Card id must be greater than 0");
        }

        var success = await _cardsOperation.LikeAsync(id, cancellationToken);
        return Ok(success);
    }

    /// <summary>
    /// Dislikes a card for the logged in user.
    /// </summary>
    [HttpPost("{id:long}/dislike")]
    [Authorize]
    public async Task<ActionResult<bool>> Dislike(long id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return BadRequest("Card id must be greater than 0");
        }

        var success = await _cardsOperation.DislikeAsync(id, cancellationToken);
        return Ok(success);
    }
}
