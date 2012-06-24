using System;
using KrdLab.Lisys.Method;

namespace KrdLab.Lisys
{
    public partial class Matrix
    {
        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�� 0 �s����쐬����D
        /// </summary>
        /// <param name="rowSize">�s��</param>
        /// <param name="columnSize">��</param>
        /// <returns>0 �s��</returns>
        public static Matrix Zero(int rowSize, int columnSize)
        {
            return new Matrix(rowSize, columnSize).Zero();
        }

        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�̒P�ʍs����쐬����D
        /// </summary>
        /// <param name="size">�s��̃T�C�Y</param>
        /// <returns>�P�ʍs��</returns>
        public static Matrix Identity(int size)
        {
            return new Matrix(size, size).Identity();
        }

        /// <summary>
        /// �w�肳�ꂽ�s��ɂ�����e�v�f�̕����𔽓]���������ʁi<c>-<paramref name="X"/></c>�j��Ԃ��D
        /// </summary>
        /// <param name="X">�e�v�f�̕����𔽓]������s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <returns>�����𔽓]����������</returns>
        public static Matrix Flip(Matrix X)
        {
            return new Matrix(X).Flip();
        }

        /// <summary>
        /// �w�肳�ꂽ�s���]�u����D
        /// </summary>
        /// <param name="X">�]�u����s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <returns>X�̃R�s�[��]�u�����s��</returns>
        public static Matrix Transpose(Matrix X)
        {
            Matrix tX = new Matrix(X.ColumnSize, X.RowSize);

            for (int r = 0; r < X.RowSize; ++r)
            {
                for (int c = 0; c < X.ColumnSize; ++c)
                {
                    tX[c, r] = X[r, c];
                }
            }
            return tX;
        }

        /// <summary>
        /// �w�肳�ꂽ�s��̋t�s����쐬����D
        /// </summary>
        /// <param name="X">�t�s������߂�s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <returns>X�̋t�s��</returns>
        public static Matrix Inverse(Matrix X)
        {
            MatrixChecker.IsSquare(X);
            Solver solver = new Solver(X, (new Matrix(X.RowSize, X.ColumnSize)).Identity());
            return solver.X;
        }

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

    }
}
