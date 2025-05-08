using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICashRepository
    {
        //1-
        public Task SetAsync(string Key, object value, TimeSpan duration);

        //2-
        public Task<string?> GetAsync(string Key);

    }
}
