using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Services.Abstractions;

namespace Services
{
    public class CasheService(ICashRepository cashRepository)
        : ICashService
    {
        public async Task<string> GetCashedValueAsync(string cashkey)
        {
            return await cashRepository.GetAsync(cashkey);
        }

        public Task SetCashValueAsync(string cashekey, object value, TimeSpan duration)
        {
            return cashRepository.SetAsync(cashekey, value, duration);
        }
    }
}
