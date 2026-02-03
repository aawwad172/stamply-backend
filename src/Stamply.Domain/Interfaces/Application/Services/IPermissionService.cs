using Stamply.Domain.Entities;

namespace Stamply.Domain.Interfaces.Application.Services;

public interface IPermissionService
{
    Task<List<string>> GetUserRolesAsync(Guid userId);
    Task<List<string>> GetUserPermissionsAsync(User user);
}
