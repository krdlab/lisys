using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// LU分解
    /// </summary>
    public class LUDecomposition
    {
        private Matrix _p;
        private Matrix _l;
        private Matrix _u;
        private int zeroValueIndex = -1;
        private int permutationCount = 0;

        /// <summary>
        /// 置換行列を取得する．
        /// </summary>
        public Matrix P
        {
            get { return this._p; }
        }

        /// <summary>
        /// 下三角行列を取得する．
        /// </summary>
        public Matrix L
        {
            get { return this._l; }
        }

        /// <summary>
        /// 上三角行列を取得する．
        /// </summary>
        public Matrix U
        {
            get { return this._u; }
        }

        /// <summary>
        /// 上三角行列が特異であった場合，<c>U[i, i] == 0</c> である <c>i</c> を取得する．
        /// それ以外の場合は，<c>-1</c> を返す．
        /// </summary>
        public int ZeroValueIndexOfU
        {
            get { return this.zeroValueIndex; }
        }

        /// <summary>
        /// LU分解で行われた行の置換回数を取得する．
        /// </summary>
        public int PermutationCount
        {
            get { return this.permutationCount; }
        }


        /// <summary>
        /// LU分解
        /// </summary>
        /// <param name="X">LU分解の対象となる<see cref="Matrix"/>（書き換えられることはない）</param>
        /// <remarks>
        /// <para>内部では，<paramref name="X"/> のコピーに対して <see cref="krdlab.law.func.dgetrf"/> を適用する．</para>
        /// <para>分解後は，<c>X = P * L * U</c> となる．</para>
        /// </remarks>
        public LUDecomposition(Matrix X)
        {
            MatrixChecker.IsNotZeroSize(X);

            Matrix lu = new Matrix(X);

            int[] p = new int[0];

            int ret = krdlab.law.func.dgetrf(lu._body, lu._rsize, lu._csize, ref p);
            if (0 < ret)
            {
                this.zeroValueIndex = ret - 1;
            }
            
            // 置換行列を構成
            /*
             * 配列 p には，例えば2行目と3行目が置換された場合に，
             *     p = {1, 3, 3};
             *            ↑1回だけ発生したということが解る
             * と格納されている．
             */
            this.permutationCount = 0;

            this._p = Matrix.Identity(lu.RowSize);
            // row==column, row < column の場合は p.Length == lu.RowSize
            // row > column              の場合は p.Length <  lu.RowSize

            // どうも下から入れ替えないといけないみたい
            // p[0] = 2
            // p[1] = 1
            // p[2] = 4 <- 「0と入れ替えた後の2と交換」なのか「入れ替える前の2と交換」なのか？ => どうも後者のようだ
            for (int from = p.Length - 1; 0 <= from; --from)
            {
                int to = p[from];
                if (0 <= to && from != to)
                {
                    this._p.Rows.Swap(from, to);
                    //this.p[from, from] = this.p[to, to] = 0;
                    //this.p[to, from] = 1;
                    //this.p[from, to] = 1;

                    if (to < p.Length && p[to] == from)
                    {
                        p[to] = -1;  // 置換が完了していることを示すフラグ
                    }

                    // 置換が発生したときだけカウントアップ
                    ++this.permutationCount;
                }
            }
            
            int minSize = Math.Min(lu.RowSize, lu.ColumnSize);
            this._l = Matrix.Zero(lu.RowSize, minSize);
            this._u = Matrix.Zero(minSize, lu.ColumnSize);

            // set L
            for (int r = 0; r < lu.RowSize; ++r)
            {
                for (int c = 0; c <= r; ++c)
                {
                    if (this._l.ColumnSize <= c)
                    {
                        break;
                    }
                    this._l[r, c] = (r == c ? 1.0 : lu[r, c]);
                }
            }

            // set U
            for (int r = 0; r < this._u.RowSize; ++r)
            {
                for (int c = r; c < lu.ColumnSize; ++c)
                {
                    this._u[r, c] = lu[r, c];
                }
            }
        }

    }
}
