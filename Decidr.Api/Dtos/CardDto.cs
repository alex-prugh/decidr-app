using Decidr.Operations.BusinessObjects;

namespace Decidr.Api.Dtos;

public class CardDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}

public static class CardExtensions
{
    public static CardDto ToDto(this Card card)
    {
        return new CardDto
        {
            Id = card.Id,
            Title = card.Title,
            Description = card.Description,
            ImageUrl = card.ImageUrl,
        };
    }
}
