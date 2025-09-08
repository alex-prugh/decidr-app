using Decidr.Operations.BusinessObjects;
using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class CardDto
{
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string? Title { get; set; }
    [DataMember]
    public string? Description { get; set; }
    [DataMember]
    public string? ImageUrl { get; set; }
    [DataMember]
    public bool IsLiked { get; set; }
    [DataMember]
    public bool IsDisliked { get; set; }
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
            IsLiked = card.IsLiked,
            IsDisliked = card.IsDisliked
        };
    }
}
