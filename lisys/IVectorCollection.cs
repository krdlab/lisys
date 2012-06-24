using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// Vectorコレクションクラスのインタフェース
    /// </summary>
    public interface IVectorCollection<out C> : IRandomAccessible<IVector>
        where C : IVectorCollection<C>
    {
        IVector this[int index]
        {
            set;
            get;
        }

        IVectorCollection<C> this[params int[] indexes]
        {
            get;
        }

        IVectorCollection<C> this[IEnumerable<int> indexes]
        {
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
        /// 指定されたIndexのコレクション要素を入れ替える．
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>入れ替え後の自身への参照</returns>
        IVectorCollection<C> Swap(int index1, int index2);
    }
}
