using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Model
{
    public interface IErrorResponce
    {
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
