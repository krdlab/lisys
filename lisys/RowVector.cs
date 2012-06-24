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
        #region コンストラクション

        /// <summary>
        /// 指定されたサイズのベクトルを作成する．
        /// </summary>
        /// <param name="size">ベクトルのサイズ（要素数）</param>
        public RowVector(int size)
            : base(size)
        {
        }

        /// <summary>
        /// インタフェースを通して，指定されたオブジェクトのコピーを作成する．
        /// </summary>
        /// <param name="v">コピーされるオブジェクトのインタフェース</param>
        public RowVector(IVector v)
            : base(v)
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

        #endregion

        #region 演算の定義

        /// <summary>
        /// 単項演算子 "+" を適用する．
        /// </summary>
        /// <param name="v"></param>
        /// <returns>引数オブジェクトのクローン</returns>
        public static RowVector operator +(RowVector v)
        {
            return new RowVector(v);
        }

        /// <summary>
        /// 単項演算子 "-" を適用する．
        /// </summary>
        /// <param name="v"></param>
        /// <returns>引数オブジェクトのクローンを符号反転したオブジェクト</returns>
        public static RowVector operator -(RowVector v)
        {
            return (RowVector)new RowVector(v).Flip();
        }


        #region Add
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(RowVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(RowVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(Vector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(RowVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(IVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        #endregion

        #region Sub
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(RowVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(RowVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(Vector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(RowVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(IVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        #endregion

        #region Mul
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(RowVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(RowVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(RowVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(IVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static RowVector operator *(double d, RowVector v)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new RowVector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static RowVector operator *(RowVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new RowVector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static RowVector operator /(RowVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new RowVector(v.Size), v, d);
        }

        #endregion

        /// <summary>
        /// 加算する．
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>加算結果の<see cref="RowVector"/></returns>
        public static new RowVector Add(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 減算する．
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>減算結果の<see cref="RowVector"/></returns>
        public static new RowVector Sub(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }

        /// <summary>
        /// スカラ値を乗算する．
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns>乗算結果の<see cref="RowVector"/></returns>
        public static new RowVector Mul(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new RowVector(v.Size), d, v);
        }

        /// <summary>
        /// スカラ値で除算する．
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns>除算結果の<see cref="RowVector"/></returns>
        public static new RowVector Div(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new RowVector(v.Size), v, d);
        }
    }
}
