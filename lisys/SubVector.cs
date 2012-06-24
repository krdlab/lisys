using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �����v�f���o�̂��߂̓����N���X
    /// </summary>
    internal class SubVector : IVector
    {
        private IVector _body = null;
        private int[] _indexes = null;

        internal SubVector(IVector body, int[] indexes)
        {
            this._body = body;
            this._indexes = indexes;
        }

        #region IVector �����o

        double IVector.this[int i]
        {
            get
            {
                return this._body[this._indexes[i]];
            }
            set
            {
                this._body[this._indexes[i]] = value;
            }
        }

        int IVector.Size
        {
            get { return this._indexes.Length; }
        }

        double IVector.Norm
        {
            get { return VectorImpl.Norm(this); }
        }

        double IVector.Sum
        {
            get { return VectorImpl.Sum(this); }
        }

        double IVector.SumSq
        {
            get { return VectorImpl.SumSq(this); }
        }

        double IVector.Average
        {
            get { return VectorImpl.Average(this); }
        }

        double IVector.Scatter
        {
            get { return VectorImpl.Scatter(this); }
        }

        double IVector.Variance
        {
            get { return VectorImpl.SampleVariance(this); }
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

        #region ������Z���\�b�h

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

        public override string ToString()
        {
            return VectorImpl.ToString(this);
        }

        #region IEnumerable �����o

        IEnumerator IEnumerable.GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion

        #region IEnumerable<double> �����o

        public IEnumerator<double> GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion
    }
}
