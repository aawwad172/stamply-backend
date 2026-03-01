using Stamply.Domain.Common;

namespace Stamply.Domain.Entities.Identity.Authentication;

public class UserRoleTenant
{
    public required Guid Id { get; set; }

    public required Guid UserId { get; set; }
    public User? User { get; set; }

    // Which role
    public required Guid RoleId { get; set; }
    public Role? Role { get; set; }

    // Whome
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}
