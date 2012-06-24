using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Exception
{
    /// <summary>
    /// �T�C�Y����v���Ȃ��Ƃ���throw�����D
    /// </summary>
    /// <remarks>
    /// <para>��Ƃ��āC�ȉ��̂悤�ȏ�Ԃ���������D</para>
    /// <list type="bullet">
    /// <item>
    /// <description>���� a�Eb ���v�Z����Ƃ��C|a| != |b|�ł�����</description>
    /// </item>
    /// <item>
    /// <description>�s��̏�Z XY ������Ƃ��CX = [x_row, x_col], Y = [y_row, y_col], x_col != y_row �ł�����</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class MismatchSizeException : LisysException
    {
        /// <summary>
        /// �����Ȃ��ō쐬����D
        /// </summary>
        public MismatchSizeException()
        { }

        /// <summary>
        /// ���b�Z�[�W���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public MismatchSizeException(string message)
            : base(message)
        { }

        /// <summary>
        /// ���b�Z�[�W�ƁC���������ƂȂ�����O���w�肵�č쐬����D
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="inner">��O�I�u�W�F�N�g</param>
        public MismatchSizeException(string message, System.Exception inner)
            : base(message, inner)
        { }
    }
}
