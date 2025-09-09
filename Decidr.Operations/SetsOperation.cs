using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface ISetsOperation
{
    /// <summary>
    /// Gets all sets for the logged-in user.
    /// </summary>
    public Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a given set for the user.
    /// </summary>
    public Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the voting results for aset.
    /// </summary>
    public Task<SetResult?> GetSetResultAsync(long setId, CancellationToken token = default);
    
    /// <summary>
    /// Adds a member to view and vote on the set.
    /// </summary>
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

    /// <inheritdoc />
    public async Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken)
    {
        var sets = await _setsDataProvider.GetAllForUser(_userContext.GetUserOrThrow().Id, false, cancellationToken);
        return sets;
    }

    /// <inheritdoc />
    public async Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken)
    {
        var set = await _setsDataProvider.Get(setId, _userContext.GetUserOrThrow().Id, true, cancellationToken);
        return set;
    }

    /// <inheritdoc />
    public async Task<SetResult?> GetSetResultAsync(long setId, CancellationToken cancellationToken)
    {
        var cardSummariesForSet = await _setsDataProvider.GetCardActivities(setId, _userContext.GetUserOrThrow().Id, cancellationToken);

        // Determine the top rated card. If multiple cards have the same rating, choose a random one.
        if (cardSummariesForSet == null || !cardSummariesForSet.Any())
            return null;

        var cardSummaries = cardSummariesForSet
            .OrderByDescending(c => c.Likes - c.Dislikes)
            .ThenByDescending(c => c.Likes)
            .ThenBy(c => c.Id); // If there's the same amount of likes and dislikes then just pick by id.

        var topCard = cardSummaries.FirstOrDefault();
        if (topCard != null)
        {
            topCard.IsSuggested = true;
        }

        return new SetResult
        {
            Id = setId,
            CardSummaries = cardSummaries.ToList()
        };
    }

    /// <inheritdoc />
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
