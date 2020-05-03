using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class GoodCategory : IValidate, IPK
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Picture { get; set; } = "";

        /************************************************/
        // Relations
        [JsonIgnore]
        public List<Good> Goods { get; set; }
        [JsonIgnore]
        public List<GoodProperty> GoodProperties { get; set; }

        public bool IsModelRight() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);

        public override string ToString()
        {
            return Name;
        }
    }
}
