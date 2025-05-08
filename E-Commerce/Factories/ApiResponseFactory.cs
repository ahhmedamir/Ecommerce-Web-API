using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            //Get All Errors in Model State

            var errors = context.ModelState.Where(error => error.Value.Errors.Any())
                .Select(error => new ValidiationError
                {
                    Filed = error.Key,
                    Errors= error.Value.Errors.Select(e=>e.ErrorMessage)

                });
            // Create Custom Response
            var response = new ValidationErrorResponse
            {
                 StatusCode= (int)HttpStatusCode.BadRequest,
                 ErrorMessage= "Validation Failed",
                 Errors= errors

            };
            //return
            return new BadRequestObjectResult(response);
        }
    }
}
