using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
   public class DeliveyMethodNotFoundException(int id)
        :NotFoundException($"No DeliveyMethod With Id {id} Was  Found")
    {

    }
}
