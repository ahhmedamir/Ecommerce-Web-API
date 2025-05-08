using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class StoreContext:DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreContext).Assembly);
        }
        #region DbSets


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
         public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
         public DbSet<OrderItem> OrderItems { get; set; }
         public DbSet<DeliveryMethod>DeliveryMethods { get; set; }
        #endregion

    }
}
