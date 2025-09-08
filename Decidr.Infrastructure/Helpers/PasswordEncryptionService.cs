using Microsoft.AspNetCore.Identity;

namespace Decidr.Infrastructure.Helpers;

/// <summary>
/// Helps with password encryption.
/// </summary>
public interface IPasswordEncryptionService
{
    /// <summary>
    /// Hash a given password.
    /// </summary>
    public string HashPassword(string password);

    /// <summary>
    /// Determines whether the password provided matches the hashed password from the db.
    /// </summary>
    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
}

public class PasswordEncryptionService : IPasswordEncryptionService
{
    private readonly PasswordHasher<string> _hasher = new();

    /// <inheritdoc />
    public string HashPassword(string password)
    {   
        ArgumentNullException.ThrowIfNull(password);
        return _hasher.HashPassword(null!, password);
    }

    /// <inheritdoc />
    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(providedPassword);

        return _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
    }
}
