using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// Matrix�I�u�W�F�N�g�̏�Ԃ��`�F�b�N����D
    /// </summary>
    internal static class MatrixChecker
    {
        public static void SizeEquals(Matrix m1, Matrix m2)
        {
            if (m1.RowSize != m2.RowSize || m1.ColumnSize != m2.ColumnSize)
            {
                throw new Exception.MismatchSizeException(
                    "Matrix1.RowSize=" + m1.RowSize + ", Matrix1.ColumnSize=" + m1.ColumnSize +
                    ", Matrix2.RowSize=" + m2.RowSize + ", Matrix2.ColumnSize=" + m2.ColumnSize);
            }
        }

        public static void CanMultiply(Matrix m1, Matrix m2)
        {
            if (m1.ColumnSize != m2.RowSize)
            {
                throw new Exception.CannotMultiplyException(
                    "Matrix1.RowSize=" + m1.RowSize + ", Matrix1.ColumnSize=" + m1.ColumnSize +
                    ", Matrix2.RowSize=" + m2.RowSize + ", Matrix2.ColumnSize=" + m2.ColumnSize);
            }
        }

        public static void CanMultiply(Matrix m, ColumnVector cv)
        {
            if (m.ColumnSize != cv.Size)
            {
                throw new Exception.CannotMultiplyException(
                    "Matrix.RowSize=" + m.RowSize + ", Matrix.ColumnSize=" + m.ColumnSize +
                    ", ColumnVector.Size=" + cv.Size);
            }
        }

        public static void CanMultiply(RowVector rv, Matrix m)
        {
            if (rv.Size != m.RowSize)
            {
                throw new Exception.CannotMultiplyException(
                    "Matrix.RowSize=" + m.RowSize + ", Matrix.ColumnSize=" + m.ColumnSize +
                    ", RowVector.Size=" + rv.Size);
            }
        }

        public static void IsSquare(Matrix m)
        {
            if (m.RowSize != m.ColumnSize)
            {
                throw new Exception.NotSquareMatrixException();
            }
        }

        public static void IsNotZeroSize(Matrix m)
        {
            if (m.RowSize == 0 || m.ColumnSize == 0)
            {
                throw new Exception.ZeroSizeException();
            }
        }

        public static void ValueIsLessThanLimit(double value)
        {
            VectorChecker.ValueIsLessThanLimit(value);
        }
    }
}
