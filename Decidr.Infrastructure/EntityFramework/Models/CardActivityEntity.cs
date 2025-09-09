using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Decidr.Infrastructure.EntityFramework.Models;

public class CardActivityEntity
{
    public long CardId { get; set; }
    public long UserId { get; set; }
    public bool IsLiked { get; set; }
    public bool IsDisliked { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;

    public virtual CardEntity Card { get; set; } = null!;
    public virtual UserEntity User { get; set; } = null!;
}

public class CardActivityEntityConfiguration : IEntityTypeConfiguration<CardActivityEntity>
{
    public void Configure(EntityTypeBuilder<CardActivityEntity> entity)
    {
        entity.HasKey(x => new { x.CardId, x.UserId }).HasName("CardActivity_pkey");
        entity.ToTable("CardActivities", "app");
        entity.Property(x => x.IsLiked).HasColumnName("IsLiked");
        entity.Property(x => x.IsDisliked).HasColumnName("IsDisliked"); // TODO: Make this a nullable FK on Users
        entity.Property(x => x.CreateDate).HasColumnName("CreateDate");

        entity.HasOne(x => x.Card)
            .WithMany(s => s.Activities)
            .HasForeignKey(x => x.CardId);

        entity.HasOne(x => x.User)
            .WithMany(s => s.CardActivities)
            .HasForeignKey(x => x.UserId);
    }
}
