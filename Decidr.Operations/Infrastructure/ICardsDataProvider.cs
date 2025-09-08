namespace Decidr.Operations.Infrastructure;

public interface ICardsDataProvider
{
    public Task<bool> LikeAsync(long userId, long cardId, CancellationToken cancellationToken = default);
    public Task<bool> DislikeAsync(long userId, long cardId, CancellationToken cancellationToken = default);

}
