using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstractions;
using Services.Specifications;
using Shared;
using Stripe;
using Product = Domain.Entities.Product;
namespace Services
{
    public class PaymentService
        (IBasketRepository basketRepository,
        IMapper mapper,
        IConfiguration configuration,
        IUnitOfWork unitOfWork) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configuration.GetRequiredSection("StripeSettings")["SecretKey"];
            //Get Basket  
            var basket = await basketRepository.GetBasketAsync(basketId)
                   ?? throw new BasketNotFoundException(basketId);
             foreach( var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id) ?? throw new ProductNotFoundExceptions(item.Id);
                item.Price = product.Price;
            }
            //For DeliveyMethod
            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No DeliveyMethod Is Choosed");
            var method = await unitOfWork.GetRepository<DeliveryMethod, int>()
               .GetByIdAsync(basket.DeliveryMethodId.Value)
               ?? throw new DeliveyMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = method.Price;
            var service = new PaymentIntentService();
            // For Amount
            var amount = (long)(basket.Items.Sum(i => i.quantity * i.Price) + basket.ShippingPrice) * 100;

             if (string.IsNullOrWhiteSpace(basket.PaymentIntentId))
            {
                //Create
                var CreateOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                var PayemntIntent = await service.CreateAsync(CreateOptions);
                basket.PaymentIntentId = PayemntIntent.Id;
                basket.ClientSecret = PayemntIntent.ClientSecret;
            }
             
              else
            {
                //Update
                var UpdatedOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };
                await service.UpdateAsync(basket.PaymentIntentId, UpdatedOptions);
            }
            await basketRepository.UpdateBasketAsync(basket);
            return mapper.Map<BasketDto>(basket);

        }
        public async Task UpdateOrderPaymentStatus(string request, string header)
        {
            var stripeSettingsSection = configuration.GetSection("StripeSettings");
            var endPointSecret = stripeSettingsSection?["EndPointSecret"];

            if (string.IsNullOrEmpty(endPointSecret))
            {
                throw new InvalidOperationException("Stripe EndPointSecret is missing.");
            }

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(request, header, endPointSecret);
            }
            catch (Exception ex)
            {
                // Handle invalid webhook payload or signature
                Console.WriteLine("Stripe event construction failed: " + ex.Message);
                throw;
            }

            var paymentIntent = stripeEvent.Data?.Object as PaymentIntent;

            if (paymentIntent == null)
            {
                Console.WriteLine("PaymentIntent is null in event: " + stripeEvent.Type);
                return;
            }

            switch (stripeEvent.Type)
            {
                case "payment_intent.payment_failed":
                    await UpdatePaymentFailed(paymentIntent.Id);
                    break;

                case "payment_intent.succeeded":
                    await UpdatePaymentReceived(paymentIntent.Id);
                    break;

                default:
                    Console.WriteLine("Unhandled event type: " + stripeEvent.Type);
                    break;
            }
        }

        //public async Task UpdateOrderPaymentStatus(string request, string header)
        //{
        //    //WebHook Secret 
        //    var endPointSecret = configuration.GetRequiredSection("StripeSettings")["EndPointSecret"];
        //    // All Informations Abouts Event That Was Sent By Stripe

        //    var stripeEvent = EventUtility.ConstructEvent(request,
        //        header, endPointSecret);
        //    // That Mean Payment Complited

        //    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

        //    switch (stripeEvent.Type)
        //    {
        //        case "payment_intent.payment_failed":
        //            await UpdatePaymentFailed(paymentIntent!.Id);

        //            break;
        //        case "payment_intent.succeeded":
        //            await UpdatePaymentReceived(paymentIntent!.Id);
        //            break;
        //        // ... handle other event types
        //        default:
        //            Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
        //            break;
        //    }
        //}
        private async Task UpdatePaymentFailed(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetByIdWithSpecificationsAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId))
                ?? throw new Exception();
            // Change PaymentStatus 

            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            unitOfWork.GetRepository<Order, Guid>().Update(order);
            await unitOfWork.SaveChangesAsync();
        }


        private async Task UpdatePaymentReceived(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetByIdWithSpecificationsAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId))
                ?? throw new Exception();

            order.PaymentStatus = OrderPaymentStatus.PaymentRecived;

            unitOfWork.GetRepository<Order, Guid>().Update(order);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
