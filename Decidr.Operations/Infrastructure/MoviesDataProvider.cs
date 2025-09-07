using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface IMoviesDataProvider
{
    public Task<List<Movie>> SearchAsync(string searchText, CancellationToken cancellationToken = default);
    public Task<List<Movie>> GetLatestPopularAsync(CancellationToken cancellationToken = default);
    public Task<List<Movie>> GetTopRatedAsync(CancellationToken cancellationToken = default);
}
