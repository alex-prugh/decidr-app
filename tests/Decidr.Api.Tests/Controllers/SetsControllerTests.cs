using Decidr.Api.Controllers;
using Decidr.Api.Dtos;
using Decidr.Operations;
using Decidr.Operations.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Decidr.Api.Tests.Controllers;

public class SetsControllerTests
{
    private ISetsOperation _subSetsOperation = null!;

    private SetsController CreateController()
    {
        _subSetsOperation = Substitute.For<ISetsOperation>();
        return new SetsController(_subSetsOperation);
    }

    [Fact]
    public async Task GetAllAsync_WithSets_ReturnsSetDtoList()
    {
        var controller = CreateController();
        var sets = new List<Set> { 
            new Set {
                Id = 1, 
                Name = "Test Set",
                Cards = new List<Card>()
            }
        };

        _subSetsOperation.GetAllSetsAsync(Arg.Any<CancellationToken>())
            .Returns(sets);

        var result = await controller.GetAllAsync();

        Assert.IsType<List<SetDto>>(result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsOkWithSetDto()
    {
        var controller = CreateController();
        var set = new Set
        {
            Id = 1,
            Name = "Test Set",
            Cards = new List<Card>()
        };

        _subSetsOperation.GetSetAsync(1, Arg.Any<CancellationToken>())
            .Returns(set);

        var result = await controller.GetByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<SetDto>(okResult.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsBadRequest()
    {
        var controller = CreateController();

        var result = await controller.GetByIdAsync(0);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetByIdAsync_WithNotFoundId_ReturnsNotFound()
    {
        var controller = CreateController();

        _subSetsOperation.GetSetAsync(999, Arg.Any<CancellationToken>())
            .Returns((Set?)null);

        var result = await controller.GetByIdAsync(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetResultsAsync_WithValidId_ReturnsSetResultDto()
    {
        var controller = CreateController();
        var setResult = new SetResult
        {
            Id = 1,
            CardSummaries = new List<CardSummary>()
        };

        _subSetsOperation.GetSetResultAsync(1, Arg.Any<CancellationToken>())
            .Returns(setResult);

        var result = await controller.GetResultsAsync(1);

        Assert.IsType<SetResultDto>(result.Value);
    }

    [Fact]
    public async Task GetResultsAsync_WithNoResults_ReturnsNull()
    {
        var controller = CreateController();

        _subSetsOperation.GetSetResultAsync(999, Arg.Any<CancellationToken>())
            .Returns((SetResult?)null);

        var result = await controller.GetResultsAsync(999);

        Assert.Null(result.Value);
    }

    [Fact]
    public async Task AddMemberAsync_WithValidRequest_ReturnsTrue()
    {
        var controller = CreateController();
        var request = new AddMemberRequestDto { Email = "test@example.com" };

        _subSetsOperation.AddMemberAsync(1, request.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await controller.AddMemberAsync(1, request);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task AddMemberAsync_WithFailedAdd_ReturnsNotFound()
    {
        var controller = CreateController();
        var request = new AddMemberRequestDto { Email = "nonexistent@example.com" };

        _subSetsOperation.AddMemberAsync(1, request.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await controller.AddMemberAsync(1, request);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
