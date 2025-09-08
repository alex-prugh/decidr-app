using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations;

/// <summary>
/// Created once per client request to track the logged-in user's info (if applicable).
/// </summary>
public class UserContext
{
    public User? Current { get; set; }

    public User GetUserOrThrow()
    {
        if (Current == null)
        {
            throw new ArgumentNullException("No current user");
        }

        return Current;
    }
}
