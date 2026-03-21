using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Stambat.Application.Utilities;
using Stambat.Domain.Common;
using Stambat.Domain.Entities.Identity;
using Stambat.Domain.Entities.Identity.Authentication;
using Stambat.Domain.Interfaces.Application.Services;
using Stambat.Domain.ValueObjects;
using Stambat.Infrastructure.Configurations.Seed;

namespace Stambat.Infrastructure.Persistence;

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

            ISecurityService securityService = services.GetRequiredService<ISecurityService>();

            // 1. Automatically apply any pending migrations
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            // 2. Seed the "System" User and Credentials
            if (!await context.Users.AnyAsync(u => u.Id == AuthSeedConstants.SystemUserId))
            {
                Guid systemUserId = AuthSeedConstants.SystemUserId;
                Guid credentialsId = IdGenerator.New();

                User? systemUser = User.Create(
                    systemUserId,
                    FullName.Create("system", "system"),
                    "system",
                    Stambat.Domain.ValueObjects.Email.Create("system@example.com"),
                    AuthSeedConstants.SystemSecurityStampGuid,
                    systemUserId,
                    isVerified: true);

                systemUser.CreatedAt = AuthSeedConstants.SeedDateUtc;

                UserCredentials credentials = UserCredentials.Create(
                    credentialsId,
                    systemUserId,
                    securityService.HashSecret(configuration.GetRequiredSetting("Security:SuperAdminPassword"))
                );

                systemUser.SetCredentials(credentials);

                context.Users.Add(systemUser);

                // Assign SuperAdmin role to system user
                context.UserRoleTenants.Add(UserRoleTenant.Create(
                    IdGenerator.New(),
                    systemUserId,
                    AuthSeedConstants.RoleIdSuperAdmin
                ));

                await context.SaveChangesAsync();
            }
            else
            {
                // Ensure system user has the SuperAdmin role if it was created previously without it
                bool hasRole = await context.UserRoleTenants.AnyAsync(ur =>
                    ur.UserId == AuthSeedConstants.SystemUserId &&
                    ur.RoleId == AuthSeedConstants.RoleIdSuperAdmin);

                if (!hasRole)
                {
                    context.UserRoleTenants.Add(UserRoleTenant.Create(
                        IdGenerator.New(),
                        AuthSeedConstants.SystemUserId,
                        AuthSeedConstants.RoleIdSuperAdmin
                    ));
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
