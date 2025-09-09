namespace Decidr.Operations.BusinessObjects;

public class User
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
}
