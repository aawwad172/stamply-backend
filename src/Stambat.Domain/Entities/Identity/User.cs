using System.Security.Cryptography.X509Certificates;

using Stambat.Domain.Common;
using Stambat.Domain.Entities.Identity.Authentication;
using Stambat.Domain.Enums;
using Stambat.Domain.Exceptions;
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
    public ICollection<UserToken> UserTokens { get; private set; } = [];
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

    public void LinkToTenant(Guid tenantId, Guid roleId)
    {
        Guard.AgainstDefault(tenantId, nameof(tenantId));
        Guard.AgainstDefault(roleId, nameof(roleId));

        var existingLink = UserRoleTenants.FirstOrDefault(x => x.TenantId == tenantId && x.RoleId == roleId);

        if (existingLink is null)
        {
            UserRoleTenants.Add(
                UserRoleTenant.Create(
                    IdGenerator.New(),
                    Id,
                    roleId,
                    tenantId)
            );
        }
        else
        {
            throw new ConflictException("User already has this role in this tenant.");
        }
    }

    public void AddRefreshToken(
        string tokenHash,
        string plaintextToken,
        DateTime expiresAt,
        Guid tokenFamilyId)
    {
        Guard.AgainstNullOrEmpty(tokenHash, nameof(tokenHash));
        Guard.AgainstNullOrEmpty(plaintextToken, nameof(plaintextToken));

        var refreshToken = RefreshToken.Create(
            IdGenerator.New(),
            Id,
            tokenFamilyId,
            tokenHash,
            plaintextToken,
            expiresAt,
            Id,
            SecurityStamp
        );

        RefreshTokens.Add(refreshToken);
    }

    public void RevokeRefreshToken(string tokenHash)
    {
        Guard.AgainstNullOrEmpty(tokenHash, nameof(tokenHash));

        var token = RefreshTokens.FirstOrDefault(rt => rt.TokenHash == tokenHash && !rt.IsRevoked);
        token?.Revoke("Revoked via User domain method");
    }

    public void AddUserToken(UserTokenType type, string value, DateTime expiry)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        var userToken = UserToken.Create(
            IdGenerator.New(),
            Id,
            value,
            type,
            expiry
        );

        UserTokens.Add(userToken);
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
