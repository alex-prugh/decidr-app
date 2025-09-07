using Decidr.Infrastructure.EntityFramework;
using Decidr.Infrastructure.EntityFramework.Models;
using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
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
                ImageUrl = cardEntities.FirstOrDefault(c => c.ImageUrl != null)?.ImageUrl
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
}
