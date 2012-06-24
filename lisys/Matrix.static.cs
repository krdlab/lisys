using System;
using KrdLab.Lisys.Method;

namespace KrdLab.Lisys
{
    public partial class Matrix
    {
        /// <summary>
        /// 指定されたサイズの 0 行列を作成する．
        /// </summary>
        /// <param name="rowSize">行数</param>
        /// <param name="columnSize">列数</param>
        /// <returns>0 行列</returns>
        public static Matrix Zero(int rowSize, int columnSize)
        {
            return new Matrix(rowSize, columnSize).Zero();
        }

        /// <summary>
        /// 指定されたサイズの単位行列を作成する．
        /// </summary>
        /// <param name="size">行列のサイズ</param>
        /// <returns>単位行列</returns>
        public static Matrix Identity(int size)
        {
            return new Matrix(size, size).Identity();
        }

        /// <summary>
        /// 指定された行列における各要素の符号を反転させた結果（<c>-<paramref name="X"/></c>）を返す．
        /// </summary>
        /// <param name="X">各要素の符号を反転させる行列（書き換えられることはない）</param>
        /// <returns>符号を反転させた結果</returns>
        public static Matrix Flip(Matrix X)
        {
            return new Matrix(X).Flip();
        }

        /// <summary>
        /// 指定された行列を転置する．
        /// </summary>
        /// <param name="X">転置する行列（書き換えられることはない）</param>
        /// <returns>Xのコピーを転置した行列</returns>
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
        /// 指定された行列の逆行列を作成する．
        /// </summary>
        /// <param name="X">逆行列を求める行列（書き換えられることはない）</param>
        /// <returns>Xの逆行列</returns>
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

    }
}
