using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public record UserResultDto(string DisplayName,string Email,string Token)
    {
    }
}
