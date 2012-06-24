using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 部分要素抽出のための内部クラス
    /// </summary>
    internal class SubVector : IVector
    {
        private readonly IVector _body;
        private readonly IEnumerable<int> _indexes;

        internal SubVector(IVector body, IEnumerable<int> indexes)
        {
            this._body = body;
            this._indexes = indexes;
        }

        public double this[int i]
        {
            get
            {
                return this._body[this._indexes.ElementAt(i)];
            }
            set
            {
                this._body[this._indexes.ElementAt(i)] = value;
            }
        }

        public Size Size
        {
            get { return new Size(this._indexes.Count()); }
        }

        double IVector.Norm
        {
            get { return Vectors.Norm(this); }
        }

        double IVector.Sum
        {
            get { return Vectors.Sum(this); }
        }

        double IVector.SumSq
        {
            get { return Vectors.SumSq(this); }
        }

        double IVector.Average
        {
            get { return Vectors.Average(this); }
        }

        double IVector.Scatter
        {
            get { return Vectors.Scatter(this); }
        }

        double IVector.SampleVariance
        {
            get { return Vectors.SampleVariance(this); }
        }

        double IVector.UnbiasedVariance
        {
            get { return Vectors.UnbiasedVariance(this); }
        }

        public void Apply(Func<int, double, double> f)
        {
            throw new NotImplementedException();    // XXX
        }


        public void ForEach(Action<int, double> action)
        {
            throw new NotImplementedException();    // XXX
        }

        public IRandomAccessible<double> this[IEnumerable<int> indexes]
        {
            get { return this[indexes.ToArray()]; }
        }

        public IRandomAccessible<double> this[params int[] indexes]
        {
            get
            {
                int ownerSize = this._indexes.Count();
                if (ownerSize < indexes.Count())
                {
                    throw new ArgumentException();
                }
                if (indexes.Any(i => i < 0 || ownerSize < i))
                {
                    throw new ArgumentException();
                }
                return new SubVector(this, indexes);
            }
        }

        public override string ToString()
        {
            return Vectors.ToString(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<double> GetEnumerator()
        {
            return Vectors.GetEnumerator(this);
        }
    }
}
