using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Decidr.Operations;

public interface IMoviesOperation
{
    /// <summary>
    /// Creates a set with the most popular movies.
    /// </summary>
    public Task<Set?> CreatePopularMoviesSetAsync(CancellationToken cancellationToken = default);
}

public class MoviesOperation : IMoviesOperation
{
    private readonly IMoviesDataProvider _moviesDataProvider;
    private readonly ISetsDataProvider _setsDataProvider;
    private readonly UserContext _userContext;
    private readonly ILogger<MoviesOperation> _logger;

    public MoviesOperation(
        IMoviesDataProvider moviesDataProvider,
        ISetsDataProvider setsDataProvider,
        UserContext userContext,
        ILogger<MoviesOperation> logger)
    {
        _moviesDataProvider = moviesDataProvider;
        _setsDataProvider = setsDataProvider;
        _userContext = userContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Set?> CreatePopularMoviesSetAsync(CancellationToken cancellationToken)
    {
        var movies = await _moviesDataProvider.GetLatestPopularAsync(cancellationToken);

        var newSet = await CreateSetFromMoviesAsync($"Popular Movies: {DateTime.Today.ToString("yyyy-MM-dd")}", movies, cancellationToken);
        return newSet;
    }

    private async Task<Set?> CreateSetFromMoviesAsync(string setName, List<Movie> movies, CancellationToken cancellationToken)
    {
        var loggedInUserId = _userContext.GetUserOrThrow().Id;
        var cards = movies.Select(m => new Card
        {
            Title = m.Title,
            Description = m.Description,
            ImageUrl = m.ImageUrl,
        });

        var newSet = await _setsDataProvider.CreateAsync(setName, cards.ToList(), loggedInUserId, cancellationToken);
        if (newSet == null)
        {
            return null;
        }

        _logger.LogInformation("Successfully created new set {Sid} for user {Uid}", newSet.Id, loggedInUserId);

        return newSet;
    }
}
