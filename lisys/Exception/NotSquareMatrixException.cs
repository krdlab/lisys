using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// 正方行列が要求されたにもかかわらず，正方行列ではなかった場合にthrowされる．
    /// </summary>
    public class NotSquareMatrixException : LisysException
    {
        /// <summary>
        /// メッセージ無しで作成
        /// </summary>
        public NotSquareMatrixException()
        { }
        /// <summary>
        /// メッセージありで作成
        /// </summary>
        /// <param name="message"></param>
        public NotSquareMatrixException(string message)
            : base(message)
        { }
        /// <summary>
        /// メッセージと，この例外が発生する原因となった例外オブジェクトを指定して作成
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NotSquareMatrixException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
