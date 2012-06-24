using System;
using System.Collections.Generic;
using System.Linq;

namespace LisysTest
{
    public static class IntExtentions
    {
        public static IEnumerable<int> To(this Int32 n, int last)
        {
            return Enumerable.Range(n, last - n + 1);
        }
    }
}