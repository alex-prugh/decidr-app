using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface ISetsOperation
{
    public Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken = default);
    public Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken = default);
}

public class SetsOperation : ISetsOperation
{
    private readonly ISetsDataProvider _setsDataProvider;
    private readonly UserContext _userContext;

    public SetsOperation(ISetsDataProvider setsDataProvider, UserContext userContext)
    {
        _setsDataProvider = setsDataProvider;
        _userContext = userContext;
    }

    public async Task<List<Set>> GetAllSetsAsync(CancellationToken cancellationToken = default)
    {
        var sets = await _setsDataProvider.GetAllForUser(_userContext.GetUserOrThrow().Id, cancellationToken);
        return sets;
    }

    public async Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken = default)
    {
        var set = await _setsDataProvider.Get(setId, _userContext.GetUserOrThrow().Id, cancellationToken);
        return set;
    }
}
