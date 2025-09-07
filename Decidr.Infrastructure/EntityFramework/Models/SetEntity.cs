using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Decidr.Operations.BusinessObjects;

namespace Decidr.Infrastructure.EntityFramework.Models;

public class SetEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long CreatorId { get; set; }
    public string? ImageUrl { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;
    public virtual ICollection<CardEntity> Cards { get; set; } = [];
}

public class SetEntityConfiguration : IEntityTypeConfiguration<SetEntity>
{
    public void Configure(EntityTypeBuilder<SetEntity> entity)
    {
        entity.HasKey(x => x.Id).HasName("Sets_pkey");
        entity.ToTable("Sets", "app");
        entity.Property(x => x.Name).HasColumnName("Name");
        entity.Property(x => x.CreatorId).HasColumnName("CreatorId"); // TODO: Make this a FK on Users
        entity.Property(x => x.ImageUrl).HasColumnName("ImageUrl");
        entity.Property(x => x.CreateDate).HasColumnName("CreateDate");
    }
}

public static class SetExtensions
{
    public static Set ToBusinessObject(this SetEntity set)
    {
        return new Set
        {
            Id = set.Id,
            Name = set.Name,
            CreatorName = set.CreatorId.ToString(), // TODO Need to fetch creator name.
            ImageUrl = set.ImageUrl,
            Cards = set.Cards.Select(c => c.ToBusinessObject()).ToList()
        };
    }
}