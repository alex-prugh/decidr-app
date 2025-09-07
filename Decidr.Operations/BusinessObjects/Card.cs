namespace Decidr.Operations.BusinessObjects;

public class Card
{
    public long Id { get; set; }
    public long SetId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
