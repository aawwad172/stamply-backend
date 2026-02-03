namespace Stamply.Domain.Interfaces.Domain.Auditing;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}
