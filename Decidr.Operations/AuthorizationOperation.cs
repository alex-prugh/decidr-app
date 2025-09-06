using Decidr.Operations.Infrastructure;
using System.Threading.Tasks;

namespace Decidr.Operations;

public interface IAuthorizationOperation
{
    public Task<bool> IsAuthorizedAsync(string username, string password, CancellationToken cancellationToken = default);
    public Task<bool> RegisterUserAsync(string username, string password, CancellationToken cancellationToken = default);
}

public class AuthorizationOperation : IAuthorizationOperation
{
    private readonly IUsersDataProvider _usersDataProvider;
    private readonly UserContext _userContext;

    public AuthorizationOperation(
        IUsersDataProvider usersDataProvider,
        UserContext userContext)
    {
        _usersDataProvider = usersDataProvider;
        _userContext = userContext;
    }

    // TODO: Add some sort of JWT checking.
    public async Task<bool> IsAuthorizedAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _usersDataProvider.GetUserAsync(username, password, cancellationToken);
        if (user == null)
        {
            return false;
        }

        _userContext.Current = user;
        return true;
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
