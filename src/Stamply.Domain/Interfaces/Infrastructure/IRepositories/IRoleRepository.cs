using Stamply.Domain.Entities.Identity.Authentication;

namespace Stamply.Domain.Interfaces.Infrastructure.IRepositories;

public interface IRoleRepository
{
    Task<Role?> GetRoleByNameAsync(string name);
}
