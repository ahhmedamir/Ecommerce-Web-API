using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specifications;
using Shared.OrderModels;

namespace Services
{
    public class OrderService(IMapper mapper,IUnitOfWork unitOfWork, IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderResult> CreateOrUpdateOrderAsync(OrderRequest request, string userEmail)
        {
            //1- Address
            var Address = mapper.Map<ShippingAddress>(request.shipToAddress);
            //2- Order Items=> Basket => BasketItems
            var Basket = await basketRepository.GetBasketAsync(request.BasketId)
                ?? throw new BasketNotFoundException(request.BasketId);
            var OrderItems = new List<OrderItem>();
             foreach( var item in Basket.Items)
            {
                var Product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync
                   (item.Id) ?? throw new ProductNotFoundExceptions(item.Id);
                OrderItems.Add(CreateOrderItem(item, Product));
            }
            var OrderRepo = unitOfWork.GetRepository<Order, Guid>();
            var existingOrder = await OrderRepo.GetByIdWithSpecificationsAsync(new OrderWithPaymentIntentIdSpecifications(Basket.PaymentIntentId));
             if (existingOrder is not null)
            {
                OrderRepo.Delete(existingOrder);
            }
            //3- Delivery
            var DeliveyMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
               .GetByIdAsync(request.DeliveryMethodId)
               ?? throw new DeliveyMethodNotFoundException(request.DeliveryMethodId);


            //4- SubTotal
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //Save To DataBase
            var Order = new Order(userEmail, Address, OrderItems, DeliveyMethod, SubTotal,Basket.PaymentIntentId);
            await OrderRepo.AddAsync(Order);
            await unitOfWork.SaveChangesAsync();
            // Map And Return
            return mapper.Map<OrderResult>(Order);







        }

        private OrderItem CreateOrderItem(BasketItem item, Product Product)
        => new OrderItem(new ProductInOrderItem(Product.Id, Product.Name, Product.PictureUrl),
            item.quantity, Product.Price);

        public async Task<IEnumerable<DeliverMethodResult>> GetDeliveryMethodsAsync()
        {
            var Methods = await unitOfWork.GetRepository<DeliveryMethod, int>()
                   .GetAllAsync();
            return mapper.Map<IEnumerable<DeliverMethodResult>>(Methods);
        }

        public async Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email)
        {
            var Orders = await unitOfWork.GetRepository<Order, Guid>()
                .GetAllWithSpecificationsAsync(new OrderWithIncludeSpecifications(email));
            return mapper.Map<IEnumerable<OrderResult>>(Orders);

        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var Order = await unitOfWork.GetRepository<Order, Guid>()
                .GetByIdWithSpecificationsAsync(new OrderWithIncludeSpecifications(id))
                ?? throw new OrderNotFoundException(id);
            return mapper.Map<OrderResult>(Order);
        }
    }
}
