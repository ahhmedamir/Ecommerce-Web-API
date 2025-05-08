using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.OrderModels;

namespace Persentation
{
   
    public class OrdersController(IServiceManager serviceManager) : ApiController
    {
        #region Create Order
        [HttpPost]
        public async Task<ActionResult<OrderResult>> Create(OrderRequest orderRequest)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManager.OrderService.CreateOrUpdateOrderAsync(orderRequest, email);
            return Ok(order);
        }
        #endregion
        #region  GetOrdersByEmail
        [HttpGet]
         public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrders()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await serviceManager.OrderService.GetOrderByEmailAsync(Email);
            return Ok(Orders);

        }
        #endregion
        #region GetOrderById
        [HttpGet("{id}")]
         public async Task<ActionResult<OrderResult>>GetOrderById(Guid id)
        {
            var Order = await serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(Order);
        }
        #endregion
        #region GetDeliveryMethods
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliverMethodResult>> GetDeliveryMethods()

        {
            var Methods = await serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(Methods);

        }

        #endregion
    }
}
