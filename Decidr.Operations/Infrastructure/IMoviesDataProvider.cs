using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface IMoviesDataProvider
{
    /// <summary>
    /// Searches for movies based on a search term.
    /// </summary>
    public Task<List<Movie>> SearchAsync(string searchText, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest popular movies.
    /// </summary>
    public Task<List<Movie>> GetLatestPopularAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the top rated movies.
    /// </summary>
    public Task<List<Movie>> GetTopRatedAsync(CancellationToken cancellationToken = default);
}
