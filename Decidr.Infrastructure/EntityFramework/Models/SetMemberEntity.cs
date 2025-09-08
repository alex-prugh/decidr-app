using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Decidr.Infrastructure.EntityFramework.Models;

public class SetMemberEntity
{
    public long SetId { get; set; }
    public long UserId { get; set; }
    public bool IsUnread { get; set; }
    public long? AddedById { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;

    public virtual SetEntity Set { get; set; } = null!;
    public virtual UserEntity User { get; set; } = null!;
}

public class SetMemberEntityConfiguration : IEntityTypeConfiguration<SetMemberEntity>
{
    public void Configure(EntityTypeBuilder<SetMemberEntity> entity)
    {
        entity.HasKey(x => new { x.SetId, x.UserId }).HasName("SetMembers_pkey");
        entity.ToTable("SetMembers", "app");
        entity.Property(x => x.IsUnread).HasColumnName("IsUnread");
        entity.Property(x => x.AddedById).HasColumnName("AddedById"); // TODO: Make this a nullable FK on Users
        entity.Property(x => x.CreateDate).HasColumnName("CreateDate");

        entity.HasOne(x => x.Set)
            .WithMany(s => s.Members)
            .HasForeignKey(x => x.SetId);

        entity.HasOne(x => x.User)
            .WithMany(s => s.SetMembers)
            .HasForeignKey(x => x.UserId);
    }
}
