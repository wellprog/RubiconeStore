using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class Check
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        //Есть класс Cart для корзины, значит все чеки - оплачены и существуют как история.
        //public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }

        /************************************************/
        // Relations

        public User User { get; set; }
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
