using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class ResponceModel <T> : IErrorResponce
    {
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public T content { get; set; }
    }
}
