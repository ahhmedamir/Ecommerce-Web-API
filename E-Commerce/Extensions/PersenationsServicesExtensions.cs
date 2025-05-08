using System.Runtime.CompilerServices;
using E_Commerce.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace E_Commerce.Extensions
{
    public  static class PersenationsServicesExtensions
    {
        public static IServiceCollection AddPersentationServices(this IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(Persentation.AssemblyReference).Assembly);
            services.Configure<ApiBehaviorOptions>(

                options =>
                {
                    options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
                });
            services.CongigureSwagger();
            services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:4200");

                });

            });
            
    
           
            
           
            return services;
           
        }
   public static IServiceCollection CongigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options=>
            {
                options.AddSecurityDefinition("Bearer", securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In= Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description= "Please Enter Bearer Token",
                    Name="Authorization",
                    Type= Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme= "Bearer",
                    BearerFormat="JWT"

                });


                options.AddSecurityRequirement(securityRequirement: new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                    {
                new OpenApiSecurityScheme
                {
                    Reference= new OpenApiReference
                    {
                        Type= ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }

                },
                new List<string>(){}
                }
            });
            });
            return services;

        }
        
    
    }
}
