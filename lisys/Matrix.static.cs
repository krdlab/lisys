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
        /// <see cref="Transpose(Matrix)"/>と等価
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Matrix T(Matrix X)
        {
            return Transpose(X);
        }

        /// <summary>
        /// <see cref="Inverse(Matrix)"/>と等価
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
        /// 0 行列を作成する．
        /// </summary>
        /// <param name="rowSize">行数</param>
        /// <param name="columnSize">列数</param>
        /// <returns>0 行列</returns>
        public static Matrix Zero(int rowSize, int columnSize)
        {
            return new Matrix(rowSize, columnSize).Zero();
        }

        /// <summary>
        /// 単位行列を作成する．
        /// </summary>
        /// <param name="size">行列のサイズ</param>
        /// <returns>単位行列</returns>
        public static Matrix Identity(int size)
        {
            return new Matrix(size, size).Identity();
        }

        /// <summary>
        /// 各要素の符号を反転させた行列（<c>-<paramref name="X"/></c>）を返す．
        /// </summary>
        /// <param name="X">各要素の符号を反転させる行列（書き換えられることはない）</param>
        /// <returns></returns>
        public static Matrix Flip(Matrix X)
        {
            return new Matrix(X).Flip();
        }

        /// <summary>
        /// 転置行列を返す．
        /// </summary>
        /// <param name="m">転置する行列（書き換えられることはない）</param>
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
        /// 逆行列を返す．
        /// </summary>
        /// <param name="m">逆行列を求める行列（書き換えられることはない）</param>
        /// <returns></returns>
        public static Matrix Inverse(Matrix m)
        {
            MatrixChecker.IsSquare(m);
            Solved result = Func.Solve(m, (new Matrix(m.RowSize, m.ColumnSize)).Identity());
            return result.X;
        }

        /// <summary>
        /// 対角要素を指定して行列を作成する．
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
        /// 対角要素を指定して行列を作成する．
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

        #region 生成系

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

        #region 判定系

        /// <summary>
        /// 正方行列であるかどうかを示す．
        /// </summary>
        /// <remarks>
        /// 正方行列の場合は <c>true</c> を，それ以外の場合は <c>false</c> を返す．
        /// </remarks>
        public static bool IsSquare(Matrix m)
        {
            return m.RowSize == m.ColumnSize;
        }

        /// <summary>
        /// 対称行列であるかどうかを示す．
        /// </summary>
        /// <remarks>
        /// 対称行列の場合は <c>true</c> を，それ以外の場合は <c>false</c> を返す．
        /// </remarks>
        public static bool IsSymmetric(Matrix m, double delta)
        {
            return IsSquare(m) && Matrix.Equals(m, Matrix.Transpose(m), delta);
        }

        /// <summary>
        /// 直交行列であるかどうかを示す．
        /// </summary>
        /// <remarks>
        /// 直交行列である場合は <c>true</c> を，それ以外の場合は <c>false</c> を返す．
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
        /// 行列をCSV形式の文字列として出力する．
        /// </summary>
        /// <returns>CSV形式の文字列</returns>
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
