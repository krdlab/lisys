using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 行ベクトル
    /// </summary>
    [Serializable]
    public class RowVector : Vector
    {
        internal RowVector()
        {
        }

        /// <summary>
        /// 指定されたサイズのベクトルを作成する．
        /// </summary>
        /// <param name="size"></param>
        public RowVector(Size size)
            : base(size)
        {
        }

        /// <summary>
        /// コピーを作成する．
        /// </summary>
        /// <param name="v"></param>
        public RowVector(Vector v)
            : this(v._body)
        {
        }

        /// <summary>
        /// 任意の個数の要素を直接指定してベクトルを作成する．
        /// </summary>
        /// <param name="arr">任意の個数のベクトルの要素</param>
        public RowVector(params double[] arr)
            : base(arr)
        {
        }

        /// <summary>
        /// コピーを作成する．
        /// </summary>
        /// <param name="v"></param>
        public RowVector(IEnumerable<double> v)
            : base(v)
        {
        }

        #region 演算の定義

        /// <summary>
        /// </summary>
        /// <returns>引数オブジェクトのコピー</returns>
        public static RowVector operator +(RowVector v)
        {
            return new RowVector(v);
        }

        /// <summary>
        /// </summary>
        /// <returns>引数オブジェクトのコピーを符号反転したオブジェクト</returns>
        public static RowVector operator -(RowVector v)
        {
            return (RowVector)new RowVector(v).Flip();
        }

        /// <summary>
        /// </summary>
        public static RowVector operator +(RowVector l, RowVector r)
        {
            return new RowVector(plus(l, r));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator -(RowVector l, RowVector r)
        {
            return new RowVector(sub(l, r));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator *(double d, RowVector v)
        {
            return new RowVector(scala(d, v));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator *(RowVector v, double d)
        {
            return new RowVector(scala(d, v));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator /(RowVector v, double d)
        {
            return new RowVector(scala(1 / d, v));
        }

        #endregion
    }
}
