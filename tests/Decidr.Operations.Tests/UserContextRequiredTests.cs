using Decidr.Operations.BusinessObjects;
using NSubstitute;
namespace Decidr.Operations.Tests;

public abstract class UserContextRequiredTests
{
    protected UserContext _subUserContext;

    protected UserContextRequiredTests()
    {
        _subUserContext = new UserContext();
        var user = new User
        {
            Id = 1,
            Name = "Test",
            Email = "email@email.com",
            Username = "TestTest"
        };

        _subUserContext.Current = user;
    }
}
