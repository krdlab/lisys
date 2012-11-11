using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KrdLab.Lisys
{
    public partial class Matrix
    {
        /// <summary>
        /// <see cref="Identity(int)"/>
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix I(int size)
        {
            return Identity(size);
        }

        /// <summary>
        /// <see cref="Transpose(Matrix)"/>�Ɠ���
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Matrix T(Matrix X)
        {
            return Transpose(X);
        }

        /// <summary>
        /// <see cref="Inverse(Matrix)"/>�Ɠ���
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Matrix Inv(Matrix X)
        {
            return Inverse(X);
        }

        /// <summary>
        /// <see cref="Trace"/>
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public static double Tr(Matrix X)
        {
            return X.Trace;
        }

        /// <summary>
        /// <see cref="Determinant"/>
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public static double Det(Matrix X)
        {
            return X.Determinant;
        }

        /// <summary>
        /// 0 �s����쐬����D
        /// </summary>
        /// <param name="rowSize">�s��</param>
        /// <param name="columnSize">��</param>
        /// <returns>0 �s��</returns>
        public static Matrix Zero(int rowSize, int columnSize)
        {
            return new Matrix(rowSize, columnSize).Zero();
        }

        /// <summary>
        /// �P�ʍs����쐬����D
        /// </summary>
        /// <param name="size">�s��̃T�C�Y</param>
        /// <returns>�P�ʍs��</returns>
        public static Matrix Identity(int size)
        {
            return new Matrix(size, size).Identity();
        }

        /// <summary>
        /// �e�v�f�̕����𔽓]�������s��i<c>-<paramref name="X"/></c>�j��Ԃ��D
        /// </summary>
        /// <param name="X">�e�v�f�̕����𔽓]������s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <returns></returns>
        public static Matrix Flip(Matrix X)
        {
            return new Matrix(X).Flip();
        }

        /// <summary>
        /// �]�u�s���Ԃ��D
        /// </summary>
        /// <param name="m">�]�u����s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <returns></returns>
        public static Matrix Transpose(Matrix m)
        {
            Matrix tX = new Matrix(m.ColumnSize, m.RowSize);
            for (int r = 0; r < m.RowSize; ++r)
            {
                for (int c = 0; c < m.ColumnSize; ++c)
                {
                    tX[c, r] = m[r, c];
                }
            }
            return tX;
        }

        /// <summary>
        /// �t�s���Ԃ��D
        /// </summary>
        /// <param name="m">�t�s������߂�s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <returns></returns>
        public static Matrix Inverse(Matrix m)
        {
            MatrixChecker.IsSquare(m);
            Solved result = Func.Solve(m, (new Matrix(m.RowSize, m.ColumnSize)).Identity());
            return result.X;
        }

        /// <summary>
        /// �Ίp�v�f���w�肵�čs����쐬����D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix Diagonal(params double[] values)
        {
            if (values.Length < 1)
            {
                throw new ArgumentException();
            }
            int size = values.Length;
            Matrix m = new Matrix(size, size).Zero();
            for (int i = 0; i < size; ++i)
            {
                m[i, i] = values[i];
            }
            return m;
        }

        /// <summary>
        /// �Ίp�v�f���w�肵�čs����쐬����D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix Diagonal(IEnumerable<double> values)
        {
            int size = values.Count();
            if (size < 1)
            {
                throw new ArgumentException();
            }
            Matrix m = new Matrix(size, size).Zero();
            for (int i = 0; i < size; ++i)
            {
                m[i, i] = values.ElementAt(i);
            }
            return m;
        }

        #region �����n

        public static Matrix CreateAsRows(params IVector[] vs)
        {
            VectorChecker.IsNotZeroSize(vs[0]);
            int size = vs[0].Size;
            if (vs.Any(v => v.Size != size))
            {
                throw new ArgumentException("jugged vectors");
            }

            int count = vs.Length;
            Matrix m = new Matrix(count, size);
            for (int r = 0; r < count; ++r)
            {
                m.Rows[r] = vs[r];
            }
            return m;
        }

        public static Matrix CreateAsColumns(params IVector[] vs)
        {
            VectorChecker.IsNotZeroSize(vs[0]);
            int size = vs[0].Size;
            if (vs.Any(v => v.Size != size))
            {
                throw new ArgumentException("jugged vectors");
            }

            int count = vs.Length;
            Matrix m = new Matrix(size, count);
            for (int c = 0; c < count; ++c)
            {
                m.Columns[c] = vs[c];
            }
            return m;
        }

        public static Matrix CreateAsColumns<C>(IVectorCollection<C> collection)
            where C : IVectorCollection<C>
        {
            int rowSize = collection[0].Size;
            int colSize = collection.Count;
            Matrix m = new Matrix(rowSize, colSize);
            for (int c = 0; c < colSize; ++c)
            {
                m.Columns[c] = collection[c];
            }
            return m;
        }

        public static Matrix CreateAsRows<C>(IVectorCollection<C> collection)
            where C : IVectorCollection<C>
        {
            int rowSize = collection.Count;
            int colSize = collection[0].Size;
            Matrix m = new Matrix(rowSize, colSize);
            for (int r = 0; r < rowSize; ++r)
            {
                m.Rows[r] = collection[r];
            }
            return m;
        }

        #endregion

        #region ����n

        /// <summary>
        /// �����s��ł��邩�ǂ����������D
        /// </summary>
        /// <remarks>
        /// �����s��̏ꍇ�� <c>true</c> ���C����ȊO�̏ꍇ�� <c>false</c> ��Ԃ��D
        /// </remarks>
        public static bool IsSquare(Matrix m)
        {
            return m.RowSize == m.ColumnSize;
        }

        /// <summary>
        /// �Ώ̍s��ł��邩�ǂ����������D
        /// </summary>
        /// <remarks>
        /// �Ώ̍s��̏ꍇ�� <c>true</c> ���C����ȊO�̏ꍇ�� <c>false</c> ��Ԃ��D
        /// </remarks>
        public static bool IsSymmetric(Matrix m, double delta)
        {
            return IsSquare(m) && Matrix.Equals(m, Matrix.Transpose(m), delta);
        }

        /// <summary>
        /// �����s��ł��邩�ǂ����������D
        /// </summary>
        /// <remarks>
        /// �����s��ł���ꍇ�� <c>true</c> ���C����ȊO�̏ꍇ�� <c>false</c> ��Ԃ��D
        /// </remarks>
        public static bool IsOrthogonal(Matrix m, double delta)
        {
            if (IsSquare(m))
            {
                Matrix I = new Matrix(m.RowSize, m.ColumnSize).Identity();
                Matrix X = m;
                Matrix tX = Matrix.Transpose(X);
                return Matrix.Equals(X * tX, I, delta) && Matrix.Equals(tX * X, I, delta);
            }
            return false;
        }

        #endregion

        /// <summary>
        /// �s���CSV�`���̕�����Ƃ��ďo�͂���D
        /// </summary>
        /// <returns>CSV�`���̕�����</returns>
        public static string ToCsv(Matrix m)
        {
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < m.RowSize; ++r)
            {
                StringBuilder row = new StringBuilder();
                for (int c = 0; c < m.ColumnSize; ++c)
                {
                    row.Append(m[r, c] + (c < m.ColumnSize - 1 ? "," : ""));
                }
                sb.AppendLine(row.ToString());
            }
            return sb.ToString();
        }
    }
}
