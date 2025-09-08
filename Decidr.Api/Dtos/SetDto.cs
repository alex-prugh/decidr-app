using Decidr.Operations.BusinessObjects;
using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class SetDto
{
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string Name { get; set; } = string.Empty;
    [DataMember]
    public string? CreatorName { get; set; }
    [DataMember]
    public string? ImageUrl { get; set; }
    [DataMember]
    public IReadOnlyCollection<CardDto> Cards { get; set; } = [];
    [DataMember]
    public bool HasVoted { get; set; }
}

public static class SetExtensions
{
    public static SetDto ToDto(this Set set)
    {
        return new SetDto
        {
            Id = set.Id,
            Name = set.Name,
            CreatorName = set.CreatorName,
            ImageUrl = set.ImageUrl,
            Cards = set.Cards.Select(c => c.ToDto()).ToList(),
            HasVoted = set.HasVoted
        };
    }
}
