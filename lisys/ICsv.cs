using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// CSV形式に変換可能であることを示すインタフェース
    /// </summary>
    public interface ICsv
    {
        /// <summary>
        /// このオブジェクトをCSV形式に変換する．
        /// </summary>
        /// <returns>CSV形式の文字列</returns>
        string ToCsv();
    }
}
