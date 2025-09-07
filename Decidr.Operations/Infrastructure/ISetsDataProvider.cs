using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface ISetsDataProvider
{
    public Task<Set?> CreateAsync(string name, ICollection<Card> cards, long creatorUserId, CancellationToken cancellationToken = default);
}
