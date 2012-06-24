using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// �����s�񂪗v�����ꂽ�ɂ�������炸�C�����s��ł͂Ȃ������ꍇ��throw�����D
    /// </summary>
    public class NotSquareMatrixException : LisysException
    {
        /// <summary>
        /// ���b�Z�[�W�����ō쐬
        /// </summary>
        public NotSquareMatrixException()
        { }
        /// <summary>
        /// ���b�Z�[�W����ō쐬
        /// </summary>
        /// <param name="message"></param>
        public NotSquareMatrixException(string message)
            : base(message)
        { }
        /// <summary>
        /// ���b�Z�[�W�ƁC���̗�O���������錴���ƂȂ�����O�I�u�W�F�N�g���w�肵�č쐬
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NotSquareMatrixException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
