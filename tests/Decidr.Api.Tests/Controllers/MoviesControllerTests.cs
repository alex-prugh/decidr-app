using Decidr.Api.Controllers;
using Decidr.Api.Dtos;
using Decidr.Operations;
using Decidr.Operations.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Decidr.Api.Tests.Controllers;

public class MoviesControllerTests
{
    private IMoviesOperation _subMoviesOperation = null!;

    private MoviesController CreateController()
    {
        _subMoviesOperation = Substitute.For<IMoviesOperation>();
        return new MoviesController(_subMoviesOperation);
    }

    [Fact]
    public async Task GetPopularMoviesAsync_WithValidMovies_ReturnsSetDto()
    {
        var controller = CreateController();
        var set = new Set {
            Id = 1, 
            Name = "Test Set", 
            Cards = new List<Card>()
        };

        _subMoviesOperation.CreatePopularMoviesSetAsync(Arg.Any<CancellationToken>())
            .Returns(set);

        var result = await controller.GetPopularMoviesAsync();

        Assert.IsType<ActionResult<SetDto?>>(result);
        Assert.IsType<SetDto>(result.Value);
    }

    [Fact]
    public async Task GetPopularMoviesAsync_WithNoMovies_ReturnsInternalServerError()
    {
        var controller = CreateController();

        _subMoviesOperation.CreatePopularMoviesSetAsync(Arg.Any<CancellationToken>())
            .Returns((Set?)null);

        var result = await controller.GetPopularMoviesAsync();
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);

        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetTopRatedMoviesAsync_WithValidMovies_ReturnsSetDto()
    {
        var controller = CreateController();
        var set = new Set
        {
            Id = 1,
            Name = "Test Set",
            Cards = new List<Card>()
        };

        _subMoviesOperation.CreateTopRatedMoviesSetAsync(Arg.Any<CancellationToken>())
            .Returns(set);

        var result = await controller.GetTopRatedMoviesAsync();

        Assert.IsType<ActionResult<SetDto?>>(result);
        Assert.IsType<SetDto>(result.Value);
    }

    [Fact]
    public async Task GetTopRatedMoviesAsync_WithNoMovies_ReturnsInternalServerError()
    {
        var controller = CreateController();

        _subMoviesOperation.CreateTopRatedMoviesSetAsync(Arg.Any<CancellationToken>())
            .Returns((Set?)null);

        var result = await controller.GetTopRatedMoviesAsync();
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);

        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}
