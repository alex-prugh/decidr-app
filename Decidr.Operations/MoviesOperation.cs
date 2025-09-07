using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface IMoviesOperation
{
    public Task<Set> GetPopularMoviesSetAsync(CancellationToken cancellationToken = default);
}

public class MoviesOperation : IMoviesOperation
{
    private readonly IMoviesDataProvider _moviesDataProvider;

    public MoviesOperation(
        IMoviesDataProvider moviesDataProvider)
    {
        _moviesDataProvider = moviesDataProvider;
    }

    public async Task<Set> GetPopularMoviesSetAsync(CancellationToken cancellationToken = default)
    {
        var movies = await _moviesDataProvider.GetLatestPopularAsync(cancellationToken);

        var setToReturn = new Set()
        {
            Id = 1,
            Name = "GULP",
            Cards = movies.Select(m => new Card
            {
                Id = 1,
                Title = m.Title,
                Description = m.Description,
                ImageUrl = m.ImageUrl,
                SetId = 1,
            }).ToList()
        };

        return setToReturn;
    }
}
