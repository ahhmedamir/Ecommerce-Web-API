using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Persentation
{
    public class RedisCashAttribute(int durationInSec) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CashService;

            string cacheKey = GenerateCashKey(context.HttpContext.Request);
            var result = await cacheService.GetCashedValueAsync(cacheKey);
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "Application/Json",
                    StatusCode = (int)HttpStatusCode.OK
                };

                return;
            }
            var contextResult = await next.Invoke();

            if (contextResult.Result is OkObjectResult okObject)
            {
                await cacheService.SetCashValueAsync(cacheKey, okObject, TimeSpan.FromSeconds(durationInSec));
            }



        }

        private string GenerateCashKey(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);
             foreach( var item in request.Query.OrderBy(q=>q.Key))
            {
                KeyBuilder.Append($"{item.Key}--{item.Value}");
            }
            return KeyBuilder.ToString()
              ;
        }



    }

   

    
}
