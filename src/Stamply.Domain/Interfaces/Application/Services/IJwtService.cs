using System.Security.Claims;

using Stamply.Domain.Entities.Identity;
using Stamply.Domain.Entities.Identity.Authentication;

namespace Stamply.Domain.Interfaces.Application.Services;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(User user);
    RefreshToken CreateRefreshTokenEntity(
        User user,
        Guid tokenFamilyId);
    Task<ClaimsPrincipal> ValidateToken(string token);
}
