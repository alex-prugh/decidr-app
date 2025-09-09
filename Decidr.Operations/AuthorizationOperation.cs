using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface IAuthorizationOperation
{
    /// <summary>
    /// Gets a user by their username and password.
    /// </summary>
    public Task<User?> GetUserAsync(string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Registers a user.
    /// </summary>
    public Task<bool> RegisterUserAsync(string username, string password, string name, string email, CancellationToken cancellationToken = default);
}

public class AuthorizationOperation : IAuthorizationOperation
{
    private readonly IUsersDataProvider _usersDataProvider;

    public AuthorizationOperation(
        IUsersDataProvider usersDataProvider)
    {
        _usersDataProvider = usersDataProvider;
    }

    /// <inheritdoc />
    public async Task<User?> GetUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _usersDataProvider.GetUserByUsernamePasswordAsync(username, password, cancellationToken);
        if (user == null)
        {
            return null;
        }

        return user;
    }

    /// <inheritdoc />
    public async Task<bool> RegisterUserAsync(string username, string password, string name, string email, CancellationToken cancellationToken)
    {
        // TODO: Probably want a better way to notify the client that a user already exists. What if they need to update email?
        var user = await _usersDataProvider.GetUserByUsernamePasswordAsync(username, password, cancellationToken);

        // If a user doesn't exist with that username and password, create one.
        if (user == null)
        {
            user = await _usersDataProvider.CreateAsync(username, password, name, email, cancellationToken);
        }

        return user != null;
    }
}
