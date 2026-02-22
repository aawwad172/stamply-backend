using Stamply.Domain.Interfaces.Domain.Auditing;

namespace Stamply.Domain.Entities.Identity.Authentication;

public class Role : IBaseEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    // Human-friendly identifier (e.g., "SuperAdmin", "TenantAdmin", "SubAdmin", "AppUser")
    public required string Name { get; init; }
    public string? Description { get; init; }

    // Navigations
    public ICollection<UserRole> UserRoles { get; init; } = [];
    public ICollection<RolePermission> RolePermissions { get; init; } = [];
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}
