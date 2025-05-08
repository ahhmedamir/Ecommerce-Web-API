using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;

namespace Persentation
{
    public class AuthenticationController(IServiceManager serviceManager):ApiController
    {
        #region Login
        //Authentication/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>>Login(LoginDto loginDto)
        {
            var Result = await serviceManager.AuthenticationService.LoginAsync(loginDto);
            return Ok(Result);
        }

        #endregion
        #region Register
        [HttpPost("Register")]
         public async Task<ActionResult<UserResultDto>>Register(UserRegisterDto userRegisterDto)
        {
            var Result = await serviceManager.AuthenticationService.RegisterAsync(userRegisterDto);
            return Ok(Result);

        }
        #endregion
        #region  Email Exist
        [HttpGet("EmailExist")]
         public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return Ok(await serviceManager.AuthenticationService.CheckEmailExist(email));

        }
        #endregion
        #region Get Current User
        [HttpGet]
        public async Task<ActionResult<UserResultDto>>GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await serviceManager.AuthenticationService.GetUserByEmail(email);
            return Ok(Result);
        }
        #endregion
        #region Get User Address
        //Authentication/Address
        [Authorize]

        [HttpGet("Address")]
         public async Task<ActionResult<AddressDto>>GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await serviceManager.AuthenticationService.GetUserAddress(email);
            return Ok(Result);
        }
        #endregion
        #region  Update Address
        [Authorize]
        [HttpPut("Address")]
         public async Task<ActionResult<AddressDto>>UpdateAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await serviceManager.AuthenticationService.UpdateUserAddress(address, email);
return Ok(Result);
        }
         
        #endregion
    }
}
