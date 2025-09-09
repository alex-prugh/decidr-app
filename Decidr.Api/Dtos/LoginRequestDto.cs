using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class LoginRequestDto
{
    [DataMember]
    public required string Username { get; set; }
    [DataMember]
    public required string Password { get; set; }
}
