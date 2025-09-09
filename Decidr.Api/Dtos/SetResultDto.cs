using Decidr.Operations.BusinessObjects;
using System.Runtime.Serialization;

namespace Decidr.Api.Dtos;

[DataContract]
public class SetResultDto
{
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string? Name { get; set; }
    [DataMember]
    public List<CardSummaryDto> CardSummaries { get; set; } = [];
}

public static class SetResultExtensions
{
    public static SetResultDto ToDto(this SetResult setResult)
    {
        return new SetResultDto
        {
            Id = setResult.Id,
            CardSummaries = setResult.CardSummaries.Select(cs => cs.ToDto()).ToList()
        };
    }
}
