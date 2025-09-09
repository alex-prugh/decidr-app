using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations.Infrastructure;

public interface IUsersDataProvider
{
    /// <summary>
    /// Gets a user by id.
    /// </summary>
    public Task<User?> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by username and password.
    /// </summary>
    public Task<User?> GetUserByUsernamePasswordAsync(string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    public Task<User?> CreateAsync(string username, string password, string name, string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their email.
    /// </summary>
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
