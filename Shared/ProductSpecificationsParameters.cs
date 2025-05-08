using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
   public class ProductSpecificationsParameters
    {
        private const int MaxPageSize = 10;
        private const int DefaultPageSize = 5;
        public int? TypeId { get; set; }
         public int? BrandId { get; set; }
         public ProductSortingOptions? Sort { get; set; }
         public int pageIndex { get; set; }
        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
         public string? Search { get; set; }


    }
     public enum ProductSortingOptions
    {
NameAsc,
NameDesc,
PriceAsc,
PriceDesc
    }
}
