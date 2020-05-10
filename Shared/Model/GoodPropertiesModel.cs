using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class GoodPropertiesModel
    {
        public Good Good { get; set; }
        public IEnumerable<GoodProperty> GoodProperties { get; set; }
        public IEnumerable<GoodPropertyValue> GoodPropertyValues { get; set; }
    }
}
