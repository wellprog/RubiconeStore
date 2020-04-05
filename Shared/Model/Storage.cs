using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class Storage
    {
        public int ID { get; set; }
        public int GoodID { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }

        /************************************************/
        // Relations

        public Good Good { get; set; }
        public List<Sell> Sells { get; set; }
    }
}
