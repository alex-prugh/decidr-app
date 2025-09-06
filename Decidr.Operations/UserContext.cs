using Decidr.Operations.BusinessObjects;

namespace Decidr.Operations;

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
