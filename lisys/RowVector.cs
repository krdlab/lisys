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
        #region �R���X�g���N�V����

        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�̃x�N�g�����쐬����D
        /// </summary>
        /// <param name="size">�x�N�g���̃T�C�Y�i�v�f���j</param>
        public RowVector(int size)
            : base(size)
        {
        }

        /// <summary>
        /// �C���^�t�F�[�X��ʂ��āC�w�肳�ꂽ�I�u�W�F�N�g�̃R�s�[���쐬����D
        /// </summary>
        /// <param name="v">�R�s�[�����I�u�W�F�N�g�̃C���^�t�F�[�X</param>
        public RowVector(IVector v)
            : base(v)
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

        #endregion

        #region ���Z�̒�`

        /// <summary>
        /// �P�����Z�q "+" ��K�p����D
        /// </summary>
        /// <param name="v"></param>
        /// <returns>�����I�u�W�F�N�g�̃N���[��</returns>
        public static RowVector operator +(RowVector v)
        {
            return new RowVector(v);
        }

        /// <summary>
        /// �P�����Z�q "-" ��K�p����D
        /// </summary>
        /// <param name="v"></param>
        /// <returns>�����I�u�W�F�N�g�̃N���[���𕄍����]�����I�u�W�F�N�g</returns>
        public static RowVector operator -(RowVector v)
        {
            return (RowVector)new RowVector(v).Flip();
        }


        #region Add
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(RowVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(RowVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(Vector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(RowVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator +(IVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }
        #endregion

        #region Sub
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(RowVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(RowVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(Vector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(RowVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static RowVector operator -(IVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }
        #endregion

        #region Mul
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(RowVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(RowVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(RowVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(IVector v1, RowVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static RowVector operator *(double d, RowVector v)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new RowVector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static RowVector operator *(RowVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new RowVector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static RowVector operator /(RowVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new RowVector(v.Size), v, d);
        }

        #endregion

        /// <summary>
        /// ���Z����D
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>���Z���ʂ�<see cref="RowVector"/></returns>
        public static new RowVector Add(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new RowVector(v1.Size), v1, v2);
        }

        /// <summary>
        /// ���Z����D
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>���Z���ʂ�<see cref="RowVector"/></returns>
        public static new RowVector Sub(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new RowVector(v1.Size), v1, v2);
        }

        /// <summary>
        /// �X�J���l����Z����D
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns>��Z���ʂ�<see cref="RowVector"/></returns>
        public static new RowVector Mul(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new RowVector(v.Size), d, v);
        }

        /// <summary>
        /// �X�J���l�ŏ��Z����D
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns>���Z���ʂ�<see cref="RowVector"/></returns>
        public static new RowVector Div(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new RowVector(v.Size), v, d);
        }
    }
}
