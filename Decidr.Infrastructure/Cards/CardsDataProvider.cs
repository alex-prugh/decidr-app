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
            var card = await GetCardAsync(userId, cardId, cancellationToken);
            if (card == null)
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

            // Now update that they have voted.
            await RecordUserHasVoted(userId, card, cancellationToken);

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
            var card = await GetCardAsync(userId, cardId, cancellationToken);
            if (card == null)
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

            // Now update that they have voted.
            await RecordUserHasVoted(userId, card, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to like card {Cid} for user {uid}", cardId, userId);
        }

        return false;
    }

    private async Task<CardEntity?> GetCardAsync(long userId, long cardId, CancellationToken cancellationToken)
    {
        return await _dbContext.Cards
            .FirstOrDefaultAsync(c => c.Id == cardId && c.Set.Members.Any(sm => sm.UserId == userId), cancellationToken);
    }

    private async Task RecordUserHasVoted(long userId, CardEntity card, CancellationToken cancellationToken)
    {
        var setMember = await _dbContext.SetMembers.FirstOrDefaultAsync(sm => sm.SetId == card.SetId && sm.UserId == userId, cancellationToken);
        if (setMember != null)
        {
            setMember.HasVoted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
