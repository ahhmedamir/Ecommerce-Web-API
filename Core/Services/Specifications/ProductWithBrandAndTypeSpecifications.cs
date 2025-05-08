using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : Specifications<Product>
    {
        //Use To Retrive Product By Id
        public ProductWithBrandAndTypeSpecifications(int id)
            : base(product => product.Id == id)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);

        }


        // Use To Get All Products
        public ProductWithBrandAndTypeSpecifications(ProductSpecificationsParameters parameters)
                : base(product =>
                (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) &&
                (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId)&&
                (string.IsNullOrWhiteSpace(parameters.Search)|| product.Name.Contains(parameters.Search.ToLower().Trim())))
       
         
        {
          
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
            #region  Sort
            ApplyPagination(parameters.pageIndex, parameters.PageSize);
            switch(parameters.Sort)
                {
                       case ProductSortingOptions.NameDesc:
                        SetOrderByDescending(product => product.Name);
                        break;
                    case ProductSortingOptions.NameAsc:
                        SetOrderBy(product=>product.Name);
                        break;
                    case ProductSortingOptions.PriceDesc:
                        SetOrderByDescending(p => p.Price);
                        break;
                case ProductSortingOptions.PriceAsc:
                    SetOrderBy(product => product.Price);
                    break;
                    default:
                        
                        break;

                
            }
            #endregion
        }


    }
}
