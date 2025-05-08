using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreContext _storeContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreContext storeContext,UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            _storeContext = storeContext;
           _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            try
            {
                #region For Update DataBase
                //Create DataBase If It Doesnot Exist And  Applying Any Pending Migrations
                if (_storeContext.Database.GetPendingMigrations().Any())
                    await _storeContext.Database.MigrateAsync();
                #endregion

                #region ProductTypes
                if (!_storeContext.ProductTypes.Any())
                {
                    //Read Types From File As String

         var TypesData = await File.ReadAllTextAsync("F:\\Route\\Route C43\\API\\G01&G02\\E-CommerceG01&G02\\infrastructure\\Persestance\\Data\\Seeding\\types.json");
                    //Transform into C# Objects
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                    //Add To Db & Save Changes
                    if (Types is not null && Types.Any())
                    {
                        await _storeContext.ProductTypes.AddRangeAsync(Types);
                        await _storeContext.SaveChangesAsync();
                    }

                }
                #endregion
                #region ProductBrands
                if (!_storeContext.ProductBrands.Any())
                {
                    //Read Brands From File As String

                    var BrandsData = await File.ReadAllTextAsync("F:\\Route\\Route C43\\API\\G01&G02\\E-CommerceG01&G02\\infrastructure\\Persestance\\Data\\Seeding\\brands.json");
                    //Transform into C# Objects
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                    //Add To Db & Save Changes
                    if (Brands is not null && Brands.Any())
                    {
                        await _storeContext.ProductBrands.AddRangeAsync(Brands);
                        await _storeContext.SaveChangesAsync();
                    }

                }
                #endregion
                #region Product
                if (!_storeContext.Products.Any())
                {
                    //Read Products From File As String

                    var ProductsData = await File.ReadAllTextAsync("F:\\Route\\Route C43\\API\\G01&G02\\E-CommerceG01&G02\\infrastructure\\Persestance\\Data\\Seeding\\products.json");
                    //Transform into C# Objects
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    //Add To Db & Save Changes
                    if (Products is not null && Products.Any())
                    {
                        await _storeContext.Products.AddRangeAsync(Products);
                        await _storeContext.SaveChangesAsync();
                    }

                }
                #endregion
                #region DeliveyMethods
                if (!_storeContext.DeliveryMethods.Any())
                {
                    //Read Types From File As String
                    var DeliveyMethodsData = await File.ReadAllTextAsync("F:\\Route\\Route C43\\API\\G01&G02\\E-CommerceG01&G02\\infrastructure\\Persestance\\Data\\Seeding\\delivery.json");
                    //TransFoem into C# Objects
                    var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveyMethodsData);
                    //Add To DatBase And Save Changes
                    if (Methods is not null && Methods.Any())
                    {
                        await _storeContext.DeliveryMethods.AddRangeAsync(Methods);
                        await _storeContext.SaveChangesAsync();

                    }
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public async Task InitializeIdentityAsync()
        {
           //Seed Default Roles
           if(! _roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                

            }
            //Seed Default Users
            if(! _userManager.Users.Any())
            {
                var SuperAdminUser = new User
                {
                    DisplayName= "SuperAdminUser",
                    Email= "SuperAdminUser@gmail.com",
                    UserName= "SuperAdminUser",
                    PhoneNumber="01225770196"

                };
                var AdminUser = new User
                {
                    DisplayName="AdminUser",
                    Email= "AdminUser@gmail.com",
                    UserName= "AdminUser",
                    PhoneNumber="01225770187"

                };
                await _userManager.CreateAsync(SuperAdminUser, "Passw0rd");//Password
                await _userManager.CreateAsync(AdminUser, "Passw0rd");
                //-------------------------------------------------------
                await _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(AdminUser, "Admin");


            }
        }
    }
}
