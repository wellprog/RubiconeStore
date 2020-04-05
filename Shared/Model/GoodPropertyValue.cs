using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class GoodPropertyValue
    {
        public int ID { get; set; }
        public int GoodPropertyID { get; set; }
        public int GoodID { get; set; }
        public string Value { get; set; }

        public GoodProperty GoodProperty { get; set; }
        public Good Good { get; set; }
    }
}
