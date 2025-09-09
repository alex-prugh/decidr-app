using Decidr.Operations;
using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using NSubstitute;

namespace Decidr.Operations.Tests;

public class CardsOperationTests : UserContextRequiredTests
{
    private ICardsDataProvider _subCardsDataProvider = null!;

    private CardsOperation CreateOperation()
    {
        _subCardsDataProvider = Substitute.For<ICardsDataProvider>();
        return new CardsOperation(_subCardsDataProvider, _subUserContext);
    }

    [Fact]
    public async Task LikeAsync_WhenSuccessful_ReturnsTrue()
    {
        var operation = CreateOperation();
        _subCardsDataProvider.LikeAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await operation.LikeAsync(1, default);

        Assert.True(result);
    }

    [Fact]
    public async Task DislikeAsync_WhenSuccessful_ReturnsTrue()
    {
        var operation = CreateOperation();
        _subCardsDataProvider.DislikeAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await operation.DislikeAsync(1, default);
        Assert.True(result);
    }

    [Fact]
    public async Task LikeAsync_WhenUnsuccessful_ReturnsFalse()
    {
        var operation = CreateOperation();
        _subCardsDataProvider.LikeAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await operation.LikeAsync(1, default);

        Assert.False(result);
    }

    [Fact]
    public async Task DislikeAsync_WhenUnsuccessful_ReturnsFalse()
    {
        var operation = CreateOperation();
        _subCardsDataProvider.DislikeAsync(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await operation.DislikeAsync(1, default);

        Assert.False(result);
    }
}
