using Decidr.Infrastructure.Movies.Dtos;
using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using System.Text.Json;

namespace Decidr.Infrastructure.Movies;

/// <summary>
/// This implementation queries movie info from the TheMovieDb API.
/// </summary>
public class MoviesDataProvider : IMoviesDataProvider
{
    private readonly HttpClient _httpClient;

    public MoviesDataProvider(
        IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("TheMovieDb");
    }

    /// <inheritdoc />
    public async Task<List<Movie>> SearchAsync(string searchText, CancellationToken cancellationToken = default)
    {
        var uri = $"search/movie?query={searchText}&include_adult=false&language=en-US&page=1";
        return await QueryMovieApi(uri);
    }

    /// <inheritdoc />
    public async Task<List<Movie>> GetLatestPopularAsync(CancellationToken cancellationToken = default)
    {
        var uri = "movie/popular?language=en-US&page=1";
        return await QueryMovieApi(uri);
    }

    /// <inheritdoc />
    public async Task<List<Movie>> GetTopRatedAsync(CancellationToken cancellationToken = default)
    {
        var uri = "movie/top_rated?language=en-US&page=1";
        return await QueryMovieApi(uri);
    }

    // <inheritdoc />
    private async Task<List<Movie>> QueryMovieApi(string uri)
    {
        using var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var movieResponse = JsonSerializer.Deserialize<TheMovieDbResults>(body);

        return movieResponse?.Movies?.Select(m => m.ToBusinessObject()).ToList() ?? [];
    }
}
