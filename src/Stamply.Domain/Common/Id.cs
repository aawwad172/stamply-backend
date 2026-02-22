namespace Stamply.Domain.Common;

public static class Id
{
    public static Guid New() => Guid.CreateVersion7();
}
