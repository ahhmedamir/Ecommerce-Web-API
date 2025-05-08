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
    
    public  class BasketController(IServiceManager ServiceManager) : ApiController
    {
        #region Get
        [HttpGet("{id}")]
         public async Task<ActionResult<BasketDto>> Get(string id)
        {
            var basket = await ServiceManager.BasketService.GetBasketAsync(id);
            return basket;
        }
        #endregion
        #region Update
        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update (BasketDto basketDto)
        {
            var Basket = await ServiceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok(basketDto);
        }
        #endregion
        #region Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await ServiceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

        #endregion
    }
}
