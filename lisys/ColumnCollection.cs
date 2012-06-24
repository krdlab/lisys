using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 行列の列ベクトルコレクション
    /// </summary>
    public class ColumnCollection : VectorCollection<ColumnCollection>
    {
        private readonly double[] _body;
        private readonly int _rsize;
        private readonly int _csize;

        internal ColumnCollection(double[] body, int rowSize, int columnSize)
        {
            this._body = body;
            this._rsize = rowSize;
            this._csize = columnSize;
        }

        protected override IVector GetBy(int c)
        {
            if (c < 0 || this.Count <= c)
            {
                throw new ArgumentOutOfRangeException();
            }
            return new RefVector(this._body, c * this._rsize, 1, this._rsize);
        }

        public override IVectorCollection<ColumnCollection> this[params int[] indexes]
        {
            get
            {
                CollectionChecker.HasIndexes(this, indexes);
                return new SubCollection<ColumnCollection>(this, indexes);
            }
        }

        public override IVectorCollection<ColumnCollection> this[IEnumerable<int> indexes]
        {
            get { return this[indexes.ToArray()]; }
        }

        public override Size Size
        {
            get { return new Size(this._csize); }
        }
    }
}
