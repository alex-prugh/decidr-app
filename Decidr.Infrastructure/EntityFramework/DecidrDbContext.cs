using Decidr.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Decidr.Infrastructure.EntityFramework;

[DebuggerDisplay("WriteContext")]
public class DecidrDbContext : DbContext
{
    public DecidrDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    internal virtual DbSet<UserEntity> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityConfiguration).Assembly);
    }
}
