using Stamply.Domain.Interfaces.Application.Services;

namespace Stamply.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get; set; }
}
