using Decidr.Operations.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decidr.Infrastructure.EntityFramework.Models;

public class UserEntity
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;

    public virtual ICollection<SetMemberEntity> SetMembers { get; set; } = [];
    public virtual ICollection<CardActivityEntity> CardActivities { get; set; } = [];
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
        entity.Property(x => x.Email).HasColumnName("Email");
        entity.Property(x => x.CreateDate).HasColumnName("CreateDate");
    }
}

public static class UserExtensions
{
    public static User ToBusinessObject(this UserEntity userEntity)
    {
        return new User
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
            Name = userEntity.Name,
            Email = userEntity.Email
        };
    }
}