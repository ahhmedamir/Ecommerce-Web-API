using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Shared;

namespace E_Commerce.Extensions
{
    public static class CoreServiceExensions


    {
         public static IServiceCollection AddCoreServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(Services.AssemblyRefernce).Assembly);
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
        
    }
}
