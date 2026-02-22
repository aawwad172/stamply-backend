using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Stamply.Domain.Common;
using Stamply.Domain.Entities.Identity;
using Stamply.Domain.ValueObjects;
using Stamply.Domain.Entities.Identity.Authentication;
using Stamply.Infrastructure.Configurations.Seed;
using Stamply.Application.Utilities;

namespace Stamply.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider)
    {
        using IServiceScope? scope = serviceProvider.CreateScope();
        IServiceProvider? services = scope.ServiceProvider;

        try
        {
            ApplicationDbContext? context = services.GetRequiredService<ApplicationDbContext>();
            IConfiguration? configuration = services.GetRequiredService<IConfiguration>();

            // 1. Automatically apply any pending migrations
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            // 2. Seed the "System" User and Credentials
            if (!await context.Users.AnyAsync(u => u.Id == AuthSeedConstants.SystemUserId))
            {
                Guid systemUserId = AuthSeedConstants.SystemUserId;
                User? systemUser = new()
                {
                    Id = systemUserId,
                    FullName = new FullName { FirstName = "system", LastName = "system" },
                    Username = "system",
                    Email = "system@example.com",
                    SecurityStamp = AuthSeedConstants.SystemSecurityStampGuid,
                    IsActive = true,
                    IsVerified = true,
                    IsDeleted = false,
                    CreatedAt = AuthSeedConstants.SeedDateUtc,
                    CreatedBy = systemUserId,
                    Credentials = new UserCredentials
                    {
                        Id = Id.New(),
                        UserId = systemUserId,
                        PasswordHash = configuration.GetRequiredSetting("Security:SystemAdminPasswordHash")
                    }
                };

                context.Users.Add(systemUser);
                
                // Assign SuperAdmin role to system user
                context.UserRoles.Add(new UserRole
                {
                    UserId = systemUserId,
                    RoleId = AuthSeedConstants.RoleIdSuperAdmin
                });

                await context.SaveChangesAsync();
            }
            else
            {
                // Ensure system user has the SuperAdmin role if it was created previously without it
                bool hasRole = await context.UserRoles.AnyAsync(ur => 
                    ur.UserId == AuthSeedConstants.SystemUserId && 
                    ur.RoleId == AuthSeedConstants.RoleIdSuperAdmin);
                
                if (!hasRole)
                {
                    context.UserRoles.Add(new UserRole
                    {
                        UserId = AuthSeedConstants.SystemUserId,
                        RoleId = AuthSeedConstants.RoleIdSuperAdmin
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            // Ideally use a logger here
            Console.WriteLine($"An error occurred during migration/seeding: {ex.Message}");
            throw;
        }
    }
}
