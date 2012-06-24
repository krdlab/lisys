using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// �T�C�Y��0�̃I�u�W�F�N�g���������Ƃ����Ƃ���throw�����
    /// </summary>
    public class ZeroSizeException : LisysException
    {
        /// <summary>
        /// �����Ȃ��ō쐬����D
        /// </summary>
        public ZeroSizeException()
        { }

        /// <summary>
        /// ���b�Z�[�W���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public ZeroSizeException(string message)
            : base(message)
        { }

        /// <summary>
        /// ���b�Z�[�W�ƁC���������ƂȂ�����O���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="inner">��O�I�u�W�F�N�g</param>
        public ZeroSizeException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
