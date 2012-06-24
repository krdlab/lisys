using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// �l���C���C�u�����ň������x�̉����l�����ł���Ƃ���throw�����D
    /// </summary>
    public class ValueIsLessThanLimitException : LisysException
    {
        /// <summary>
        /// �����Ȃ��ō쐬����D
        /// </summary>
        public ValueIsLessThanLimitException()
        { }

        /// <summary>
        /// ���b�Z�[�W���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public ValueIsLessThanLimitException(string message)
            : base(message)
        { }

        /// <summary>
        /// ���b�Z�[�W�ƁC���������ƂȂ�����O���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="inner">��O�I�u�W�F�N�g</param>
        public ValueIsLessThanLimitException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
