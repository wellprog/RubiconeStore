using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Model
{
    public interface IErrorResponce
    {
        int ErrorCode { get; set; }
        string ErrorDescription { get; set; }
    }
}
