using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Decidr.Infrastructure.EntityFramework;

/// <summary>
/// This was needed in order to use EF migrations. This connection string should be in the appsettings, but the migration
/// doesn't happen during runtime. The appsettings couldn't be read at that time.
/// TODO: Look into how I can avoid hard-coding this here.
/// </summary>
public class DecidrDbContextFactory : IDesignTimeDbContextFactory<DecidrDbContext>
{
    public DecidrDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DecidrDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=decidrdb;Username=postgres;Password=postgres");
        return new DecidrDbContext(optionsBuilder.Options);
    }
}
