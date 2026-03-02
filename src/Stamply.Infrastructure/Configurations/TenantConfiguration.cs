using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stamply.Domain.Entities;

namespace Stamply.Infrastructure.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        // Business Name
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Unique Slug for URLs (e.g., stamply.app/ahmads-coffee)
        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        // Branding Fields
        builder.Property(t => t.LogoUrl)
            .HasMaxLength(500);

        builder.Property(t => t.PrimaryColor)
            .HasDefaultValue("#000000")
            .HasMaxLength(7);

        builder.Property(t => t.SecondaryColor)
            .HasDefaultValue("#FFFFFF")
            .HasMaxLength(7);

        // Regional Settings
        builder.Property(t => t.TimeZoneId)
            .HasDefaultValue("Asia/Amman")
            .HasMaxLength(50);

        builder.Property(t => t.CurrencyCode)
            .HasDefaultValue("JOD")
            .HasMaxLength(3);

        // Status
        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        // Auditing
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.CreatedBy).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired(false);
        builder.Property(t => t.UpdatedBy).IsRequired(false);
        builder.Property(t => t.IsDeleted).HasDefaultValue(false);

        // Relationships
        // The many-to-many relationship is already configured in UserRoleTenantConfiguration,
        // but we define the CardTemplate one-to-many here.
        // builder.HasMany(t => t.CardTemplates)
        //     .WithOne(ct => ct.Tenant)
        //     .HasForeignKey(ct => ct.TenantId)
        //     .OnDelete(DeleteBehavior.Restrict);
        // Restrict prevents deleting a Tenant if they have active templates
    }
}
