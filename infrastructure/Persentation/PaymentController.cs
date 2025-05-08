using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace Persentation
{
    public class PaymentController(IServiceManager serviceManager)
        :ApiController
    {
        [HttpPost("{basketId}")]
         
          public  async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent( string basketId)
        {
            var Result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(Result);

        }

        //For WebHook
        [HttpPost("webhook")]
        public async Task<IActionResult>WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await serviceManager.PaymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);
            return new EmptyResult();
        }
    
    }
}
