using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    internal class CollectionChecker
    {
        internal static void HasIndexes<C>(IVectorCollection<C> collection, int[] indexes)
            where C : IVectorCollection<C>
        {
            if (indexes == null)
            {
                throw new ArgumentNullException("indexes");
            }
            if (indexes.Length <= 0 || collection.Count < indexes.Length)
            {
                throw new ArgumentOutOfRangeException("illegal length");
            }
            foreach (int i in indexes)
            {
                if (i < 0 || collection.Count <= i)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
