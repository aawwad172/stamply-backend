using Stamply.Domain.Entities.Authentication;

namespace Stamply.Domain.Interfaces.Infrastructure.IRepositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token, Guid userId);
}
