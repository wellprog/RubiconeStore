using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class GoodProperty : IValidate, IPK
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string DefaultValue { get; set; } = "";
        public int GoodCategoryID { get; set; } = 0;

        /************************************************/
        // Relations
        [JsonIgnore]
        public GoodCategory GoodCategory { get; set; }
        [JsonIgnore]
        public List<GoodPropertyValue> GoodPropertyValues { get; set; }

        public bool IsModelRight() =>   !string.IsNullOrWhiteSpace(Name) &&
                                        !string.IsNullOrWhiteSpace(Description) &&
                                        !string.IsNullOrWhiteSpace(DefaultValue) &&
                                        GoodCategoryID > 0;
    }
}
