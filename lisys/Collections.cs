using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// コレクションに対する共通実装．一部公開 I/F を含む．
    /// </summary>
    public static class Collections
    {
        internal static IVectorCollection<C> Subcollection<C>(IVectorCollection<C> collection,
                                                   int startIndex)
            where C : IVectorCollection<C>
        {
            return Subcollection(collection, startIndex, collection.Count - startIndex);
        }

        internal static IVectorCollection<C> Subcollection<C>(IVectorCollection<C> collection,
                                                        int startIndex,
                                                        int length)
            where C : IVectorCollection<C>
        {
            if (collection.Count <= startIndex)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (length < 0)
            {
                throw new ArgumentException();
            }

            int endIndex = (collection.Count < startIndex + length) ? collection.Count : startIndex + length;

            int[] idxs = new int[endIndex - startIndex];
            for (int i = 0; i < idxs.Length; ++i)
            {
                idxs[i] = i + startIndex;
            }

            return new SubCollection<C>(collection, idxs);
        }

        /// <summary>
        /// 行コレクションを行列として出力
        /// </summary>
        public static Matrix ToMatrix(this IVectorCollection<RowCollection> rc)
        {
            return Matrix.CreateAsRows(rc);
        }

        /// <summary>
        /// 行コレクションを行列として出力
        /// </summary>
        public static Matrix ToMatrix(this IEnumerable<RowVector> rows)
        {
            return new Matrix(rows.ToArray());
        }

        /// <summary>
        /// 列コレクションを行列として出力
        /// </summary>
        public static Matrix ToMatrix(this IVectorCollection<ColumnCollection> cc)
        {
            return Matrix.CreateAsColumns(cc);
        }

        /// <summary>
        /// 列コレクションを行列として出力
        /// </summary>
        public static Matrix ToMatrix(this IEnumerable<ColumnVector> cols)
        {
            return new Matrix(cols.ToArray());
        }
    }
}
