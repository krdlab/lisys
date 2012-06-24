using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ���U�̎��
    /// </summary>
    public enum VarianceType
    {
        /// <summary>
        /// ���U�i���ς���̍���2��a��N�Ŋ������l�j
        /// </summary>
        DivN,

        /// <summary>
        /// �W�{���U�i����ʁFN-1�Ŋ������l�j
        /// </summary>
        Sample,
    }

    /// <summary>
    /// �����̑Ώ�
    /// </summary>
    public enum Target
    {
        /// <summary>
        /// �s�P�ʂ�ΏۂƂ���D
        /// </summary>
        EachRow,

        /// <summary>
        /// ��P�ʂ�ΏۂƂ���D
        /// </summary>
        EachColumn,
    }

    /// <summary>
    /// �����̕���
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// �s�����ɏ������s���D
        /// </summary>
        Row,

        /// <summary>
        /// ������ɏ������s���D
        /// </summary>
        Column,
    }

    /// <summary>
    /// �x�N�g���̎��
    /// </summary>
    public enum VectorType
    {
        /// <summary>
        /// �s�x�N�g��
        /// </summary>
        RowVector,

        /// <summary>
        /// ��x�N�g��
        /// </summary>
        ColumnVector,
    }

    /// <summary>
    /// �R���N�V�����̎��
    /// </summary>
    public enum CollectionType
    {
        /// <summary>
        /// �s�R���N�V����
        /// </summary>
        Rows,

        /// <summary>
        /// ��R���N�V����
        /// </summary>
        Columns,
    }
}
