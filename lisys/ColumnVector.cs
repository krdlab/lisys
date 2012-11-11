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
        internal ColumnVector()
        {
        }

        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�̃x�N�g�����쐬����D
        /// </summary>
        /// <param name="size"></param>
        public ColumnVector(Size size)
            : base(size)
        {
        }

        /// <summary>
        /// �R�s�[���쐬����D
        /// </summary>
        /// <param name="v"></param>
        public ColumnVector(Vector v)
            : this(v._body)
        {
        }

        /// <summary>
        /// �v�f�𒼐ڎw�肵�ăx�N�g�����쐬����D
        /// </summary>
        /// <param name="arr"></param>
        public ColumnVector(params double[] arr)
            : base(arr)
        {
        }

        /// <summary>
        /// �R�s�[���쐬����D
        /// </summary>
        /// <param name="v"></param>
        public ColumnVector(IEnumerable<double> v)
            : base(v)
        {
        }

        #region ���Z�̒�`

        /// <summary>
        /// </summary>
        public static ColumnVector operator +(ColumnVector v)
        {
            return new ColumnVector(v);
        }

        /// <summary>
        /// </summary>
        public static ColumnVector operator -(ColumnVector v)
        {
            return (ColumnVector)new ColumnVector(v).Flip();
        }

        /// <summary>
        /// </summary>
        public static ColumnVector operator +(ColumnVector l, ColumnVector r)
        {
            return new ColumnVector(plus(l, r));
        }

        /// <summary>
        /// </summary>
        public static ColumnVector operator -(ColumnVector l, ColumnVector r)
        {
            return new ColumnVector(sub(l, r));
        }

        /// <summary>
        /// </summary>
        public static ColumnVector operator *(double d, ColumnVector v)
        {
            return new ColumnVector(scala(d, v));
        }

        /// <summary>
        /// </summary>
        public static ColumnVector operator *(ColumnVector v, double d)
        {
            return new ColumnVector(scala(d, v));
        }

        /// <summary>
        /// </summary>
        public static ColumnVector operator /(ColumnVector v, double d)
        {
            return new ColumnVector(scala(1 / d, v));
        }

        #endregion

        #region ����

        /// <summary>
        /// </summary>
        public static Matrix operator *(ColumnVector cv, RowVector rv)
        {
            VectorChecker.IsNotZeroSize(cv);
            VectorChecker.IsNotZeroSize(rv);
            int rowSize = cv.Size;
            int colSize = rv.Size;
            Matrix ret = new Matrix(rowSize, colSize);
            for (int r = 0; r < rowSize; ++r)
            {
                for (int c = 0; c < colSize; ++c)
                {
                    ret[r, c] = cv._body[r] * rv._body[c];
                }
            }
            return ret;
        }

        #endregion
    }
}
