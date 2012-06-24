using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ベクトルコレクションの抽象．行列のベクトル単位でのアクセスを定義する．
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public abstract class VectorCollection<C> : IVectorCollection<C>
        where C : IVectorCollection<C>
    {
        public IVector this[int index]
        {
            get
            {
                return GetBy(index);
            }
            set
            {
                IVector dst = this[index];
                IVector src = value;
                for (int i = 0; i < src.Size; ++i)
                {
                    dst[i] = src[i];
                }
            }
        }

        IRandomAccessible<IVector> IRandomAccessible<IVector>.this[params int[] indexes]
        {
            get { return this[indexes]; }
        }

        IRandomAccessible<IVector> IRandomAccessible<IVector>.this[IEnumerable<int> indexes]
        {
            get { return this[indexes]; }
        }

        public int Count
        {
            get { return this.Size; }
        }

        public IVectorCollection<C> Swap(int index1, int index2)
        {
            if (index1 != index2)
            {
                if (index1 < 0 || this.Count <= index1
                    || index2 < 0 || this.Count <= index2)
                {
                    throw new ArgumentOutOfRangeException();
                }
                IVector v1 = this[index1];
                IVector v2 = this[index2];
                for (int i = 0; i < v1.Size; ++i)
                {
                    double val = v1[i];
                    v1[i] = v2[i];
                    v2[i] = val;
                }
            }
            return this;
        }

        public void ForEach(Action<int, IVector> action)
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                action(i, this[i]);
            }
        }

        public void Apply(Func<int, IVector, IVector> f)
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                this[i] = f(i, this[i]);
            }
        }

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IVector> GetEnumerator()
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                yield return this[i];
            }
        }

        #endregion

        #region Abstract

        protected abstract IVector GetBy(int index);

        public abstract IVectorCollection<C> this[params int[] indexes]
        {
            get;
        }

        public abstract IVectorCollection<C> this[IEnumerable<int> indexes]
        {
            get;
        }

        public abstract Size Size
        {
            get;
        }

        #endregion
    }
}
