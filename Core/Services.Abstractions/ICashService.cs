using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public  interface  ICashService
    {
        public Task<string> GetCashedValueAsync(string cashkey);
        public Task SetCashValueAsync(string cashekey, object value, TimeSpan duration);

    }
}
