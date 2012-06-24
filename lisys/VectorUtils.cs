using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// クラス横断的に存在するチェック処理を定義する．
    /// </summary>
    internal static class VectorChecker
    {
        /// <summary>
        /// 引数に指定されたベクトルのサイズが0でないことを調べる．
        /// </summary>
        /// <param name="v"></param>
        /// <exception cref="Exception.ZeroSizeException">
        /// ベクトルサイズが0の場合にthrowされる．
        /// </exception>
        public static void ZeroSize(IVector v)
        {
            if (v.Size == 0)
            {
                throw new Exception.ZeroSizeException();
            }
        }

        /// <summary>
        /// 引数に指定されたベクトルのサイズが一致することを調べる．
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <exception cref="Exception.MismatchSizeException">
        /// ベクトルのサイズが一致しないときにthrowされる．
        /// </exception>
        public static void MismatchSize(IVector v1, IVector v2)
        {
            if (v1.Size != v2.Size)
            {
                throw new Exception.MismatchSizeException();
            }
        }

        public static void ValueIsLessThanLimit(double value)
        {
            if (krdlab.law.CalculationChecker.IsLessThanLimit(value))
            {
                throw new Exception.ValueIsLessThanLimitException();
            }
        }
    }

}
