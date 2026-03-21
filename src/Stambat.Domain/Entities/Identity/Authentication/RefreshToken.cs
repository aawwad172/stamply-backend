using Stambat.Domain.Interfaces.Domain;
using Stambat.Domain.Interfaces.Domain.Auditing;

namespace Stambat.Domain.Entities.Identity.Authentication;

public class RefreshToken : IEntity, ICreationAudit
{
    // Identity
    public Guid Id { get; init; }

    // Link to user/session family
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public Guid TokenFamilyId { get; private set; }

    // Security
    public string TokenHash { get; private set; }
    public string PlaintextToken { get; private set; } = string.Empty;

    // Lifetime
    public DateTime ExpiresAt { get; private set; }

    // Revocation
    public DateTime? RevokedAt { get; private set; }
    public string? ReasonRevoked { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; }
    public RefreshToken? ReplacedByToken { get; private set; }

    // Issuance metadata
    public string? SecurityStampAtIssue { get; private set; }

    // Auditing
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    // Convenience
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsExpired && !IsRevoked;

    // EF Core constructor
    private RefreshToken()
    {
        TokenHash = default!;
    }

    public static RefreshToken Create(
        Guid id,
        Guid userId,
        Guid tokenFamilyId,
        string tokenHash,
        string plaintextToken,
        DateTime expiresAt,
        Guid createdBy,
        string? securityStampAtIssue = null)
    {
        return new RefreshToken
        {
            Id = id,
            UserId = userId,
            TokenFamilyId = tokenFamilyId,
            TokenHash = tokenHash,
            PlaintextToken = plaintextToken,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            SecurityStampAtIssue = securityStampAtIssue
        };
    }

    public void Revoke(string reason, Guid? replacedByTokenId = null)
    {
        if (!IsRevoked)
        {
            RevokedAt = DateTime.UtcNow;
            ReasonRevoked = reason;
            ReplacedByTokenId = replacedByTokenId;
        }
    }
}
