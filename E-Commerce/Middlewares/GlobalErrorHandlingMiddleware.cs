using System.Net;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_Commerce.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next,ILogger<GlobalErrorHandlingMiddleware> logger )
        {
           _next = next;
            _logger = logger;
        }
         public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandleNotFoundPointAsync(httpContext);
            }
            catch(Exception exception)
            {
                _logger.LogError($"Something Went Wrong{exception}");
                await HandleExceptionAsync(httpContext, exception);
            }
           
        }
         private async Task HandleNotFoundPointAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var response = new ErorrDetails
            {
                StutusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = $"The End Point {httpContext.Request.Path}"
            }.ToString();
            await httpContext.Response.WriteAsync(response);
        }
        public async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            // Set Default Status code 500
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Set Content Type => Application/json
            httpContext.Response.ContentType = "application/json";
            var response = new ErorrDetails
            {
                ErrorMessage = exception.Message
            };
            //c#8
            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnAuthorizedException => (int)HttpStatusCode.Unauthorized,
                ValidationException validationException => HandleValidationException(validationException, response),


                _ => (int)HttpStatusCode.InternalServerError
            };

            // return standered  Response
            response.StutusCode = httpContext.Response.StatusCode;
            
            await httpContext.Response.WriteAsync(response.ToString());


        }

        private int HandleValidationException(ValidationException validationException, ErorrDetails response)
        {
            response.Errors = validationException.Errors;
                return (int)HttpStatusCode.BadRequest;
        }
    }
}
