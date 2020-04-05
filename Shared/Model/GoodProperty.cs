using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class GoodProperty
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string DefaultValue { get; set; } = "";
        public int GoodCategoryID { get; set; } = 0;

        /************************************************/
        // Relations
        public GoodCategory GoodCategory { get; set; }
        public List<GoodPropertyValue> GoodPropertyValues { get; set; }
    }
}
