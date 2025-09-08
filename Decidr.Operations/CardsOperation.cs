using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface ICardsOperation
{
    /// <summary>
    /// Likes a card for user.
    /// </summary>
    public Task<bool> LikeAsync(long cardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dislikes a card for user.
    /// </summary>
    public Task<bool> DislikeAsync(long cardId, CancellationToken cancellationToken = default);
}

public class CardsOperation : ICardsOperation
{
    private readonly ICardsDataProvider _cardsDataProvider;
    private readonly UserContext _userContext;

    public CardsOperation(
        ICardsDataProvider cardsDataProvider, 
        UserContext userContext)
    {
        _cardsDataProvider = cardsDataProvider;
        _userContext = userContext;
    }

    /// <inheritdoc />
    public async Task<bool> LikeAsync(long cardId, CancellationToken cancellationToken = default)
    {
        var sets = await _cardsDataProvider.LikeAsync(_userContext.GetUserOrThrow().Id, cardId, cancellationToken);
        return sets;
    }

    /// <inheritdoc />
    public async Task<bool> DislikeAsync(long cardId, CancellationToken cancellationToken = default)
    {
        var sets = await _cardsDataProvider.DislikeAsync(_userContext.GetUserOrThrow().Id, cardId, cancellationToken);
        return sets;
    }
}
