using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ErorrDetails
    {
         public int StutusCode { get; set; }
         public string ErrorMessage { get; set; }
         public IEnumerable<string>?Errors { get; set; }
        public override string ToString()
      => JsonSerializer.Serialize(this);
    }
}
