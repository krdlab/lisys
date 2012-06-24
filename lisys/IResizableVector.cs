using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// サイズ変更可能なベクトルを表すインタフェース
    /// </summary>
    public interface IResizableVector : IVector
    {
        /// <summary>
        /// ベクトルのサイズを指定された新しいサイズに変更する．
        /// </summary>
        /// <param name="size">新しいベクトルのサイズ</param>
        /// <returns>リサイズ後の自身への参照</returns>
        IVector Resize(int size);
    }
}
