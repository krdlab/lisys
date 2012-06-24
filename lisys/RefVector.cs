using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// コレクションで使用される参照用ベクトルクラス
    /// </summary>
    internal class RefVector : IVector
    {
        private readonly double[] _body;
        private readonly int _begin;
        private readonly int _interval;
        private readonly int _size;

        internal RefVector(double[] body, int begin, int interval, int size)
        {
            this._body = body;
            this._begin = begin;
            this._interval = interval;
            this._size = size;
        }

        public override string ToString()
        {
            return Vectors.ToString(this);
        }

        #region IVector

        public IRandomAccessible<double> this[IEnumerable<int> indexes]
        {
            get { throw new NotImplementedException(); }
        }

        public IRandomAccessible<double> this[params int[] indexes]
        {
            get { throw new NotImplementedException(); }
        }

        public double this[int i]
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

        public Size Size
        {
            get { return new Size(this._size); }
        }

        double IVector.Norm
        {
            get
            {
                return Vectors.Norm(this);
            }
        }

        double IVector.Sum
        {
            get
            {
                return Vectors.Sum(this);
            }
        }

        double IVector.SumSq
        {
            get
            {
                return Vectors.SumSq(this);
            }
        }

        double IVector.Average
        {
            get
            {
                return Vectors.Average(this);
            }
        }

        double IVector.Scatter
        {
            get
            {
                return Vectors.Scatter(this);
            }
        }

        double IVector.SampleVariance
        {
            get
            {
                return Vectors.SampleVariance(this);
            }
        }

        double IVector.UnbiasedVariance
        {
            get
            {
                return Vectors.UnbiasedVariance(this);
            }
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<double> GetEnumerator()
        {
            return Vectors.GetEnumerator(this);
        }

        #endregion

        public void Apply(Func<int, double, double> f)
        {
            int size = this.Size;
            for (int i = 0; i < size; ++i)
            {
                this[i] = f(i, this[i]);
            }
        }

        public void ForEach(Action<int, double> action)
        {
            int size = this.Size;
            for (int i = 0; i < size; ++i)
            {
                action(i, this[i]);
            }
        }
    }
}
