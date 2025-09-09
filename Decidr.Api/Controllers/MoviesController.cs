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
    public async Task<ActionResult<SetDto?>> GetPopularMoviesAsync(CancellationToken cancellationToken = default)
    {
        var set = await _moviesOperation.CreatePopularMoviesSetAsync(cancellationToken);

        // If we're unable to grab the movies, something is wrong internally.
        if (set == null)
        {
            return StatusCode(500, "Internal server error");
        }

        return set?.ToDto();
    }

    /// <summary>
    /// Grabs the top rated movies.
    /// </summary>
    /// <returns>A set of cards with the top rated movies</returns>
    [HttpGet("top-rated")]
    [Authorize]
    public async Task<ActionResult<SetDto?>> GetTopRatedMoviesAsync(CancellationToken cancellationToken = default)
    {
        var set = await _moviesOperation.CreateTopRatedMoviesSetAsync(cancellationToken);

        // If we're unable to grab the movies, something is wrong internally.
        if (set == null)
        {
            return StatusCode(500, "Internal server error");
        }

        return set?.ToDto();
    }
}
