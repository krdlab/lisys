using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// <see cref="IVectorCollection.ForEach(VectorCollectionAction)"/>�Ŏg�p�����f���Q�[�g
    /// </summary>
    /// <param name="v">�R���N�V�������ێ�����<see cref="IVector"/></param>
    public delegate void VectorCollectionAction(IVector v);

    /// <summary>
    /// <see cref="IVectorCollection.ForEach(VectorCollectionActionWithIndex)"/>�Ŏg�p�����f���Q�[�g
    /// </summary>
    /// <param name="i"><see cref="IVector"/>��index</param>
    /// <param name="v">�R���N�V�������ێ�����<see cref="IVector"/></param>
    public delegate void VectorCollectionActionWithIndex(int i, IVector v);

    /// <summary>
    /// Vector�R���N�V�����N���X�̃C���^�t�F�[�X
    /// </summary>
    public interface IVectorCollection : IEnumerable<IVector>
    {
        /// <summary>
        /// �R���N�V�����̗v�f�̃I�u�W�F�N�g��ݒ�E�擾����D
        /// </summary>
        /// <param name="i">�R���N�V����index</param>
        /// <returns>IVector�C���^�t�F�[�X</returns>
        IVector this[int i]
        {
            set;
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
        /// �ێ�����eVector�I�u�W�F�N�g��action��K�p����D
        /// </summary>
        /// <param name="action">index�����̃A�N�V����</param>
        /// <returns>ForEach���\�b�h���K�p���ꂽ�R���N�V�����I�u�W�F�N�g</returns>
        IVectorCollection ForEach(VectorCollectionAction action);

        /// <summary>
        /// �ێ�����eVector�I�u�W�F�N�g��action��K�p����D
        /// </summary>
        /// <param name="action">index�L��̃A�N�V����</param>
        /// <returns>ForEach���\�b�h���K�p���ꂽ�R���N�V�����I�u�W�F�N�g</returns>
        IVectorCollection ForEach(VectorCollectionActionWithIndex action);

        /// <summary>
        /// �����R���N�V������Ԃ��D
        /// </summary>
        /// <param name="startIndex">�J�nindex</param>
        /// <returns><paramref name="startIndex"/>����<c><see cref="IVectorCollection.Count"/>-1</c>�܂ł̕����R���N�V����</returns>
        IVectorCollection Subcollection(int startIndex);

        /// <summary>
        /// �����R���N�V������Ԃ��D
        /// </summary>
        /// <param name="startIndex">�J�nindex</param>
        /// <param name="count">�擾����R���N�V�����̐�</param>
        /// <returns><paramref name="startIndex"/>����͂��܂钷��<paramref name="count"/>�̕����R���N�V����</returns>
        IVectorCollection Subcollection(int startIndex, int count);

        /// <summary>
        /// �ێ�����R���N�V��������C<see cref="Matrix"/>���쐬����D
        /// </summary>
        Matrix ToMatrix();

        /// <summary>
        /// �w�肳�ꂽIndex�̃R���N�V�����v�f�����ւ���D
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>����ւ���̎��g�ւ̎Q��</returns>
        IVectorCollection Swap(int index1, int index2);
    }
}
