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

    /// <summary>
    /// Grabs the most popular movies.
    /// </summary>
    /// <returns>A set of cards with the most popular movies</returns>
    [HttpGet("popular")]
    [Authorize]
    public async Task<ActionResult<SetDto?>> Get(CancellationToken cancellationToken = default)
    {
        var set = await _moviesOperation.GetPopularMoviesSetAsync(cancellationToken);

        // If we're unable to grab the movies, something is wrong internally.
        if (set == null)
        {
            return StatusCode(500, "Internal server error");
        }

        return set?.ToDto();
    }
}
