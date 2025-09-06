using Decidr.Infrastructure.EntityFramework;
using Decidr.Infrastructure.Helpers;
using Decidr.Infrastructure.Users;
using Decidr.Operations.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Decidr.Infrastructure;

public static class SetUp
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordEncryptionService, PasswordEncryptionService>();
        services.AddScoped<IUsersDataProvider, UsersDataProvider>();
        services.AddDbContext<DecidrDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DecidrDb");
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
