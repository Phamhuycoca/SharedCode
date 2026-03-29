using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedCode.Api.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiControllerServices(
       this IServiceCollection services,
       IConfiguration configuration,
       params Assembly[] assemblies
   )
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
        });
        return services;
    }
}
