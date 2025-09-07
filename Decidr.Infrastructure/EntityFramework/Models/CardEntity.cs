using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Decidr.Infrastructure.EntityFramework.Models;

public class CardEntity
{
    public long Id { get; set; }
    public long SetId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;
    public virtual SetEntity Set { get; set; } = null!;
}

public class CardEntityConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> entity)
    {
        entity.HasKey(x => x.Id).HasName("Cards_pkey");
        entity.ToTable("Cards", "app");
        entity.Property(x => x.SetId).HasColumnName("SetId");
        entity.Property(x => x.Title).HasColumnName("Title");
        entity.Property(x => x.Description).HasColumnName("Description");
        entity.Property(x => x.ImageUrl).HasColumnName("ImageUrl");
        entity.Property(x => x.CreateDate).HasColumnName("CreateDate");

        entity.HasOne(c => c.Set)
            .WithMany(s => s.Cards)
            .HasForeignKey(c => c.SetId);
    }
}
