using System;
using System.Collections.Generic;
using System.Text;

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

        public Good SelledGood { get; set; }
        public Storage Storage { get; set; }
        public Check Check { get; set; }

        public int getPrice()
        {
            return Count * SelledGood.Price;
        }
        public bool IsModelRight()
        {
            return Count != 0;
        }
    }
}
