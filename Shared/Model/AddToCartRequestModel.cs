using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class AddToCartRequestModel
    {
        public User User { get; set; }
        public Sell Sell { get; set; }
    }
}
