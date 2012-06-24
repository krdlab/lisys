using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 行列が持つ列ベクトルコレクション
    /// </summary>
    public class ColumnCollection : IVectorCollection
    {
        private double[] _body = null;
        private int _rsize = 0;
        private int _csize = 0;

        internal ColumnCollection(double[] body, int rowSize, int columnSize)
        {
            this._body = body;
            this._rsize = rowSize;
            this._csize = columnSize;
        }

        #region IVectorCollection メンバ

        /// <summary>
        /// 列コレクションから，指定した列の取得・設定を行う．
        /// </summary>
        /// <param name="c">列index</param>
        /// <returns>列ベクトル</returns>
        public IVector this[int c]
        {
            get
            {
                if (c < 0 || this.Count <= c)
                {
                    throw new IndexOutOfRangeException();
                }
                return new RefVector(this._body, c * this._rsize, 1, this._rsize);
            }
            set
            {
                CollectionImpl.Setter(this, c, value);
            }
        }

        /// <summary>
        /// 列数を取得する．
        /// </summary>
        public int Count
        {
            get { return this._csize; }
        }

        /// <summary>
        /// コレクションに action を適用する．
        /// </summary>
        /// <param name="action"><see cref="VectorCollectionAction"/>で規定されるアクション</param>
        /// <returns>適用後の自身への参照</returns>
        public IVectorCollection ForEach(VectorCollectionAction action)
        {
            return CollectionImpl.ForEach(this, action);
        }

        /// <summary>
        /// コレクションに action を適用する．
        /// </summary>
        /// <param name="action"><see cref="VectorCollectionActionWithIndex"/>で規定されるアクション</param>
        /// <returns>適用後の自身への参照</returns>
        public IVectorCollection ForEach(VectorCollectionActionWithIndex action)
        {
            return CollectionImpl.ForEach(this, action);
        }

        /// <summary>
        /// 部分コレクションを返す．
        /// </summary>
        /// <param name="startIndex">開始index</param>
        /// <returns><paramref name="startIndex"/>から<c><see cref="IVectorCollection.Count"/>-1</c>までの部分コレクション</returns>
        public IVectorCollection Subcollection(int startIndex)
        {
            return CollectionImpl.Subcollection(this, CollectionType.Columns, startIndex);
        }

        /// <summary>
        /// 部分コレクションを返す．
        /// </summary>
        /// <param name="startIndex">開始index</param>
        /// <param name="count">取得するコレクションの数</param>
        /// <returns><paramref name="startIndex"/>からはじまる長さ<paramref name="count"/>の部分コレクション</returns>
        public IVectorCollection Subcollection(int startIndex, int count)
        {
            return CollectionImpl.Subcollection(this, CollectionType.Columns, startIndex, count);
        }

        /// <summary>
        /// 保持しているコレクションから，新しい<see cref="Matrix"/>オブジェクトを作成する．
        /// </summary>
        /// <returns><see cref="Matrix"/>オブジェクト</returns>
        public Matrix ToMatrix()
        {
            return CollectionImpl.ColumnsToMatrix(this);
        }

        /// <summary>
        /// 指定された列を入れ替える．
        /// </summary>
        /// <param name="index1">列Index</param>
        /// <param name="index2">列Index</param>
        /// <returns>入れ替え後の自身への参照</returns>
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

        /// <summary>
        /// 列挙子を取得する．
        /// </summary>
        /// <returns>列挙子</returns>
        public IEnumerator<IVector> GetEnumerator()
        {
            return CollectionImpl.Enumerator(this);
        }

        #endregion

        /// <summary>
        /// 列コレクションから指定された列を取得する．
        /// </summary>
        /// <param name="indexes">取得したい列のindex配列</param>
        /// <returns>引数で指定された列セットの列コレクション</returns>
        public IVectorCollection this[params int[] indexes]
        {
            get
            {
                CollectionImpl.CheckIndexes(this, indexes);
                return new SubCollection(this, indexes, CollectionType.Columns);
            }
        }

    }
}
