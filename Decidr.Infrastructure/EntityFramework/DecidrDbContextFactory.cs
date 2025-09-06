using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Decidr.Infrastructure.EntityFramework;

public class DecidrDbContextFactory : IDesignTimeDbContextFactory<DecidrDbContext>
{
    public DecidrDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DecidrDbContext>();

        // Use your Postgres connection string here
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=decidrdb;Username=postgres;Password=postgres");

        return new DecidrDbContext(optionsBuilder.Options);
    }
}
