using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByVal)"/>�̂��߂̃f���Q�[�g
    /// </summary>
    /// <remarks>
    /// <code>
    /// foreach element in elements:
    ///     action(element)
    /// </code>
    /// </remarks>
    /// <param name="val"><see cref="IVector"/>���ێ�����v�f�̒l</param>
    public delegate void ElementActionByVal(double val);

    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByRef)"/>�̂��߂̃f���Q�[�g
    /// </summary>
    /// <remarks>
    /// <code>
    /// foreach element in elements:
    ///     action(ref element)
    /// </code>
    /// </remarks>
    /// <param name="val"><see cref="IVector"/>���ێ�����v�f�̒l</param>
    public delegate void ElementActionByRef(ref double val);

    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByValWithIndex)"/>�̂��߂̃f���Q�[�g
    /// </summary>
    /// <remarks>
    /// <code>
    /// for i in [0..Size-1]:
    ///     action(i, v[i])
    /// </code>
    /// </remarks>
    /// <param name="i"><see cref="IVector"/>���ێ�����v�f��index</param>
    /// <param name="val"><see cref="IVector"/>���ێ�����v�f�̒l</param>
    public delegate void ElementActionByValWithIndex(int i, double val);

    /// <summary>
    /// <see cref="IVector.ForEach(ElementActionByRefWithIndex)"/>�̂��߂̃f���Q�[�g
    /// </summary>
    /// <remarks>
    /// <code>
    /// for i in [0..Size-1]:
    ///     action(i, ref v[i])
    /// </code>
    /// </remarks>
    /// <param name="i"><see cref="IVector"/>���ێ�����v�f��index</param>
    /// <param name="val"><see cref="IVector"/>���ێ�����v�f�̒l</param>
    public delegate void ElementActionByRefWithIndex(int i, ref double val);

    /// <summary>
    /// �x�N�g���C���^�t�F�[�X
    /// </summary>
    public interface IVector : IEnumerable<double>
    {
        /// <summary>
        /// �v�f�̐ݒ�E�擾���s���D
        /// </summary>
        /// <param name="i">�v�f��index</param>
        /// <returns>�w�肳�ꂽindex�����v�f</returns>
        double this[int i]
        {
            set;
            get;
        }

        /// <summary>
        /// �v�f�����擾����D
        /// </summary>
        int Size
        {
            get;
        }

        /// <summary>
        /// �m�������擾����D
        /// </summary>
        double Norm
        {
            get;
        }

        /// <summary>
        /// �v�f�̍��v���擾����D
        /// </summary>
        double Sum
        {
            get;
        }

        /// <summary>
        /// �v�f�̓��a���擾����D
        /// </summary>
        double SumSq
        {
            get;
        }

        /// <summary>
        /// �v�f�̕��ϒl���擾����D
        /// </summary>
        double Average
        {
            get;
        }

        /// <summary>
        /// �v�f�̎U�z�l���擾����D
        /// </summary>
        /// <remarks>
        /// <code>
        /// val = 0.0;
        /// avg = this.Average;
        /// foreach(e in this)
        /// {
        ///     val += ((e - avg) * (e - avg));
        /// }
        /// </code>
        /// </remarks>
        double Scatter
        {
            get;
        }

        /// <summary>
        /// �W�{���U�l���擾����D
        /// </summary>
        double Variance
        {
            get;
        }

        /// <summary>
        /// �e�v�f�̕����𔽓]������D
        /// </summary>
        /// <returns>���g�̎Q��</returns>
        IVector Flip();

        /// <summary>
        /// �e�v�f�̒l��0�ɂ���D
        /// </summary>
        /// <returns>���g�ւ̎Q��</returns>
        IVector Zero();

        //IVector Clone();

        /// <summary>
        /// ���̃I�u�W�F�N�g�̗v�f����Ȃ�z����擾����D
        /// </summary>
        /// <returns>�v�f�l�̔z��</returns>
        double[] ToArray();

        // �ȉ��̃��\�b�h�́CRefVector�Ŏ����ł��Ȃ�
        //Clear
        //Resize

        /// <summary>
        /// <see cref="ElementActionByVal"/>�ɂ��K�肳�ꂽ�f���Q�[�g���e�v�f�ɓK�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByVal"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        IVector ForEach(ElementActionByVal action);

        /// <summary>
        /// <see cref="ElementActionByRef"/>�ɂ��K�肳�ꂽ�f���Q�[�g���e�v�f�ɓK�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRef"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        IVector ForEach(ElementActionByRef action);

        /// <summary>
        /// <see cref="ElementActionByValWithIndex"/>�ɂ��K�肳�ꂽ�f���Q�[�g���e�v�f�ɓK�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByValWithIndex"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        IVector ForEach(ElementActionByValWithIndex action);

        /// <summary>
        /// <see cref="ElementActionByRefWithIndex"/>�ɂ��K�肳�ꂽ�f���Q�[�g���e�v�f�ɓK�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRefWithIndex"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        IVector ForEach(ElementActionByRefWithIndex action);

        /// <summary>
        /// �����x�N�g����Ԃ��D
        /// </summary>
        /// <param name="startIndex">�����x�N�g���̊J�n�ʒu</param>
        /// <returns><paramref name="startIndex"/>����I�[�܂ł̕����x�N�g��</returns>
        /// <remarks>
        /// ���̃x�N�g�����ێ�����v�f�̕����W���ł��镔���x�N�g����Ԃ��D
        /// �����x�N�g�����ێ�����v�f�́C���̃x�N�g���̗v�f�Ɠ������̂ł���D
        /// </remarks>
        IVector Subvector(int startIndex);

        /// <summary>
        /// �����x�N�g����Ԃ��D
        /// </summary>
        /// <param name="startIndex">�����x�N�g���̊J�n�ʒu</param>
        /// <param name="length">�J�n�ʒu����̒���</param>
        /// <returns>
        /// <paramref name="startIndex"/>����C����<paramref name="length"/>�̕����x�N�g����Ԃ��D
        /// <paramref name="length"/>��<see cref="IVector.Size"/>�𒴂���ꍇ�́C
        /// �I�[�܂Łi[<paramref name="startIndex"/>, <see cref="IVector.Size"/>)�j�̕����x�N�g����Ԃ��D
        /// </returns>
        /// <remarks>
        /// ���̃x�N�g�����ێ�����v�f�̕����W���ł��镔���x�N�g����Ԃ��D
        /// �����x�N�g�����ێ�����v�f�́C���̃x�N�g���̗v�f�Ɠ������̂ł���D
        /// </remarks>
        IVector Subvector(int startIndex, int length);


        /// <summary>
        /// �e�v�f��<paramref name="value"/>�����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        IVector Add(double value);

        /// <summary>
        /// ���̃x�N�g���I�u�W�F�N�g��<paramref name="v"/>�����Z����D
        /// </summary>
        /// <param name="v">�x�N�g���I�u�W�F�N�g</param>
        /// <returns>���g�ւ̎Q��</returns>
        IVector Add(IVector v);

        /// <summary>
        /// ���̃x�N�g���I�u�W�F�N�g����<paramref name="v"/>�����Z����D
        /// </summary>
        /// <param name="v">�x�N�g���I�u�W�F�N�g</param>
        /// <returns>���g�ւ̎Q��</returns>
        IVector Sub(IVector v);

        /// <summary>
        /// �e�v�f��<paramref name="value"/>����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        IVector Mul(double value);

        /// <summary>
        /// �e�v�f��<paramref name="value"/>�����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        IVector Div(double value);
    }
}
