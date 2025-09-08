using Decidr.Api.Dtos;
using Decidr.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Decidr.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationOperation _authOperation;
    private readonly IConfiguration _configuration;

    public AuthorizationController(
        IAuthorizationOperation authOperation, 
        IConfiguration configuration)
    {
        _authOperation = authOperation;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _authOperation.RegisterUserAsync(request.Username, request.Password, request.Name, request.Email, cancellationToken);
        return result
            ? Ok(new { message = "User registered successfully" })
            : BadRequest(new { message = "Unable to register successfully. Try again " });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await _authOperation.GetUserAsync(request.Username, request.Password, cancellationToken);
        if (user == null)
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(request.Username, user.Id);
        return Ok(new { token });
    }

    private string GenerateJwtToken(string username, long userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("user_id", userId.ToString()),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // TODO: Figure out how to rotate this.
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}