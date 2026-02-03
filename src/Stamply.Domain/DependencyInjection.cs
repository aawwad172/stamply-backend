using Microsoft.Extensions.DependencyInjection;

namespace Stamply.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services;
}
