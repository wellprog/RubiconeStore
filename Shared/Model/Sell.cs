using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class Sell
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public int StorageID { get; set; }
        public int CheckID { get; set; }

        /************************************************/
        // Relations

        [JsonIgnore]
        public Storage Storage { get; set; }
        [JsonIgnore]
        public Check Check { get; set; }

        public bool IsModelRight()
        {
            return Count != 0;
        }
    }
}
