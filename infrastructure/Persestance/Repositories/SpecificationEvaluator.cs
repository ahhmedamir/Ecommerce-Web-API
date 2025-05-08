using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(
            IQueryable<T>inputQuery,
            Specifications<T> specifications
            ) where T:class
        {
            // step 01:
            var query = inputQuery;
            //step 02:
            if (specifications.Criteria is not null) query = query.Where(specifications.Criteria);
            //Step 03: Aggreagte Expressions

            //foreach (var item in specifications.IncludeExpression)
            //{
            //    query = query.Include(item);
            //}
            query = specifications.IncludeExpression.Aggregate(
            query,
            (currentquery, includeexpression) => currentquery.Include(includeexpression));
           if (specifications.OrderBy is not null)
            {
                query = query.OrderBy(specifications.OrderBy);
            }
            else if (specifications.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specifications.OrderByDescending);
            }

            return query;
       
        }

    }
}
