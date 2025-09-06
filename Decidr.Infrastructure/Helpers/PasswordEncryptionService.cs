using Microsoft.AspNetCore.Identity;

namespace Decidr.Infrastructure.Helpers;

public interface IPasswordEncryptionService
{
    public string HashPassword(string password);
    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
}

public class PasswordEncryptionService : IPasswordEncryptionService
{
    private readonly PasswordHasher<string> _hasher = new();

    public string HashPassword(string password)
    {   
        if (string.IsNullOrEmpty(password)) 
            throw new ArgumentException(nameof(password));

        return _hasher.HashPassword(null!, password);
    }

    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword)) 
            throw new ArgumentException(nameof(hashedPassword));

        if (string.IsNullOrEmpty(providedPassword)) 
            throw new ArgumentException(nameof(providedPassword));

        return _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
    }
}
