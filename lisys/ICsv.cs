using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// CSV�`���ɕϊ��\�ł��邱�Ƃ������C���^�t�F�[�X
    /// </summary>
    public interface ICsv
    {
        /// <summary>
        /// ���̃I�u�W�F�N�g��CSV�`���ɕϊ�����D
        /// </summary>
        /// <returns>CSV�`���̕�����</returns>
        string ToCsv();
    }
}
