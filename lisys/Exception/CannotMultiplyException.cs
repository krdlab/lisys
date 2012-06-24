using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// ��Z���ł��Ȃ��Ƃ���throw�����D
    /// </summary>
    public class CannotMultiplyException : LisysException
    {
        /// <summary>
        /// �����Ȃ��ō쐬����D
        /// </summary>
        public CannotMultiplyException()
        { }

        /// <summary>
        /// ���b�Z�[�W���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public CannotMultiplyException(string message)
            : base(message)
        { }

        /// <summary>
        /// ���b�Z�[�W�ƁC���������ƂȂ�����O���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="inner">��O�I�u�W�F�N�g</param>
        public CannotMultiplyException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
