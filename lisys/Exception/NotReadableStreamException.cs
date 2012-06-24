using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// 読み込み可能でないStreamに対して，読み込み操作を行ったときにthrowされる．
    /// </summary>
    public class NotReadableStreamException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public NotReadableStreamException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public NotReadableStreamException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public NotReadableStreamException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
