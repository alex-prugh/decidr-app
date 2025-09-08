using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface IUsersDataProvider
{
    public Task<User?> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);
    public Task<User?> GetUserByUsernamePasswordAsync(string username, string password, CancellationToken cancellationToken = default);
    public Task<User?> CreateAsync(string username, string password, string name, string email, CancellationToken cancellationToken = default);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
