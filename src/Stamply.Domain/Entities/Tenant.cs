using Stamply.Domain.Entities.Identity.Authentication;
using Stamply.Domain.Interfaces.Domain;
using Stamply.Domain.Interfaces.Domain.Auditing;

namespace Stamply.Domain.Entities;

/// <summary>
/// This entity represent the business that subscribe to our business.
/// </summary>
public class Tenant : IEntity, IBaseEntity
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    // Branding & Identity
    public required string Slug { get; set; } // Unique index in DB
    public string? LogoUrl { get; set; }
    public string PrimaryColor { get; set; } = "#000000"; // Default Black
    public string SecondaryColor { get; set; } = "#FFFFFF"; // Default White

    // Operational
    public bool IsActive { get; set; } = true;
    public string TimeZoneId { get; set; } = "Jordan Standard Time";
    public string CurrencyCode { get; set; } = "JOD";

    // Relationships
    public ICollection<UserRoleTenant> UserRoleTenants { get; set; } = [];
    // public ICollection<CardTemplate> CardTemplates { get; set; } = [];

    // Auditing (from your IBaseEntity)
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}
