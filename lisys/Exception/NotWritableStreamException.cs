using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// �������݉\�łȂ�Stream�ɑ΂��āC�������ݑ�����s�����Ƃ���throw�����D
    /// </summary>
    public class NotWritableStreamException : LisysException
    {
        /// <summary>
        /// �����Ȃ��ō쐬����D
        /// </summary>
        public NotWritableStreamException()
        { }

        /// <summary>
        /// ���b�Z�[�W���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public NotWritableStreamException(string message)
            : base(message)
        { }

        /// <summary>
        /// ���b�Z�[�W�ƁC���������ƂȂ�����O���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="inner">��O�I�u�W�F�N�g</param>
        public NotWritableStreamException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
