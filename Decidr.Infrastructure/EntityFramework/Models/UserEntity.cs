using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decidr.Infrastructure.EntityFramework.Models;

public class UserEntity
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;
}

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> entity)
    {
        entity.HasKey(x => x.Id).HasName("Users_pkey");
        entity.ToTable("Users", "app");
        entity.Property(x => x.Username).HasColumnName("Username");
        entity.Property(x => x.PasswordHash).HasColumnName("PasswordHash");
        entity.Property(x => x.Name).HasColumnName("Name");
        entity.Property(x => x.CreateDate).HasColumnName("CreateDate");
    }
}
