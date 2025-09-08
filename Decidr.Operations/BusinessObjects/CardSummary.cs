namespace Decidr.Operations.BusinessObjects;

public class CardSummary
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public long Likes { get; set; }
    public long Dislikes { get; set; }
    public bool IsSuggested { get; set; }
}