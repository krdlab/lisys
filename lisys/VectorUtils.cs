using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    internal static class VectorChecker
    {
        internal static void IsNotZeroSize(IRandomAccessible<double> v)
        {
            if (v.Size == 0)
            {
                throw new ArgumentException("zero size");
            }
        }

        internal static void SizeEquals(IRandomAccessible<double> v1, IRandomAccessible<double> v2)
        {
            if (v1.Size != v2.Size)
            {
                throw new ArgumentException(
                    String.Format("mismatch size: ({0}) != ({1})", v1.Size, v2.Size));
            }
        }
    }

}
