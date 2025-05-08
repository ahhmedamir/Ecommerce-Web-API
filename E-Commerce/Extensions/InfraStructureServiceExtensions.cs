using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Shared;
using StackExchange.Redis;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Commerce.Extensions
{
    public  static class InfraStructureServiceExtensions
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICashRepository, CasheRepository>();
            services.AddDbContext<StoreContext>(
                
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            } );
            services.AddDbContext<StoreIdentityContext>(

                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
                });
            services.ConfigureIdentityService();
            services.ConfigureJwt(configuration);
            services.AddSingleton<IConnectionMultiplexer>(
                _=>ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
            return services;
        }
    public static IServiceCollection ConfigureIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
             
            })
                .AddEntityFrameworkStores<StoreIdentityContext>();
            return services;

        }
    public static IServiceCollection ConfigureJwt(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtoptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options=>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
ValidateIssuer= true,
ValidateAudience= true,
ValidateLifetime= true,
ValidateIssuerSigningKey= true,
ValidIssuer= jwtoptions.Issuer,
ValidAudience= jwtoptions.Audience,
IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey))
                };



            });
            services.AddAuthorization();
            return services;
        }
    }
}
