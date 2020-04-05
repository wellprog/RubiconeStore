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

        /************************************************/
        // Relations

        public Storage Storage { get; set; }
        public List<CheckSell> CheckSells { get; set; }
    }
}
