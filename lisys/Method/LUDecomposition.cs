using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// LU����
    /// </summary>
    public class LUDecomposition
    {
        private Matrix _p;
        private Matrix _l;
        private Matrix _u;
        private int zeroValueIndex = -1;
        private int permutationCount = 0;

        /// <summary>
        /// �u���s����擾����D
        /// </summary>
        public Matrix P
        {
            get { return this._p; }
        }

        /// <summary>
        /// ���O�p�s����擾����D
        /// </summary>
        public Matrix L
        {
            get { return this._l; }
        }

        /// <summary>
        /// ��O�p�s����擾����D
        /// </summary>
        public Matrix U
        {
            get { return this._u; }
        }

        /// <summary>
        /// ��O�p�s�񂪓��قł������ꍇ�C<c>U[i, i] == 0</c> �ł��� <c>i</c> ���擾����D
        /// ����ȊO�̏ꍇ�́C<c>-1</c> ��Ԃ��D
        /// </summary>
        public int ZeroValueIndexOfU
        {
            get { return this.zeroValueIndex; }
        }

        /// <summary>
        /// LU�����ōs��ꂽ�s�̒u���񐔂��擾����D
        /// </summary>
        public int PermutationCount
        {
            get { return this.permutationCount; }
        }


        /// <summary>
        /// LU����
        /// </summary>
        /// <param name="X">LU�����̑ΏۂƂȂ�<see cref="Matrix"/>�i�����������邱�Ƃ͂Ȃ��j</param>
        /// <remarks>
        /// <para>�����ł́C<paramref name="X"/> �̃R�s�[�ɑ΂��� <see cref="krdlab.law.func.dgetrf"/> ��K�p����D</para>
        /// <para>������́C<c>X = P * L * U</c> �ƂȂ�D</para>
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
            
            // �u���s����\��
            /*
             * �z�� p �ɂ́C�Ⴆ��2�s�ڂ�3�s�ڂ��u�����ꂽ�ꍇ�ɁC
             *     p = {1, 3, 3};
             *            ��1�񂾂����������Ƃ������Ƃ�����
             * �Ɗi�[����Ă���D
             */
            this.permutationCount = 0;

            this._p = Matrix.Identity(lu.RowSize);
            // row==column, row < column �̏ꍇ�� p.Length == lu.RowSize
            // row > column              �̏ꍇ�� p.Length <  lu.RowSize

            // �ǂ������������ւ��Ȃ��Ƃ����Ȃ��݂���
            // p[0] = 2
            // p[1] = 1
            // p[2] = 4 <- �u0�Ɠ���ւ������2�ƌ����v�Ȃ̂��u����ւ���O��2�ƌ����v�Ȃ̂��H => �ǂ�����҂̂悤��
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
                        p[to] = -1;  // �u�����������Ă��邱�Ƃ������t���O
                    }

                    // �u�������������Ƃ������J�E���g�A�b�v
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
