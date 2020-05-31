using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class CartItemModel : IValidate
    {
        public Good Good { get; set; }
        public int Count { get; set; }

        public bool IsModelRight()
        {
            return Count > 0 && Good != null;
        }
    }
}
