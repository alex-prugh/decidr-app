using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class AddMemberRequestDto
{
    [DataMember]
    public required string Email { get; set; }
}
