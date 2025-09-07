using Decidr.Api.Dtos;
using Decidr.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Decidr.Api.Controllers;

[ApiController]
[Route("api/movies")]
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
    public async Task<SetDto> Get(CancellationToken cancellationToken = default)
    {
        var set = await _moviesOperation.GetPopularMoviesSetAsync(cancellationToken);
        return set.ToDto();
    }
}
