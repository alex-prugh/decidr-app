using Decidr.Infrastructure.EntityFramework.Models;

namespace Decidr.Infrastructure.EntityFramework.Views;

public class SetWithUnreadInfo
{
    public required SetEntity Set { get; set; }
    public bool IsUnread { get; set; }
}
