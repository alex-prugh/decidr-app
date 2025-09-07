using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;

namespace Decidr.Operations;

public interface IAuthorizationOperation
{
    public Task<User?> GetUserAsync(string username, string password, CancellationToken cancellationToken = default);
    public Task<bool> RegisterUserAsync(string username, string password, CancellationToken cancellationToken = default);
}

public class AuthorizationOperation : IAuthorizationOperation
{
    private readonly IUsersDataProvider _usersDataProvider;

    public AuthorizationOperation(
        IUsersDataProvider usersDataProvider)
    {
        _usersDataProvider = usersDataProvider;
    }

    public async Task<User?> GetUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _usersDataProvider.GetUserAsync(username, password, cancellationToken);
        if (user == null)
        {
            return null;
        }

        return user;
    }

    public async Task<bool> RegisterUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _usersDataProvider.GetUserAsync(username, password, cancellationToken);

        if (user == null)
        {
            // Create a new user.
            user = await _usersDataProvider.CreateAsync(username, password, cancellationToken);
        }

        return user != null;
    }
}
