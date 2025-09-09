using Decidr.Api.Controllers;
using Decidr.Operations;
using Decidr.Operations.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Decidr.Api.Extensions;

/// <summary>
/// An attribute to put over any operation or method where we require the logged-in user's
/// information to be available in the UserContext.
/// </summary>
public class RequiresUserContextAttribute : TypeFilterAttribute
{
    public RequiresUserContextAttribute() : base(typeof(RequiresUserFilter))
    {
    }
}

public class RequiresUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Grab the user id from the bearer.
        var userId = context.HttpContext.User.FindFirst(AuthorizationController.UserIdClaim)?.Value ?? throw new InvalidOperationException("No user id in bearer");
        var userContext = context.HttpContext.RequestServices.GetRequiredService<UserContext>();

        // Get the user from the db and hydrate the UserContext.Current with that user.
        var usersDataProvider = context.HttpContext.RequestServices.GetRequiredService<IUsersDataProvider>();
        var user = await usersDataProvider.GetUserByIdAsync(int.Parse(userId));
        userContext.Current = user ?? throw new InvalidOperationException($"Unable to find user associated with user id in bearer token: {userId}");
        await next();
    }
}