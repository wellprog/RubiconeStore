using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class Storage : IValidate
    {
        public int ID { get; set; }
        public int GoodID { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }

        /************************************************/
        // Relations

        [JsonIgnore]
        public Good Good { get; set; }
        [JsonIgnore]
        public List<Sell> Sells { get; set; }

        public bool IsModelRight()
        {
            return Price > 0 && Count > 0 && GoodID > 0;
        }
    }
}
