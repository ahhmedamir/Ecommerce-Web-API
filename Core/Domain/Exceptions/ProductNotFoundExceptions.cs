using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public  class ProductNotFoundExceptions : NotFoundException
    {
        public ProductNotFoundExceptions(int id) :
            base($"Product With Id : {id} Not Found")
        {

        }
    }
}
