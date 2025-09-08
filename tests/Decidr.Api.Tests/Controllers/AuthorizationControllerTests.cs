using Decidr.Api.Controllers;
using Decidr.Api.Dtos;
using Decidr.Operations;
using Decidr.Operations.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Decidr.Api.Tests.Controllers;

public class AuthorizationControllerTests
{
    private IAuthorizationOperation _subAuthOperation = null!;
    private IConfiguration _subConfiguration = null!;

    private AuthorizationController CreateController()
    {
        _subAuthOperation = Substitute.For<IAuthorizationOperation>();
        _subConfiguration = Substitute.For<IConfiguration>();

        return new AuthorizationController(_subAuthOperation, _subConfiguration);
    }

    [Fact]
    public async Task RegisterAsync_WithValidRequest_ReturnsOk()
    {
        var controller = CreateController();
        var request = new RegisterRequestDto
        {
            Username = "testuser",
            Password = "Password123",
            Name = "Test User",
            Email = "test@example.com"
        };

        _subAuthOperation.RegisterUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await controller.RegisterAsync(request);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RegisterAsync_WithFailedRegistration_ReturnsBadRequest()
    {
        var controller = CreateController();
        var request = new RegisterRequestDto
        {
            Username = "existinguser",
            Password = "Password123",
            Name = "Existing User",
            Email = "existing@example.com"
        };

        _subAuthOperation.RegisterUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await controller.RegisterAsync(request);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsOkWithToken()
    {
        var controller = CreateController();
        var request = new LoginRequestDto
        {
            Username = "testuser",
            Password = "Password123"
        };
        var user = new User { Id = 1, Username = "testuser" };

        _subAuthOperation.GetUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _subConfiguration["Jwt:Key"].Returns("test-test-test-test-test-test-test-test-test");

        var result = await controller.LoginAsync(request);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var controller = CreateController();
        var request = new LoginRequestDto
        {
            Username = "invaliduser",
            Password = "wrongpassword"
        };

        _subAuthOperation.GetUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var result = await controller.LoginAsync(request);
        Assert.IsType<UnauthorizedResult>(result);
    }
}

