using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// Vector�R���N�V�����N���X�̃C���^�t�F�[�X
    /// </summary>
    public interface IVectorCollection<out C> : IRandomAccessible<IVector>
        where C : IVectorCollection<C>
    {
        IVector this[int index]
        {
            set;
            get;
        }

        IVectorCollection<C> this[params int[] indexes]
        {
            get;
        }

        IVectorCollection<C> this[IEnumerable<int> indexes]
        {
            get;
        }

        /// <summary>
        /// �ێ�����R���N�V���������擾����D
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// �w�肳�ꂽIndex�̃R���N�V�����v�f�����ւ���D
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>����ւ���̎��g�ւ̎Q��</returns>
        IVectorCollection<C> Swap(int index1, int index2);
    }
}
