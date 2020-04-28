using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    //Класс корзины. По задумке хранится на устройстве
    public class Cart
    {
        public User User { get; set; }
        public List<Sell> Sells { get; set; }

        public Cart(User cartOwner)
        {
            User = cartOwner;
        }

        public Cart() { }

        public int getPrice()
        {
            int sellPricesSum = 0;

            for (int i = 0; i < Sells.Count; i++)
            {
                sellPricesSum += Sells[i].getPrice();
            }

            return sellPricesSum;
        }
    }
}
