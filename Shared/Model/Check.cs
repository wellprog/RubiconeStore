using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class Check
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }

        /************************************************/
        // Relations

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public List<Sell> Sells { get; set; } = new List<Sell>();

        public bool IsModelRight()
        {
            return UserID != 0 && CreatedDate != DateTime.MinValue;
        }
    }
}
