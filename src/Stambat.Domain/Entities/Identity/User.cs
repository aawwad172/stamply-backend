using Stambat.Domain.Common;
using Stambat.Domain.Entities.Identity.Authentication;
using Stambat.Domain.Interfaces.Domain.Auditing;
using Stambat.Domain.ValueObjects;

namespace Stambat.Domain.Entities.Identity;

public class User : IBaseEntity
{
    // Properties
    public required Guid Id { get; init; }
    public FullName FullName { get; private set; }
    public string Username { get; private set; }
    public Email Email { get; private set; }
    public Guid? UserCredentialsId { get; private set; }
    public string SecurityStamp { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; set; }
    public bool IsVerified { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Navigation properties
    public virtual UserCredentials? Credentials { get; private set; }
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];
    public ICollection<UserRoleTenant> UserRoleTenants { get; private set; } = [];
    public ICollection<WalletPass> WalletPasses { get; private set; } = [];

    // EF Core constructor
    private User()
    {
        // Required for EF Core
        FullName = default!;
        Username = default!;
        Email = default!;
        SecurityStamp = default!;
    }

    public void SetCredentials(UserCredentials credentials)
    {
        Credentials = credentials;
        UserCredentialsId = credentials.Id;
    }

    public void UpdateSecurityStamp()
    {
        SecurityStamp = Guid.CreateVersion7().ToString();
    }

    public void VerifyEmail()
    {
        IsVerified = true;
        UpdateSecurityStamp();
    }

    // Static factory for user registration/creation
    public static User Create(
        Guid id,
        FullName fullName,
        string username,
        Email email,
        string securityStamp,
        Guid createdBy,
        bool isVerified = false)
    {
        Guard.AgainstDefault(id, nameof(id));
        Guard.AgainstNullOrEmpty(username, nameof(username));
        Guard.AgainstNull(fullName, nameof(fullName));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNullOrEmpty(securityStamp, nameof(securityStamp));
        Guard.AgainstDefault(createdBy, nameof(createdBy));

        return new User
        {
            Id = id,
            FullName = fullName,
            Username = username,
            Email = email,
            SecurityStamp = securityStamp,
            IsActive = true,
            IsDeleted = false,
            IsVerified = isVerified,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }
}
