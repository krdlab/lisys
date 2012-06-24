using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// サイズが一致しないときにthrowされる．
    /// </summary>
    /// <remarks>
    /// <para>例として，以下のような状態があげられる．</para>
    /// <list type="bullet">
    /// <item>
    /// <description>内積 a・b を計算するとき，|a| != |b|であった</description>
    /// </item>
    /// <item>
    /// <description>行列の乗算 XY をするとき，X = [x_row, x_col], Y = [y_row, y_col], x_col != y_row であった</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class MismatchSizeException : LisysException
    {
        /// <summary>
        /// 引数なしで作成する．
        /// </summary>
        public MismatchSizeException()
        { }

        /// <summary>
        /// メッセージを指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        public MismatchSizeException(string message)
            : base(message)
        { }

        /// <summary>
        /// メッセージと，発生原因となった例外を指定して作成する．
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">例外オブジェクト</param>
        public MismatchSizeException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
