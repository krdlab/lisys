using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �s�x�N�g��
    /// </summary>
    [Serializable]
    public class RowVector : Vector
    {
        internal RowVector()
        {
        }

        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�̃x�N�g�����쐬����D
        /// </summary>
        /// <param name="size"></param>
        public RowVector(Size size)
            : base(size)
        {
        }

        /// <summary>
        /// �R�s�[���쐬����D
        /// </summary>
        /// <param name="v"></param>
        public RowVector(Vector v)
            : this(v._body)
        {
        }

        /// <summary>
        /// �C�ӂ̌��̗v�f�𒼐ڎw�肵�ăx�N�g�����쐬����D
        /// </summary>
        /// <param name="arr">�C�ӂ̌��̃x�N�g���̗v�f</param>
        public RowVector(params double[] arr)
            : base(arr)
        {
        }

        /// <summary>
        /// �R�s�[���쐬����D
        /// </summary>
        /// <param name="v"></param>
        public RowVector(IEnumerable<double> v)
            : base(v)
        {
        }

        #region ���Z�̒�`

        /// <summary>
        /// </summary>
        /// <returns>�����I�u�W�F�N�g�̃R�s�[</returns>
        public static RowVector operator +(RowVector v)
        {
            return new RowVector(v);
        }

        /// <summary>
        /// </summary>
        /// <returns>�����I�u�W�F�N�g�̃R�s�[�𕄍����]�����I�u�W�F�N�g</returns>
        public static RowVector operator -(RowVector v)
        {
            return (RowVector)new RowVector(v).Flip();
        }

        /// <summary>
        /// </summary>
        public static RowVector operator +(RowVector l, RowVector r)
        {
            return new RowVector(plus(l, r));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator -(RowVector l, RowVector r)
        {
            return new RowVector(sub(l, r));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator *(double d, RowVector v)
        {
            return new RowVector(scala(d, v));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator *(RowVector v, double d)
        {
            return new RowVector(scala(d, v));
        }

        /// <summary>
        /// </summary>
        public static RowVector operator /(RowVector v, double d)
        {
            return new RowVector(scala(1 / d, v));
        }

        #endregion
    }
}
