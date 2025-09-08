using Decidr.Api.Controllers;
using Decidr.Operations;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Decidr.Api.Tests.Controllers;

public class CardsControllerTests
{
    private ICardsOperation _subCardsOperation = null!;

    private CardsController CreateController()
    {
        _subCardsOperation = Substitute.For<ICardsOperation>();
        return new CardsController(_subCardsOperation);
    }

    [Fact]
    public async Task LikeAsync_WithValidId_ReturnsOk()
    {
        var controller = CreateController();
        const int cardId = 1;

        _subCardsOperation.LikeAsync(cardId, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await controller.LikeAsync(cardId);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task LikeAsync_WithInvalidId_ReturnsBadRequest()
    {
        var controller = CreateController();
        const int cardId = 0;

        var result = await controller.LikeAsync(cardId);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DislikeAsync_WithValidId_ReturnsOk()
    {
        var controller = CreateController();
        const long cardId = 1;

        _subCardsOperation.DislikeAsync(cardId, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await controller.DislikeAsync(cardId);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DislikeAsync_WithInvalidId_ReturnsBadRequest()
    {
        var controller = CreateController();
        const long cardId = 0;

        var result = await controller.DislikeAsync(cardId);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}

