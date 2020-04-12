using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class RequestModel <T>
    {
        public string AuthKey { get; set; }

        public T Content { get; set; }
    }
}
