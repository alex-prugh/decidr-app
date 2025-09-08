using Decidr.Infrastructure.EntityFramework;
using Decidr.Infrastructure.EntityFramework.Models;
using Decidr.Infrastructure.Helpers;
using Decidr.Operations.BusinessObjects;
using Decidr.Operations.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Decidr.Infrastructure.Users;

public class UsersDataProvider : IUsersDataProvider
{
    private readonly DecidrDbContext _dbContext;
    private readonly IPasswordEncryptionService _passwordEncryptionService;
    private readonly ILogger<UsersDataProvider> _logger;

    public UsersDataProvider(
        DecidrDbContext dbContext,
        IPasswordEncryptionService passwordEncryptionService,
        ILogger<UsersDataProvider> logger)
    {
        _dbContext = dbContext;
        _passwordEncryptionService = passwordEncryptionService;
        _logger = logger;
    }

    public async Task<User?> CreateAsync(string username, string password, string name, string email, CancellationToken cancellationToken)
    {
        var user = await GetUserByUsernamePasswordAsync(username, password, cancellationToken);
        if (user != null)
        {
            return user;
        }

        try
        {
            var hashedPassword = _passwordEncryptionService.HashPassword(password);
            var newUser = new UserEntity
            {
                Username = username,
                PasswordHash = hashedPassword,
                Email = email,
                Name = name,
            };

            _dbContext.Add(newUser);
            await _dbContext.SaveChangesAsync();
            user = newUser.ToBusinessObject();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ran into error when creating new user. Attempted username: {UserName}", username);
        }

        return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var dbUser = await _dbContext.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);

        return dbUser?.ToBusinessObject();
    }

    public async Task<User?> GetUserByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var dbUser = await _dbContext.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        return dbUser?.ToBusinessObject();
    }

    public async Task<User?> GetUserByUsernamePasswordAsync(string username, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = _passwordEncryptionService.HashPassword(password);
        var dbUser = await _dbContext.Users
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync(cancellationToken);

        if (dbUser == null)
        {
            return null;
        }

        var hashResult = _passwordEncryptionService.VerifyHashedPassword(dbUser.PasswordHash, password);
        if (hashResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return dbUser.ToBusinessObject();
    }
}
