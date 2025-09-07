using Decidr.Operations.BusinessObjects;

namespace Decidr.Api.Dtos;

public class SetDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CreatorName { get; set; }
    public string? ImageUrl { get; set; }
    public IReadOnlyCollection<CardDto> Cards { get; set; } = [];
    public bool IsUnread { get; set; }
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
            IsUnread = set.IsUnread
        };
    }
}
