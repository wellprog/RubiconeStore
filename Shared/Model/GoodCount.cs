using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Shared.Model
{
    public class GoodCount
    {
        public Good Good { get; }
        public int Count { get; }

        public GoodCount(Good _good, int _count)
        {
            Good = _good;
            Count = _count;
        }
    }
}
