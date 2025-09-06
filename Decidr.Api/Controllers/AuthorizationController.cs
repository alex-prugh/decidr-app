using Decidr.Api.Dtos;
using Decidr.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Decidr.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationOperation _authOperation;

    public AuthorizationController(
        IAuthorizationOperation authOperation)
    {
        _authOperation = authOperation;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _authOperation.RegisterUserAsync(request.Username, request.Password, cancellationToken);
        return result
            ? Ok(new { message = "User registered successfully" })
            : BadRequest(new { message = "Unable to register successfully. Try again " });
    }

    // TODO: Set up JWT auth.
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var success = await _authOperation.IsAuthorizedAsync(request.Username, request.Password, cancellationToken);
        return success
            ? Ok(new { message = "Login successful." })
            : Unauthorized();
    }
}