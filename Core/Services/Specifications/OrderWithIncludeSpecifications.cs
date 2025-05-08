using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.OrderEntities;

namespace Services.Specifications
{
   public class OrderWithIncludeSpecifications:Specifications<Order>

    {
         public OrderWithIncludeSpecifications(Guid id)
            :base(o=>o.Id==id)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.OrderItems);

        }
        //Get By Email
        public OrderWithIncludeSpecifications( string email)
            : base(o => o.UserEmail == email)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.OrderItems);
            SetOrderBy(O => O.OrderDate);

        }

    }
}
