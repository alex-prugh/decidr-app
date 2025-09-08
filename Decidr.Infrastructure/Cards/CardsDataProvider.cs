using Decidr.Infrastructure.EntityFramework;
using Decidr.Infrastructure.EntityFramework.Models;
using Decidr.Operations.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Decidr.Infrastructure.Cards;

public class CardsDataProvider : ICardsDataProvider
{
    private readonly DecidrDbContext _dbContext;
    private readonly ILogger<CardsDataProvider> _logger;

    public CardsDataProvider(
        DecidrDbContext dbContext,
        ILogger<CardsDataProvider> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> DislikeAsync(long userId, long cardId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingEntity = await _dbContext.CardActivities
            .FirstOrDefaultAsync(ca => ca.UserId == userId && ca.CardId == cardId);

            if (existingEntity != null)
            {
                existingEntity.IsLiked = false;
                existingEntity.IsDisliked = true;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }

            // Make sure that this user has access to this set.
            var hasAccess = await UserHasAccessToCard(userId, cardId, cancellationToken);
            if (!hasAccess)
            {
                return false;
            }

            var activity = new CardActivityEntity
            {
                UserId = userId,
                CardId = cardId,
                IsDisliked = true,
                IsLiked = false,
            };

            _dbContext.Add(activity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to dislike card {Cid} for user {uid}", cardId, userId);
        }

        return false;
    }

    public async Task<bool> LikeAsync(long userId, long cardId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingEntity = await _dbContext.CardActivities
            .FirstOrDefaultAsync(ca => ca.UserId == userId && ca.CardId == cardId);

            if (existingEntity != null)
            {
                existingEntity.IsLiked = true;
                existingEntity.IsDisliked = false;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }

            // Make sure that this user has access to this set.
            var hasAccess = await UserHasAccessToCard(userId, cardId, cancellationToken);
            if (!hasAccess)
            {
                return false;
            }

            var activity = new CardActivityEntity
            {
                UserId = userId,
                CardId = cardId,
                IsDisliked = false,
                IsLiked = true,
            };

            _dbContext.Add(activity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to like card {Cid} for user {uid}", cardId, userId);
        }

        return false;
    }

    private async Task<bool> UserHasAccessToCard(long userId, long cardId, CancellationToken cancellationToken)
    {
        return await _dbContext.Cards
                        .Where(c => c.Id == cardId)
                        .AnyAsync(c => c.Set.Members.Any(sm => sm.UserId == userId), cancellationToken);
    }
}
