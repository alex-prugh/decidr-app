using Decidr.Infrastructure.EntityFramework;
using Decidr.Infrastructure.EntityFramework.Models;
using Decidr.Infrastructure.EntityFramework.Views;
using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Decidr.Infrastructure.Sets;

public class SetsDataProvider : ISetsDataProvider
{
    private readonly DecidrDbContext _dbContext;
    private readonly ILogger<SetsDataProvider> _logger;

    public SetsDataProvider(
        DecidrDbContext dbContext, 
        ILogger<SetsDataProvider> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> AddMemberAsync(long setId, User user, long addedByUserId, CancellationToken cancellationToken)
    {
        try
        {
            var set = await _dbContext.Sets.Where(s => s.Id == setId).FirstOrDefaultAsync(cancellationToken);
            if (set == null)
            {
                return false;
            }

            var newMember = new SetMemberEntity
            {
                SetId = set.Id,
                UserId = user.Id,
                HasVoted = false,
                AddedById = addedByUserId,
            };

            _dbContext.Add(newMember);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ran into error when adding a member to a set. Set id: {Sid} / New member user id: {Uid} / Added by user id: {AddedByUid}", setId, user.Id, addedByUserId);
        }

        return false;

    }

    // <inheritdoc />
    public async Task<Set?> CreateAsync(string name, ICollection<Card> cards, long creatorUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cardEntities = cards.Select(c => new CardEntity
            {
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
            });

            var newSet = new SetEntity
            {
                Name = name,
                Cards = cardEntities.ToList(),
                CreatorId = creatorUserId,
                ImageUrl = cardEntities
                    .Where(c => c.ImageUrl != null)
                    .OrderBy(c => Guid.NewGuid())
                    .Select(c => c.ImageUrl)
                    .FirstOrDefault(),
                Members = new List<SetMemberEntity>
                {
                    new SetMemberEntity
                    {
                        UserId = creatorUserId,
                    }
                }
            };

            _dbContext.Add(newSet);
            await _dbContext.SaveChangesAsync();

            return newSet.ToBusinessObject();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ran into error when creating a new set. Creator user id: {Uid}", creatorUserId);
        }

        return null;
    }

    // <inheritdoc />
    public async Task<Set?> Get(long setId, long userId, bool includeCards = false, CancellationToken cancellationToken = default)
    {
        var setEntity = await QuerySetsForUser(userId, includeCards, setId)
            .FirstOrDefaultAsync(cancellationToken);

        return setEntity?.ToBusinessObject(userId);
    }

    // <inheritdoc />
    public async Task<List<Set>> GetAllForUser(long userId, bool includeCards = false, CancellationToken cancellationToken = default)
    {
        var setEntities = await QuerySetsForUser(userId, includeCards)
            .ToListAsync(cancellationToken);

        return setEntities.Select(s => s.ToBusinessObject(userId)).ToList();
    }

    // <inheritdoc />
    public async Task<List<CardSummary>> GetCardActivities(long setId, long userId, CancellationToken cancellationToken = default)
    {
        var hasAccessToSet = await _dbContext.SetMembers.AnyAsync(sm => sm.SetId == setId && sm.UserId == userId, cancellationToken);
        if (!hasAccessToSet)
        {
            return [];
        }

        var cardSummaries = await _dbContext.Cards
        .Where(c => c.SetId == setId)
        .Select(c => new CardSummary
        {
            Id = c.Id,
            Title = c.Title,
            ImageUrl = c.ImageUrl,
            Likes = c.Activities.Count(a => a.IsLiked),
            Dislikes = c.Activities.Count(a => a.IsDisliked)
        })
        .ToListAsync(cancellationToken);

        return cardSummaries;
    }

    private IQueryable<SetWithUnreadInfo> QuerySetsForUser(long userId, bool includeCards = false, long? setId = null)
    {
        var query = _dbContext.SetMembers
            .Where(sm => (setId == null || sm.SetId == setId) && sm.UserId == userId);

        // Avoid loading cards if they're not needed.
        if (includeCards)
        {
            query = query
                .Include(sm => sm.Set)
                    .ThenInclude(s => s.Cards)
                        .ThenInclude(c => c.Activities);
        }
        else
        {
            query = query
                .Include(sm => sm.Set);
        }

        return query.Select(sm => new SetWithUnreadInfo
        {
            Set = sm.Set,
            HasVoted = sm.HasVoted
        });
    }
}
