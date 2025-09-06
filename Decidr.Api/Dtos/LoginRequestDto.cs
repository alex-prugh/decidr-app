using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class LoginRequestDto
{
    [DataMember(Name = "username")]
    public required string Username { get; set; }
    public required string Password { get; set; }
}
