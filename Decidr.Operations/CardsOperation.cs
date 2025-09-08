using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface ICardsOperation
{
    public Task<bool> LikeAsync(long cardId, CancellationToken cancellationToken = default);
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

    public async Task<bool> LikeAsync(long cardId, CancellationToken cancellationToken = default)
    {
        var sets = await _cardsDataProvider.LikeAsync(_userContext.GetUserOrThrow().Id, cardId, cancellationToken);
        return sets;
    }

    public async Task<bool> DislikeAsync(long cardId, CancellationToken cancellationToken = default)
    {
        var sets = await _cardsDataProvider.DislikeAsync(_userContext.GetUserOrThrow().Id, cardId, cancellationToken);
        return sets;
    }
}
