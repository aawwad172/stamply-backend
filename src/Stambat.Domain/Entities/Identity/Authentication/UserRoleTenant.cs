using Stambat.Domain.Interfaces.Domain;

namespace Stambat.Domain.Entities.Identity.Authentication;

public class UserRoleTenant : IEntity
{
    public required Guid Id { get; init; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    // Which role
    public Guid RoleId { get; private set; }
    public Role? Role { get; private set; }

    // Whome
    public Guid? TenantId { get; private set; }
    public Tenant? Tenant { get; private set; }

    // EF Core constructor
    private UserRoleTenant() { }

    // Static factory for creating the link
    public static UserRoleTenant Create(Guid id, Guid userId, Guid roleId, Guid? tenantId = null)
    {
        return new UserRoleTenant
        {
            Id = id,
            UserId = userId,
            RoleId = roleId,
            TenantId = tenantId
        };
    }
}
