using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// サイズが0のオブジェクトを扱おうとしたときにthrowされる
    /// </summary>
    public class ZeroSizeException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public ZeroSizeException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public ZeroSizeException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public ZeroSizeException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
