using Decidr.Operations.BusinessObjects;

namespace Decidr.Api.Dtos;

public class CardSummaryDto
{
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public long Likes { get; set; }
    public long Dislikes { get; set; }
    public bool IsSuggested { get; set; }
}

public static class CardSummaryExtensions
{
    public static CardSummaryDto ToDto(this CardSummary cardSummary)
    {
        return new CardSummaryDto
        {
            Title = cardSummary.Title,
            ImageUrl = cardSummary.ImageUrl,
            Likes = cardSummary.Likes,
            Dislikes = cardSummary.Dislikes,
            IsSuggested = cardSummary.IsSuggested
        };
    }
}