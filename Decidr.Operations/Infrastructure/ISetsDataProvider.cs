using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface ISetsDataProvider
{
    /// <summary>
    /// Creates a new set and adds the passed-in cards to it.
    /// </summary>
    public Task<Set?> CreateAsync(string name, ICollection<Card> cards, long creatorUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all sets for a given user.
    /// </summary>
    /// <param name="userId">The user id of the user who we are grabbing sets for</param>
    /// <param name="includeCards">Whether or not to include this set's card information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public Task<List<Set>> GetAllForUser(long userId, bool includeCards = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single set for a user.
    /// </summary>
    /// <param name="setId">The set to grab</param>
    /// <param name="userId">The user id of the user who we are grabbing sets for</param>
    /// <param name="includeCards">Whether or not to include this set's card information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public Task<Set?> Get(long setId, long userId, bool includeCards = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information on each member that has voted on the cards in the provided set.
    /// </summary>
    public Task<List<CardSummary>> GetCardActivities(long setId, long userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a member to a set so they can view it and vote on it.
    /// </summary>
    public Task<bool> AddMemberAsync(long setId, User user, long addedByUserId, CancellationToken cancellationToken);
}
