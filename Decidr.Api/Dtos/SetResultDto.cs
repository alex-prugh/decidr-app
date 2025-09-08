using Decidr.Operations.BusinessObjects;

namespace Decidr.Api.Dtos;

public class SetResultDto
{
    public long Id { get; set; }
    public string? Name { get; set; }

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
