using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class GoodPropertyValue
    {
        public int ID { get; set; }
        public int GoodPropertyID { get; set; }
        public int GoodID { get; set; }
        public string Value { get; set; }

        [JsonIgnore]
        public GoodProperty GoodProperty { get; set; }
        [JsonIgnore]
        public Good Good { get; set; }
    }
}
