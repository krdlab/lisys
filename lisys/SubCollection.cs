using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 行列が持つベクトルコレクションのサブコレクション
    /// </summary>
    internal class SubCollection<Owner> : VectorCollection<Owner>
        where Owner : IVectorCollection<Owner>
    {
        private readonly IVectorCollection<Owner> _body;
        private readonly int[] _indexes;

        internal SubCollection(IVectorCollection<Owner> body, int[] indexes)
        {
            this._body = body;
            this._indexes = indexes;
        }

        protected override IVector GetBy(int index)
        {
            return this._body[this._indexes[index]];
        }

        public override IVectorCollection<Owner> this[params int[] indexes]
        {
            get
            {
                CollectionChecker.HasIndexes(this, indexes);
                return new SubCollection<Owner>(this, indexes);
            }
        }

        public override IVectorCollection<Owner> this[IEnumerable<int> indexes]
        {
            get { return this[indexes.ToArray()]; }
        }

        public override Size Size
        {
            get { return new Size(this._indexes.Length); }
        }
    }
}
