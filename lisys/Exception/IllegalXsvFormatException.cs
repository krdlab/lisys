using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// XsvFormatの内容が不正な場合にthrowされる．
    /// </summary>
    public class IllegalXsvFormatException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public IllegalXsvFormatException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public IllegalXsvFormatException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public IllegalXsvFormatException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
