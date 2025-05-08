using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;

namespace Persentation
{
  
    public class ProductsController(IServiceManager ServiceManager):ApiController
    {

        #region GetAllProducts
       
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducts([FromQuery] ProductSpecificationsParameters parameters)
        {
            var products = await ServiceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(products);
        }
        #endregion
        #region Get All Brands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
        {
            var Brands = await ServiceManager.ProductService.GetAllBrandsAsync();
            return Ok(Brands);
        }
        #endregion
        #region Get All Types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
        {
            var Types = await ServiceManager.ProductService.GetAllTypesAsync();
            return Ok(Types);
        }
        #endregion
        #region Get Product By Id
  
        [ProducesResponseType(typeof(ProductResultDto), (int)HttpStatusCode.OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetProductById(int id)
        {
            var Product = await ServiceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }
        #endregion

    }
}
