using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface ISetsDataProvider
{
    public Task<Set?> CreateAsync(string name, ICollection<Card> cards, long creatorUserId, CancellationToken cancellationToken = default);
    public Task<List<Set>> GetAllForUser(long userId, CancellationToken cancellationToken = default);
    public Task<Set?> Get(long setId, long userId, CancellationToken cancellationToken = default);
    public Task<List<CardSummary>> GetCardActivities(long setId, long userId, CancellationToken cancellationToken = default);
    public Task<bool> AddMemberAsync(long setId, User user, long addedByUserId, CancellationToken cancellationToken);
}
