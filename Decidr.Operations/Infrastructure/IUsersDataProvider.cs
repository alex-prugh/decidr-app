using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface IUsersDataProvider
{
    public Task<User?> GetUserAsync(string username, string password, CancellationToken cancellationToken = default);
    public Task<User?> CreateAsync(string username, string password, CancellationToken cancellationToken = default);
}
