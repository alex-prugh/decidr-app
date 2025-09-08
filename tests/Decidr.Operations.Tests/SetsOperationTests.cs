using Decidr.Operations;
using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using NSubstitute;

namespace Decidr.Operations.Tests;

public class SetsOperationTests : UserContextRequiredTests
{
    private ISetsDataProvider _subSetsDataProvider = null!;
    private IUsersDataProvider _subUsersDataProvider = null!;

    private SetsOperation CreateOperation()
    {
        _subSetsDataProvider = Substitute.For<ISetsDataProvider>();
        _subUsersDataProvider = Substitute.For<IUsersDataProvider>();
        return new SetsOperation(_subSetsDataProvider, _subUsersDataProvider, _subUserContext);
    }

    [Fact]
    public async Task GetAllSetsAsync_ReturnsListOfSets()
    {
        var operation = CreateOperation();
        var sets = new List<Set> { new Set(), new Set() };
        _subSetsDataProvider.GetAllForUser(Arg.Any<long>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(sets);

        var result = await operation.GetAllSetsAsync(default);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetSetAsync_WithValidId_ReturnsSet()
    {
        var operation = CreateOperation();
        var set = new Set { Id = 1 };
        _subSetsDataProvider.Get(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(set);

        var result = await operation.GetSetAsync(1, default);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetSetAsync_WithInvalidId_ReturnsNull()
    {
        var operation = CreateOperation();
        _subSetsDataProvider.Get(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns((Set?)null);

        var result = await operation.GetSetAsync(1, default);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSetResultAsync_WithValidId_ReturnsSetResult()
    {
        var operation = CreateOperation();
        var cardSummaries = new List<CardSummary>
        {
            new CardSummary { Id = 1, Likes = 5, Dislikes = 2 },
            new CardSummary { Id = 2, Likes = 8, Dislikes = 1 },
            new CardSummary { Id = 3, Likes = 5, Dislikes = 5 }
        };
        _subSetsDataProvider.GetCardActivities(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(cardSummaries);

        var result = await operation.GetSetResultAsync(1, default);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(3, result.CardSummaries.Count);
    }

    [Fact]
    public async Task GetSetResultAsync_WhenNoCardsExist_ReturnsNull()
    {
        var operation = CreateOperation();
        var cardSummaries = new List<CardSummary>();
        _subSetsDataProvider.GetCardActivities(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(cardSummaries);

        var result = await operation.GetSetResultAsync(1, default);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSetResultAsync_SortsByLikesMinusDislikesThenByLikes()
    {
        var operation = CreateOperation();
        var cardSummaries = new List<CardSummary>
        {
            new CardSummary { Id = 1, Likes = 1, Dislikes = 0 }, // Score 1, Likes 1
            new CardSummary { Id = 2, Likes = 2, Dislikes = 1 }, // Score 1, Likes 2
            new CardSummary { Id = 3, Likes = 0, Dislikes = 0 } // Score 0, Likes 0
        };
        _subSetsDataProvider.GetCardActivities(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(cardSummaries);

        var result = await operation.GetSetResultAsync(1, default);
        var sortedCards = result?.CardSummaries;

        Assert.NotNull(sortedCards);
        Assert.Equal(2, sortedCards[0].Id);
        Assert.Equal(1, sortedCards[1].Id);
        Assert.Equal(3, sortedCards[2].Id);
    }

    [Fact]
    public async Task AddMemberAsync_WhenUserAndSetExist_ReturnsTrue()
    {
        var operation = CreateOperation();
        var user = new User { Id = 2 };

        _subUsersDataProvider.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _subSetsDataProvider.AddMemberAsync(Arg.Any<long>(), Arg.Any<User>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await operation.AddMemberAsync(1, "test@example.com", default);

        Assert.True(result);
    }

    [Fact]
    public async Task AddMemberAsync_WhenUserDoesNotExist_ReturnsFalse()
    {
        var operation = CreateOperation();

        _subUsersDataProvider.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var result = await operation.AddMemberAsync(1, "test@example.com", default);

        Assert.False(result);
    }

    [Fact]
    public async Task AddMemberAsync_WhenSetsDataProviderFails_ReturnsFalse()
    {
        var operation = CreateOperation();
        var user = new User { Id = 2 };

        _subUsersDataProvider.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _subSetsDataProvider.AddMemberAsync(Arg.Any<long>(), Arg.Any<User>(), Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await operation.AddMemberAsync(1, "test@example.com", default);

        Assert.False(result);
    }
}
