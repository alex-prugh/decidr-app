using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Decidr.Operations.Tests;

public class MoviesOperationTests : UserContextRequiredTests
{
    private IMoviesDataProvider _subMoviesDataProvider = null!;
    private ISetsDataProvider _subSetsDataProvider = null!;
    private ILogger<MoviesOperation> _subLogger = null!;

    private MoviesOperation CreateOperation()
    {
        _subMoviesDataProvider = Substitute.For<IMoviesDataProvider>();
        _subSetsDataProvider = Substitute.For<ISetsDataProvider>();
        _subLogger = Substitute.For<ILogger<MoviesOperation>>();
        return new MoviesOperation(_subMoviesDataProvider, _subSetsDataProvider, _subUserContext, _subLogger);
    }

    [Fact]
    public async Task CreatePopularMoviesSetAsync_WhenMoviesAreFoundAndSetIsCreated_ReturnsSet()
    {
        var operation = CreateOperation();
        var movies = new List<Movie>
        {
            new Movie { Title = "Movie 1", Description = "Desc 1", ImageUrl = "url1" }
        };
        var newSet = new Set { Id = 1, Name = "Popular Movies", Cards = new List<Card>() };

        _subMoviesDataProvider.GetLatestPopularAsync(Arg.Any<CancellationToken>())
            .Returns(movies);
        _subSetsDataProvider.CreateAsync(Arg.Any<string>(), Arg.Any<List<Card>>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(newSet);

        var result = await operation.CreatePopularMoviesSetAsync(default);

        Assert.NotNull(result);
        Assert.Equal(newSet, result);
    }

    [Fact]
    public async Task CreatePopularMoviesSetAsync_WhenSetCreationFails_ReturnsNull()
    {
        var operation = CreateOperation();
        var movies = new List<Movie>
        {
            new Movie { Title = "Movie 1", Description = "Desc 1", ImageUrl = "url1" }
        };

        _subMoviesDataProvider.GetLatestPopularAsync(Arg.Any<CancellationToken>())
            .Returns(movies);
        _subSetsDataProvider.CreateAsync(Arg.Any<string>(), Arg.Any<List<Card>>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns((Set?)null);

        var result = await operation.CreatePopularMoviesSetAsync(default);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTopRatedMoviesSetAsync_WhenMoviesAreFoundAndSetIsCreated_ReturnsSet()
    {
        var operation = CreateOperation();
        var movies = new List<Movie>
        {
            new Movie { Title = "Movie 1", Description = "Desc 1", ImageUrl = "url1" }
        };
        var newSet = new Set { Id = 1, Name = "Popular Movies", Cards = new List<Card>() };

        _subMoviesDataProvider.GetTopRatedAsync(Arg.Any<CancellationToken>())
            .Returns(movies);
        _subSetsDataProvider.CreateAsync(Arg.Any<string>(), Arg.Any<List<Card>>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(newSet);

        var result = await operation.CreateTopRatedMoviesSetAsync(default);

        Assert.NotNull(result);
        Assert.Equal(newSet, result);
    }

    [Fact]
    public async Task CreateTopRatedMoviesSetAsync_WhenSetCreationFails_ReturnsNull()
    {
        var operation = CreateOperation();
        var movies = new List<Movie>
        {
            new Movie { Title = "Movie 1", Description = "Desc 1", ImageUrl = "url1" }
        };

        _subMoviesDataProvider.GetTopRatedAsync(Arg.Any<CancellationToken>())
            .Returns(movies);
        _subSetsDataProvider.CreateAsync(Arg.Any<string>(), Arg.Any<List<Card>>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns((Set?)null);

        var result = await operation.CreateTopRatedMoviesSetAsync(default);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateSearchTermMoviesSetAsync_WhenMoviesAreFoundAndSetIsCreated_ReturnsSet()
    {
        var operation = CreateOperation();
        var movies = new List<Movie>
        {
            new Movie { Title = "Movie 1", Description = "Desc 1", ImageUrl = "url1" }
        };
        var newSet = new Set { Id = 1, Name = "Popular Movies", Cards = new List<Card>() };

        _subMoviesDataProvider.SearchAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(movies);
        _subSetsDataProvider.CreateAsync(Arg.Any<string>(), Arg.Any<List<Card>>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(newSet);

        var result = await operation.CreateSearchTermMoviesSetAsync("hi", default);

        Assert.NotNull(result);
        Assert.Equal(newSet, result);
    }

    [Fact]
    public async Task CreateSearchTermMoviesSetAsync_WhenSetCreationFails_ReturnsNull()
    {
        var operation = CreateOperation();
        var movies = new List<Movie>
        {
            new Movie { Title = "Movie 1", Description = "Desc 1", ImageUrl = "url1" }
        };

        _subMoviesDataProvider.SearchAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(movies);
        _subSetsDataProvider.CreateAsync(Arg.Any<string>(), Arg.Any<List<Card>>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns((Set?)null);

        var result = await operation.CreateSearchTermMoviesSetAsync("hi", default);

        Assert.Null(result);
    }
}
