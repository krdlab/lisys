using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// 引数が不正の場合にthrowされる．
    /// </summary>
    public class IllegalArgumentException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public IllegalArgumentException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public IllegalArgumentException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public IllegalArgumentException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
