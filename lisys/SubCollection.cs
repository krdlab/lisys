using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 行列が持つベクトルコレクションのサブコレクション
    /// </summary>
    internal class SubCollection : IVectorCollection
    {
        private IVectorCollection _body = null;
        private int[] _indexes = null;
        private CollectionType _type;

        internal SubCollection(IVectorCollection body, int[] indexes, CollectionType type)
        {
            this._body = body;
            this._indexes = indexes;
            this._type = type;
        }

        #region IVectorCollection メンバ

        public IVector this[int index]
        {
            get
            {
                return this._body[this._indexes[index]];
            }
            set
            {
                CollectionImpl.Setter(this, index, value);
            }
        }

        public int Count
        {
            get { return this._indexes.Length; }
        }

        public IVectorCollection ForEach(VectorCollectionAction action)
        {
            return CollectionImpl.ForEach(this, action);
        }

        public IVectorCollection ForEach(VectorCollectionActionWithIndex action)
        {
            return CollectionImpl.ForEach(this, action);
        }

        public IVectorCollection Subcollection(int startIndex)
        {
            return CollectionImpl.Subcollection(this, this._type, startIndex);
        }

        public IVectorCollection Subcollection(int startIndex, int length)
        {
            return CollectionImpl.Subcollection(this, this._type, startIndex, length);
        }

        Matrix IVectorCollection.ToMatrix()
        {
            return CollectionImpl.ToMatrix(this, this._type);
        }

        public IVectorCollection Swap(int index1, int index2)
        {
            return CollectionImpl.Swap(this, index1, index2);
        }

        #endregion

        #region IEnumerable メンバ

        IEnumerator IEnumerable.GetEnumerator()
        {
            return CollectionImpl.Enumerator(this);
        }

        #endregion

        #region IEnumerable<IVector> メンバ

        public IEnumerator<IVector> GetEnumerator()
        {
            return CollectionImpl.Enumerator(this);
        }

        #endregion
    }
}
