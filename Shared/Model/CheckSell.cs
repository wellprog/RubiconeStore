using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class CheckSell
    {
        public int ID { get; set; }
        public int SellID { get; set; }
        public int CheckID { get; set; }

        /************************************************/
        // Relations

        public Sell Sell { get; set; }
        public Check Check { get; set; }
    }
}
