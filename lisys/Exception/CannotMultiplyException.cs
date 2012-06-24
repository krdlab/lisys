using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// 乗算ができないときにthrowされる．
    /// </summary>
    public class CannotMultiplyException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public CannotMultiplyException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public CannotMultiplyException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public CannotMultiplyException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
