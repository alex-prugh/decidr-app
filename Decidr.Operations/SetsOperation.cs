using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface ISetsOperation
{
    public Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken = default);
    public Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken = default);
    public Task<SetResult?> GetSetResultAsync(long setId, CancellationToken token = default);
    public Task<bool> AddMemberAsync(long setId, string email, CancellationToken cancellationToken);
}

public class SetsOperation : ISetsOperation
{
    private readonly ISetsDataProvider _setsDataProvider;
    private readonly IUsersDataProvider _usersDataProvider;
    private readonly UserContext _userContext;
    private static readonly Random _random = new Random();

    public SetsOperation(
        ISetsDataProvider setsDataProvider, 
        IUsersDataProvider usersDataProvider,
        UserContext userContext)
    {
        _setsDataProvider = setsDataProvider;
        _usersDataProvider = usersDataProvider;
        _userContext = userContext;
    }

    public async Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken)
    {
        var sets = await _setsDataProvider.GetAllForUser(_userContext.GetUserOrThrow().Id, false, cancellationToken);
        return sets;
    }

    public async Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken)
    {
        var set = await _setsDataProvider.Get(setId, _userContext.GetUserOrThrow().Id, true, cancellationToken);
        return set;
    }

    public async Task<SetResult?> GetSetResultAsync(long setId, CancellationToken cancellationToken)
    {
        var cardSummariesForSet = await _setsDataProvider.GetCardActivities(setId, _userContext.GetUserOrThrow().Id, cancellationToken);

        // Determine the top rated card. If multiple cards have the same rating, choose a random one.
        if (cardSummariesForSet == null || !cardSummariesForSet.Any())
            return null;

        var maxLikes = cardSummariesForSet.Max(c => c.Likes);
        var topCardId = (long)-1;

        // No votes yet
        if (maxLikes == 0)
        {
            topCardId = -1;
        }
        else
        {
            var topCards = cardSummariesForSet.Where(c => c.Likes == maxLikes).ToList();
            var topCard = topCards[_random.Next(topCards.Count)];
            topCardId = topCard.Id;
        }

        var cardSummaries = cardSummariesForSet
            .OrderByDescending(c => c.Id == topCardId)
            .ThenByDescending(c => c.Likes - c.Dislikes)
            .ThenByDescending(c => c.Likes);

        var cardSummary = cardSummaries.FirstOrDefault(cs => cs.Id ==  topCardId);
        if (cardSummary != null)
        {
            cardSummary.IsSuggested = true;
        }

        return new SetResult
        {
            Id = setId,
            CardSummaries = cardSummaries.ToList()
        };
    }

    public async Task<bool> AddMemberAsync(long setId, string email, CancellationToken cancellationToken)
    {
        var user = await _usersDataProvider.GetByEmailAsync(email, cancellationToken);
        if (user == null)
        {
            return false;
        }

        var success = await _setsDataProvider.AddMemberAsync(setId, user, _userContext.GetUserOrThrow().Id, cancellationToken);
        return success;
    }
}
