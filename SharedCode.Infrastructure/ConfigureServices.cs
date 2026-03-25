using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SharedCode.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? dbOptions = null
    )
        where TContext : DbContext
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddDbContext<TContext>(options =>
        {
            if (dbOptions != null)
            {
                dbOptions(options);
            }
            else
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                );
            }
        });

        return services;
    }
}