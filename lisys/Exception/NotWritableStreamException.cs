using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// 書き込み可能でないStreamに対して，書き込み操作を行ったときにthrowされる．
    /// </summary>
    public class NotWritableStreamException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public NotWritableStreamException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public NotWritableStreamException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public NotWritableStreamException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
