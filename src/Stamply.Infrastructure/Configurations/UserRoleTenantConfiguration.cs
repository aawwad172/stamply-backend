using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stamply.Domain.Entities.Identity.Authentication;

namespace Stamply.Infrastructure.Configurations;

public class UserRoleTenantConfiguration : IEntityTypeConfiguration<UserRoleTenant>
{
    public void Configure(EntityTypeBuilder<UserRoleTenant> builder)
    {
        builder.HasKey(urt => urt.Id); // Use the surrogate ID

        builder.HasOne(urt => urt.User)
            .WithMany(u => u.UserRoleTenants)
            .HasForeignKey(urt => urt.UserId);

        builder.HasOne(urt => urt.Role)
            .WithMany(urt => urt.UserRoleTenants)
            .HasForeignKey(urt => urt.RoleId);

        builder.HasOne(urt => urt.Tenant)
            .WithMany(t => t.UserRoleTenants)
            .HasForeignKey(urt => urt.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint to prevent a user from having the same role 
        // in the same tenant twice
        builder.HasIndex(urt => new { urt.UserId, urt.RoleId, urt.TenantId })
                .IsUnique();
    }
}
