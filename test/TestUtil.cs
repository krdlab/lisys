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

    public class DoubleSequence : IEnumerable<double>
    {
        public IEnumerator<double> GetEnumerator()
        {
            yield return 0;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}