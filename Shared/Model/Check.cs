using System;
using System.Collections.Generic;
using System.Text;

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

        public User User { get; set; }
        public List<CheckSell> CheckSells { get; set; }
    }
}
