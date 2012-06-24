using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// XSV�̏�Ԃ��s���ȂƂ���throw�����D
    /// </summary>
    public class IllegalXsvStateException : LisysException
    {
        /// <summary>
        /// �����Ȃ��ō쐬����D
        /// </summary>
        public IllegalXsvStateException()
        { }

        /// <summary>
        /// ���b�Z�[�W���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public IllegalXsvStateException(string message)
            : base(message)
        { }

        /// <summary>
        /// ���b�Z�[�W�ƁC���������ƂȂ�����O���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="inner">��O�I�u�W�F�N�g</param>
        public IllegalXsvStateException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
