using Stamply.Domain.Entities.Authentication;

namespace Stamply.Domain.Interfaces.Infrastructure.IRepositories;

public interface IRoleRepository
{
    Task<Role?> GetRoleByNameAsync(string name);
}
