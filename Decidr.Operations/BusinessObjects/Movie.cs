namespace Decidr.Operations.BusinessObjects;

public class Movie
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Popularity { get; set; }
    public string? ImageUrl { get; set; }
    public DateTimeOffset? ReleaseDate { get; set; }
}