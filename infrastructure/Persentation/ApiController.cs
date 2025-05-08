using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace Persentation
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ErorrDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErorrDetails), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ValidationErrorResponse), (int)HttpStatusCode.BadRequest)]
    public class ApiController:ControllerBase
    {
    }
}
