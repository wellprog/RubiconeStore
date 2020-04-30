﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class Check
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }

        /************************************************/
        // Relations

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public List<Sell> Sells { get; set; }

        public int getPrice()
        {
            int sellPricesSum = 0;

            for (int i = 0; i < Sells.Count; i++)
            {
                sellPricesSum += Sells[i].getPrice();
            }

            return sellPricesSum;
        }
        public bool IsModelRight()
        {
            return UserID != 0 && CreatedDate != DateTime.MinValue;
        }
    }
}
