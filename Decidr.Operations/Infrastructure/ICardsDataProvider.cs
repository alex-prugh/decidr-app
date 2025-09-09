namespace Decidr.Operations.Infrastructure;

public interface ICardsDataProvider
{
    /// <summary>
    /// Like a card.
    /// </summary>
    public Task<bool> LikeAsync(long userId, long cardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dislike a card.
    /// </summary>
    public Task<bool> DislikeAsync(long userId, long cardId, CancellationToken cancellationToken = default);

}
