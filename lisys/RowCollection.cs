using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 行列が持つ行ベクトルコレクション
    /// </summary>
    public class RowCollection : VectorCollection<RowCollection>
    {
        private readonly double[] _body;
        private readonly int _rsize;
        private readonly int _csize;

        internal RowCollection(double[] body, int rowSize, int columnSize)
        {
            this._body = body;
            this._rsize = rowSize;
            this._csize = columnSize;
        }

        protected override IVector GetBy(int r)
        {
            if (r < 0 || this.Count <= r)
            {
                throw new IndexOutOfRangeException();
            }
            return new RefVector(this._body, r, this._rsize, this._csize);
        }

        public override IVectorCollection<RowCollection> this[params int[] indexes]
        {
            get
            {
                CollectionChecker.HasIndexes(this, indexes);
                return new SubCollection<RowCollection>(this, indexes);
            }
        }

        public override IVectorCollection<RowCollection> this[IEnumerable<int> indexes]
        {
            get { return this[indexes.ToArray()]; }
        }

        public override Size Size
        {
            get { return new Size(this._rsize); }
        }
    }
}
