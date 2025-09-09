namespace Decidr.Operations.BusinessObjects;

public class SetResult
{
    public long Id { get; set; }

    public List<CardSummary> CardSummaries { get; set; } = [];
}
