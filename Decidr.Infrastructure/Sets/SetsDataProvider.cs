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
                ImageUrl = cardEntities.FirstOrDefault(c => c.ImageUrl != null)?.ImageUrl,
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

    public async Task<Set?> Get(long setId, long userId, CancellationToken cancellationToken = default)
    {
        var setEntity = await QuerySetsForUser(userId, setId)
            .FirstOrDefaultAsync(cancellationToken);

        return setEntity?.ToBusinessObject();
    }

    public async Task<List<Set>> GetAllForUser(long userId, CancellationToken cancellationToken = default)
    {
        var setEntities = await QuerySetsForUser(userId)
            .ToListAsync(cancellationToken);

        return setEntities.Select(s => s.ToBusinessObject()).ToList();
    }

    private IQueryable<SetWithUnreadInfo> QuerySetsForUser(long userId, long? setId = null)
    {
        return _dbContext.SetMembers
                    .Where(sm => (setId == null || sm.SetId == setId) && sm.UserId == userId)
                    .Include(sm => sm.Set)
                    .ThenInclude(s => s.Cards)
                    .Select(sm => new SetWithUnreadInfo {
                       Set = sm.Set,
                       IsUnread = sm.IsUnread
                    });
    }
}
