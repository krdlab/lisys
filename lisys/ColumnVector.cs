using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ��x�N�g��
    /// </summary>
    [Serializable]
    public class ColumnVector : Vector
    {
        #region �R���X�g���N�V����

        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�̃x�N�g�����쐬����D
        /// </summary>
        /// <param name="size">�x�N�g���̃T�C�Y�i�v�f���j</param>
        public ColumnVector(int size)
            : base(size)
        {
        }

        /// <summary>
        /// �C���^�t�F�[�X��ʂ��āC�w�肳�ꂽ�I�u�W�F�N�g�̃R�s�[���쐬����D
        /// </summary>
        /// <param name="v">�R�s�[�����I�u�W�F�N�g�̃C���^�t�F�[�X</param>
        public ColumnVector(IVector v)
            : base(v)
        {
        }

        /// <summary>
        /// �C�ӂ̌��̗v�f�𒼐ڎw�肵�ăx�N�g�����쐬����D
        /// </summary>
        /// <param name="arr">�C�ӂ̌��̃x�N�g���̗v�f</param>
        public ColumnVector(params double[] arr)
            : base(arr)
        {
        }

        #endregion

        #region ���Z�̒�`

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static ColumnVector operator +(ColumnVector v)
        {
            return new ColumnVector(v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static ColumnVector operator -(ColumnVector v)
        {
            return (ColumnVector)new ColumnVector(v).Flip();
        }


        #region Add
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator +(ColumnVector v1, ColumnVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator +(ColumnVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator +(Vector v1, ColumnVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator +(ColumnVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator +(IVector v1, ColumnVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new ColumnVector(v1.Size), v1, v2);
        }
        #endregion

        #region Sub
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator -(ColumnVector v1, ColumnVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator -(ColumnVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator -(Vector v1, ColumnVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator -(ColumnVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new ColumnVector(v1.Size), v1, v2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static ColumnVector operator -(IVector v1, ColumnVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new ColumnVector(v1.Size), v1, v2);
        }
        #endregion

        #region Mul
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(ColumnVector v1, ColumnVector v2)
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
        public static double operator *(ColumnVector v1, Vector v2)
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
        public static double operator *(Vector v1, ColumnVector v2)
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
        public static double operator *(ColumnVector v1, IVector v2)
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
        public static double operator *(IVector v1, ColumnVector v2)
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
        public static ColumnVector operator *(double d, ColumnVector v)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new ColumnVector(v.Size), d, v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ColumnVector operator *(ColumnVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new ColumnVector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ColumnVector operator /(ColumnVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new ColumnVector(v.Size), v, d);
        }

        #region RowVector�Ƃ̉��Z
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rv"></param>
        /// <param name="cv"></param>
        /// <returns></returns>
        public static double operator *(RowVector rv, ColumnVector cv)
        {
            VectorChecker.MismatchSize(rv, cv);
            return VectorImpl.Dot(rv, cv);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cv"></param>
        /// <param name="rv"></param>
        /// <returns></returns>
        public static Matrix operator *(ColumnVector cv, RowVector rv)
        {
            VectorChecker.ZeroSize(cv);
            VectorChecker.ZeroSize(rv);

            Matrix ret = new Matrix(cv.Size, rv.Size);
            for (int r = 0; r < cv.Size; ++r)
            {
                for (int c = 0; c < rv.Size; ++c)
                {
                    ret[r, c] = cv[r] * rv[c];
                }
            }
            return ret;
        }
        #endregion

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static new ColumnVector Add(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new ColumnVector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static new ColumnVector Sub(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new ColumnVector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static new ColumnVector Mul(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new ColumnVector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static new ColumnVector Div(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new ColumnVector(v.Size), v, d);
        }
    }
}
