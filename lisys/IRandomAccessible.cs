using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 2 つのパラメータを取り，値を返すアクション (関数)．
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public delegate R ReturnableAction<in T1, in T2, out R>(T1 t1, T2 t2);

    /// <summary>
    /// ランダムアクセス可能であることを示す型クラス．
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRandomAccessible<T> : IEnumerable<T>
    {
        /// <summary>
        /// 要素の設定・取得を行う．
        /// </summary>
        /// <param name="i">要素の位置 (0 始まり)</param>
        /// <returns>指定された位置の要素</returns>
        T this[int i]
        {
            set;
            get;
        }

        IRandomAccessible<T> this[params int[] indexes]     // XXX いらない気がする
        {
            get;
        }

        /// <summary>
        /// サブセットを返す．
        /// </summary>
        /// <param name="indexes">取得したい位置</param>
        /// <returns>サブセット</returns>
        IRandomAccessible<T> this[IEnumerable<int> indexes] // XXX いらない気がする
        {
            get;
        }

        /// <summary>
        /// 要素数を取得する．
        /// </summary>
        Size Size
        {
            get;
        }

        /// <summary>
        /// 各要素に対してアクションを実行する．
        /// </summary>
        /// <param name="action">アクション</param>
        void ForEach(Action<int, T> action);

        /// <summary>
        /// map 操作．
        /// </summary>
        /// <param name="f">関数</param>
        void Apply(Func<int, T, T> f);
    }
}
