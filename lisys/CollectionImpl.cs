using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// Collectionクラスに共通した実装を提供する．
    /// </summary>
    internal static class CollectionImpl
    {
        internal static IVectorCollection ForEach(IVectorCollection collection, VectorCollectionAction action)
        {
            for (int i = 0; i < collection.Count; ++i)
            {
                action(collection[i]);
            }
            return collection;
        }

        internal static IVectorCollection ForEach(IVectorCollection collection, VectorCollectionActionWithIndex action)
        {
            for (int i = 0; i < collection.Count; ++i)
            {
                action(i, collection[i]);
            }
            return collection;
        }

        internal static IEnumerator<IVector> Enumerator(IVectorCollection collection)
        {
            for (int i = 0; i < collection.Count; ++i)
            {
                yield return collection[i];
            }
        }

        internal static Matrix ToMatrix(IVectorCollection collection, CollectionType type)
        {
            if (type == CollectionType.Rows)
            {
                return RowsToMatrix(collection);
            }
            else
            {
                return ColumnsToMatrix(collection);
            }
        }

        internal static Matrix ColumnsToMatrix(IVectorCollection collection)
        {
            int rowSize = collection[0].Size;
            int colSize = collection.Count;

            Matrix m = new Matrix(rowSize, colSize);
            for (int c = 0; c < colSize; ++c)
            {
                m.Columns[c] = collection[c];
            }

            return m;
        }

        internal static Matrix RowsToMatrix(IVectorCollection collection)
        {
            int rowSize = collection.Count;
            int colSize = collection[0].Size;

            Matrix m = new Matrix(rowSize, colSize);
            for (int r = 0; r < rowSize; ++r)
            {
                m.Rows[r] = collection[r];
            }

            return m;
        }

        internal static IVectorCollection Subcollection(IVectorCollection collection,
                                                        CollectionType type,
                                                        int startIndex)
        {
            return Subcollection(collection, type, startIndex, collection.Count - startIndex);
        }

        internal static IVectorCollection Subcollection(IVectorCollection collection,
                                                        CollectionType type,
                                                        int startIndex,
                                                        int length)
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

            return new SubCollection(collection, idxs, type);
        }

        internal static IVectorCollection Swap(IVectorCollection collection, int index1, int index2)
        {
            if (index1 < 0 || collection.Count <= index1 || index2 < 0 || collection.Count <= index2)
            {
                throw new IndexOutOfRangeException();
            }

            if (index1 != index2)
            {
                IVector rv1 = collection[index1];
                IVector rv2 = collection[index2];
                for (int i = 0; i < rv1.Size; ++i)
                {
                    double val = rv1[i];
                    rv1[i] = rv2[i];
                    rv2[i] = val;
                }
            }

            return collection;
        }

        internal static void CheckIndexes(IVectorCollection collection, int[] indexes)
        {
            if (indexes == null)
            {
                throw new Exception.IllegalArgumentException();
            }
            if (indexes.Length <= 0 || collection.Count < indexes.Length)
            {
                throw new Exception.IllegalArgumentException();
            }
            foreach (int i in indexes)
            {
                if (i < 0 || collection.Count <= i)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        internal static void Setter(IVectorCollection collection, int index, IVector value)
        {
            IVector dst = collection[index];
            IVector src = value;

            VectorChecker.MismatchSize(src, dst);

            for (int i = 0; i < src.Size; ++i)
            {
                dst[i] = src[i];
            }
        }

    }

    /// <summary>
    /// コレクションで使用される参照用ベクトルクラス
    /// </summary>
    internal class RefVector : IVector
    {
        private double[] _body = null;
        private int _begin;
        private int _interval;
        private int _size;

        internal RefVector(double[] body, int begin, int interval, int size)
        {
            this._body = body;
            this._begin = begin;
            this._interval = interval;
            this._size = size;
        }

        #region 固有の実装

        public override string ToString()
        {
            return VectorImpl.ToString(this);
        }

        #endregion

        #region IVector メンバ

        double IVector.this[int i]
        {
            get
            {
                return this._body[this._begin + i * this._interval];
            }
            set
            {
                this._body[this._begin + i * this._interval] = value;
            }
        }

        int IVector.Size
        {
            get { return this._size; }
        }

        double IVector.Norm
        {
            get
            {
                return VectorImpl.Norm(this);
            }
        }

        double IVector.Sum
        {
            get
            {
                return VectorImpl.Sum(this);
            }
        }

        double IVector.SumSq
        {
            get
            {
                return VectorImpl.SumSq(this);
            }
        }

        double IVector.Average
        {
            get
            {
                return VectorImpl.Average(this);
            }
        }

        double IVector.Scatter
        {
            get
            {
                return VectorImpl.Scatter(this);
            }
        }

        double IVector.Variance
        {
            get
            {
                return VectorImpl.SampleVariance(this);
            }
        }

        IVector IVector.Flip()
        {
            return VectorImpl.Flip(this);
        }

        IVector IVector.Zero()
        {
            return VectorImpl.Zero(this);
        }

        double[] IVector.ToArray()
        {
            return VectorImpl.ToArray(this);
        }

        IVector IVector.ForEach(ElementActionByVal action)
        {
            return VectorImpl.ForEach(this, action);
        }

        IVector IVector.ForEach(ElementActionByRef action)
        {
            return VectorImpl.ForEach(this, action);
        }

        IVector IVector.ForEach(ElementActionByValWithIndex action)
        {
            return VectorImpl.ForEach(this, action);
        }

        IVector IVector.ForEach(ElementActionByRefWithIndex action)
        {
            return VectorImpl.ForEach(this, action);
        }

        IVector IVector.Subvector(int startIndex)
        {
            return VectorImpl.Subvector(this, startIndex);
        }

        IVector IVector.Subvector(int startIndex, int length)
        {
            return VectorImpl.Subvector(this, startIndex, length);
        }

        #region 代入演算メソッド

        IVector IVector.Add(double value)
        {
            return VectorImpl.AddEq(this, value);
        }
        IVector IVector.Add(IVector v)
        {
            return VectorImpl.AddEq(this, v);
        }
        IVector IVector.Sub(IVector v)
        {
            return VectorImpl.SubEq(this, v);
        }
        IVector IVector.Mul(double value)
        {
            return VectorImpl.MulEq(this, value);
        }
        IVector IVector.Div(double value)
        {
            return VectorImpl.DivEq(this, value);
        }

        #endregion

        #endregion

        #region IEnumerable メンバ

        IEnumerator IEnumerable.GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion

        #region IEnumerable<double> メンバ

        public IEnumerator<double> GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion
    }
}
