namespace Decidr.Operations.BusinessObjects;

public class Set
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CreatorName { get; set; }
    public string? ImageUrl { get; set; }
    public IReadOnlyCollection<Card> Cards { get; set; } = [];
    public bool IsUnread { get; set; }
}
