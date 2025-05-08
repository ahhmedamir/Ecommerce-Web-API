using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper Mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var Brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var BrandsReuslt = Mapper.Map<IEnumerable<BrandResultDto>>(Brands);
            return BrandsReuslt;
           
        }

        public  async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters parameters)
        {
            var Products = await unitOfWork.GetRepository<Product, int>()
                .GetAllWithSpecificationsAsync(new ProductWithBrandAndTypeSpecifications(parameters)) ;
            var ProductsReuslt = Mapper.Map<IEnumerable<ProductResultDto>>(Products);
            //For Count
            var count = ProductsReuslt.Count();
            var totalCount = await unitOfWork.GetRepository<Product, int>()
                .CountAsync(new ProductCountSpecifications(parameters));
            var result = new PaginatedResult<ProductResultDto>(
                parameters.pageIndex,// pageindex
                count,//pagesize
                totalCount,// totalcount
               ProductsReuslt);
            
            
            return result;
        }

        public async  Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var TypesReuslt = Mapper.Map<IEnumerable<TypeResultDto>>(Types);
            return TypesReuslt;
        }

        public  async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var Product = await unitOfWork.GetRepository<Product, int>().GetByIdWithSpecificationsAsync(new ProductWithBrandAndTypeSpecifications(id));
            return Product is null ? throw new ProductNotFoundExceptions(id)
               : Mapper.Map<ProductResultDto>(Product);
        }
    }
}
