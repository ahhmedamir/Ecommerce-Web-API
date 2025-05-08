using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.OrderEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            #region shippingAddress
            builder.OwnsOne(order => order.ShippingAddress, Address => Address.WithOwner());

            #endregion
            #region OrderItem
            builder.HasMany(order => order.OrderItems).WithOne();


            #endregion
            #region PaymentStatus
            builder.Property(order => order.PaymentStatus)
                .HasConversion(S => S.ToString(),
                S => Enum.Parse<OrderPaymentStatus>(S));

            #endregion
            #region DeliveryMethod
            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
            #endregion
            #region SubTotal
            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(18,3)");
            #endregion
            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
