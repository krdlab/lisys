using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// 値が，ライブラリで扱う精度の下限値未満であるときにthrowされる．
    /// </summary>
    public class ValueIsLessThanLimitException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public ValueIsLessThanLimitException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public ValueIsLessThanLimitException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public ValueIsLessThanLimitException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
