using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// <see cref="IVectorCollection.ForEach(VectorCollectionAction)"/>で使用されるデリゲート
    /// </summary>
    /// <param name="v">コレクションが保持する<see cref="IVector"/></param>
    public delegate void VectorCollectionAction(IVector v);

    /// <summary>
    /// <see cref="IVectorCollection.ForEach(VectorCollectionActionWithIndex)"/>で使用されるデリゲート
    /// </summary>
    /// <param name="i"><see cref="IVector"/>のindex</param>
    /// <param name="v">コレクションが保持する<see cref="IVector"/></param>
    public delegate void VectorCollectionActionWithIndex(int i, IVector v);

    /// <summary>
    /// Vectorコレクションクラスのインタフェース
    /// </summary>
    public interface IVectorCollection : IEnumerable<IVector>
    {
        /// <summary>
        /// コレクションの要素のオブジェクトを設定・取得する．
        /// </summary>
        /// <param name="i">コレクションindex</param>
        /// <returns>IVectorインタフェース</returns>
        IVector this[int i]
        {
            set;
            get;
        }

        /// <summary>
        /// 保持するコレクション数を取得する．
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 保持する各Vectorオブジェクトにactionを適用する．
        /// </summary>
        /// <param name="action">index無しのアクション</param>
        /// <returns>ForEachメソッドが適用されたコレクションオブジェクト</returns>
        IVectorCollection ForEach(VectorCollectionAction action);

        /// <summary>
        /// 保持する各Vectorオブジェクトにactionを適用する．
        /// </summary>
        /// <param name="action">index有りのアクション</param>
        /// <returns>ForEachメソッドが適用されたコレクションオブジェクト</returns>
        IVectorCollection ForEach(VectorCollectionActionWithIndex action);

        /// <summary>
        /// 部分コレクションを返す．
        /// </summary>
        /// <param name="startIndex">開始index</param>
        /// <returns><paramref name="startIndex"/>から<c><see cref="IVectorCollection.Count"/>-1</c>までの部分コレクション</returns>
        IVectorCollection Subcollection(int startIndex);

        /// <summary>
        /// 部分コレクションを返す．
        /// </summary>
        /// <param name="startIndex">開始index</param>
        /// <param name="count">取得するコレクションの数</param>
        /// <returns><paramref name="startIndex"/>からはじまる長さ<paramref name="count"/>の部分コレクション</returns>
        IVectorCollection Subcollection(int startIndex, int count);

        /// <summary>
        /// 保持するコレクションから，<see cref="Matrix"/>を作成する．
        /// </summary>
        Matrix ToMatrix();

        /// <summary>
        /// 指定されたIndexのコレクション要素を入れ替える．
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>入れ替え後の自身への参照</returns>
        IVectorCollection Swap(int index1, int index2);
    }
}
