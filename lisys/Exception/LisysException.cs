using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// Lisys�����Ŕ��������O�̊��N���X
    /// </summary>
    public class LisysException : System.Exception
    {
        internal LisysException()
        { }
        internal LisysException(string message)
            : base(message)
        { }
        internal LisysException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
