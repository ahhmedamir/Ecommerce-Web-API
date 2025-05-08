using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstractions
{
    public interface IProductService
    {
        public Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters parameters);
        public Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        public Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        public Task<ProductResultDto?> GetProductByIdAsync(int id);


    }
}
