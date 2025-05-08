
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Services.Abstractions;
using Services;
using E_Commerce.Middlewares;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Factories;
using E_Commerce.Extensions;

namespace E_Commerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddInfraStructureServices(builder.Configuration);
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddControllers().AddApplicationPart(typeof(Persentation.AssemblyReference).Assembly);
            builder.Services.AddPersentationServices();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            #region Build
            var app = builder.Build();
            #endregion

            #region PipLines
        
            await app.SeedDbAsync();
            app.UseCustomExceptionsMiddleWare();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseCors("CORSPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
          
            #endregion


        }
    }
}
