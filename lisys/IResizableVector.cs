using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �T�C�Y�ύX�\�ȃx�N�g����\���C���^�t�F�[�X
    /// </summary>
    public interface IResizableVector : IVector
    {
        /// <summary>
        /// �x�N�g���̃T�C�Y���w�肳�ꂽ�V�����T�C�Y�ɕύX����D
        /// </summary>
        /// <param name="size">�V�����x�N�g���̃T�C�Y</param>
        /// <returns>���T�C�Y��̎��g�ւ̎Q��</returns>
        IVector Resize(int size);
    }
}
