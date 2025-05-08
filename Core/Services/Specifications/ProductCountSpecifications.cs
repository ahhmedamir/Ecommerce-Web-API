using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Shared;

namespace Services.Specifications
{
    public class ProductCountSpecifications:Specifications<Product>
    {
        public ProductCountSpecifications(ProductSpecificationsParameters parameters)
                 : base(product =>
                (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) &&
                (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId) &&
                (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.Contains(parameters.Search.ToLower().Trim())))
        {

        }
    }
}
