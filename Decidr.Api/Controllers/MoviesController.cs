using Decidr.Api.Dtos;
using Decidr.Api.Extensions;
using Decidr.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Decidr.Api.Controllers;

[ApiController]
[Route("api/movies")]
[RequiresUserContext]
public class MoviesController : ControllerBase
{
    private readonly IMoviesOperation _moviesOperation;

    public MoviesController(
        IMoviesOperation moviesOperation)
    {
        _moviesOperation = moviesOperation;
    }

    [HttpGet("popular")]
    [Authorize]
    public async Task<ActionResult<SetDto?>> Get(CancellationToken cancellationToken = default)
    {
        var set = await _moviesOperation.GetPopularMoviesSetAsync(cancellationToken);
        if (set == null)
        {
            return StatusCode(500, "Internal server error");
        }

        return set?.ToDto();
    }
}
