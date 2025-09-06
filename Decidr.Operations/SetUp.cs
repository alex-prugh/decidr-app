using Microsoft.Extensions.DependencyInjection;

namespace Decidr.Operations;
public static class SetUp
{
    public static IServiceCollection AddOperations(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationOperation, AuthorizationOperation>();
        services.AddScoped<UserContext>();
        return services;
    }
}
