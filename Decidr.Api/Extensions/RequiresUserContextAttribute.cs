using Decidr.Operations;
using Decidr.Operations.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Decidr.Api.Extensions;

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
        var userId = context.HttpContext.User.FindFirst("user_id")?.Value ?? throw new InvalidOperationException("No user id in bearer");
        var userContext = context.HttpContext.RequestServices.GetRequiredService<UserContext>();
        var usersDataProvider = context.HttpContext.RequestServices.GetRequiredService<IUsersDataProvider>();
        var user = await usersDataProvider.GetUserByIdAsync(int.Parse(userId));
        userContext.Current = user ?? throw new InvalidOperationException($"Unable to find user associated with user id in bearer token: {userId}");
        await next();
    }
}