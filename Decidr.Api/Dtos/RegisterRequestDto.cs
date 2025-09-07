namespace Decidr.Api.Dtos;

public class RegisterRequestDto
{
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
