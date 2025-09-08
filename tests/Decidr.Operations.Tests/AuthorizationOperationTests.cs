using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using NSubstitute;

namespace Decidr.Operations.Tests;

public class AuthorizationOperationTests
{
    private IUsersDataProvider _subUsersDataProvider = null!;

    private AuthorizationOperation CreateOperation()
    {
        _subUsersDataProvider = Substitute.For<IUsersDataProvider>();
        return new AuthorizationOperation(_subUsersDataProvider);
    }

    [Fact]
    public async Task GetUserAsync_WithValidCredentials_ReturnsUser()
    {
        var operation = CreateOperation();
        var user = new User { Id = 1, Username = "testuser" };

        _subUsersDataProvider.GetUserByUsernamePasswordAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(user);

        var result = await operation.GetUserAsync("testuser", "password", default);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserAsync_WithInvalidCredentials_ReturnsNull()
    {
        var operation = CreateOperation();

        _subUsersDataProvider.GetUserByUsernamePasswordAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var result = await operation.GetUserAsync("invaliduser", "wrongpassword", default);

        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterUserAsync_WithNewUser_ReturnsTrue()
    {
        var operation = CreateOperation();

        _subUsersDataProvider.GetUserByUsernamePasswordAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);
        _subUsersDataProvider.CreateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new User { Id = 1, Username = "newuser" });

        var result = await operation.RegisterUserAsync("newuser", "password", "New User", "new@example.com", default);

        Assert.True(result);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingUser_ReturnsTrue()
    {
        var operation = CreateOperation();
        var existingUser = new User { Id = 1, Username = "existinguser" };

        _subUsersDataProvider.GetUserByUsernamePasswordAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(existingUser);

        var result = await operation.RegisterUserAsync("existinguser", "password", "Existing User", "existing@example.com", default);

        Assert.True(result);
    }
}

