using Decidr.Operations.BusinessObjects;
using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class CardSummaryDto
{
    [DataMember]
    public string? Title { get; set; }
    [DataMember]
    public string? ImageUrl { get; set; }
    [DataMember]
    public long Likes { get; set; }
    [DataMember]
    public long Dislikes { get; set; }
    [DataMember]
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