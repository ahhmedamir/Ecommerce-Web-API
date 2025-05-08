using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntities
{
    public class Order:BaseEntity<Guid>
    {

        public Order()
        {

        }
        //-----------------------
        public Order(string _UserEmail, ShippingAddress _shippingAddress, ICollection<OrderItem> _orderItems, DeliveryMethod _deliveryMethod, decimal _Subtotal,string _PaymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = _UserEmail;
            ShippingAddress = _shippingAddress;
            OrderItems = _orderItems;
            DeliveryMethod = _deliveryMethod;
            SubTotal = _Subtotal;
            PaymentIntentId = _PaymentIntentId;
        }
        //UserEmail
        public string UserEmail { get; set; }
        //Address
        public ShippingAddress ShippingAddress { get; set; }
         //OrderItems
          public ICollection<OrderItem> OrderItems { get; set; }
        //PaymentStatus
         public OrderPaymentStatus PaymentStatus { get; set; }
        //DeliveyMethod
        public int? DeliveryMethodId { get; set; }
         public DeliveryMethod DeliveryMethod { get; set; }
        //SubTotal
        public decimal SubTotal { get; set; }
        //Payment
        public string PaymentIntentId { get; set; } 
        //Order Date
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;



    }
}
