using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface ISetsOperation
{
    public Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken = default);
    public Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken = default);
    public Task<SetResult?> GetSetResultAsync(long setId, CancellationToken token = default);
}

public class SetsOperation : ISetsOperation
{
    private readonly ISetsDataProvider _setsDataProvider;
    private readonly UserContext _userContext;
    private static readonly Random _random = new Random();

    public SetsOperation(ISetsDataProvider setsDataProvider, UserContext userContext)
    {
        _setsDataProvider = setsDataProvider;
        _userContext = userContext;
    }

    public async Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken)
    {
        var sets = await _setsDataProvider.GetAllForUser(_userContext.GetUserOrThrow().Id, cancellationToken);
        return sets;
    }

    public async Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken)
    {
        var set = await _setsDataProvider.Get(setId, _userContext.GetUserOrThrow().Id, cancellationToken);
        return set;
    }

    public async Task<SetResult?> GetSetResultAsync(long setId, CancellationToken cancellationToken)
    {
        var cardSummariesForSet = await _setsDataProvider.GetCardActivities(setId, _userContext.GetUserOrThrow().Id, cancellationToken);

        // Determine the top rated card. If multiple cards have the same rating, choose a random one.
        if (cardSummariesForSet == null || !cardSummariesForSet.Any())
            return null;

        var maxLikes = cardSummariesForSet.Max(c => c.Likes);
        var topCards = cardSummariesForSet.Where(c => c.Likes == maxLikes).ToList();
        var topCard = topCards[_random.Next(topCards.Count)];

        var cardSummaries = cardSummariesForSet
        .OrderByDescending(c => c.Id == topCard.Id)
        .ThenByDescending(c => c.Likes);

        return new SetResult
        {
            Id = setId,
            CardSummaries = cardSummariesForSet
        };
    }
}
