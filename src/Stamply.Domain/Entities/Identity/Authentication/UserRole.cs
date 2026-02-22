namespace Stamply.Domain.Entities.Identity.Authentication;

public class UserRole
{
    public required Guid UserId { get; set; }
    public User? User { get; set; }

    // Which role
    public required Guid RoleId { get; set; }
    public Role? Role { get; set; }
}
