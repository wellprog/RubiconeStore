using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class CartModel
    {
        public Check Check { get; set; }
        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}
