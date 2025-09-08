using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class RegisterRequestDto
{
    [DataMember]
    public required string Name { get; set; }
    [DataMember]
    public required string Username { get; set; }
    [DataMember]
    public required string Password { get; set; }
    [DataMember]
    public required string Email { get; set; }
}
