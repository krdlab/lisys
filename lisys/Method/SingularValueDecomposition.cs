using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// ���ْl����
    /// </summary>
    public class SingularValueDecomposition
    {
        private Matrix _u = null;
        private Matrix _s = null;
        private Matrix _v = null;

        /// <summary>
        /// �e���ْl�ɑΉ����鍶���كx�N�g�����e��Ɋi�[���ꂽ�s��
        /// </summary>
        public Matrix U
        {
            get { return this._u; }
        }

        /// <summary>
        /// �Ίp�v�f�����ْl�ł���s��
        /// </summary>
        public Matrix S
        {
            get { return this._s; }
        }

        /// <summary>
        /// �e���ْl�ɑΉ�����E���كx�N�g�����e��Ɋi�[���ꂽ�s��
        /// </summary>
        public Matrix V
        {
            get { return this._v; }
        }

        /// <summary>
        /// ���ْl����
        /// </summary>
        /// <param name="X">���ْl���������s��i�����������邱�Ƃ͂Ȃ��j</param>
        public SingularValueDecomposition(Matrix X)
        {
            if (X.RowSize < 2 || X.ColumnSize < 2)
            {
                throw new Exception.IllegalArgumentException(
                    "Matrix size is less than 2 (RowSize=" + X.RowSize + ", ColumnSize=" + X.ColumnSize + ").");
            }

            Matrix x = new Matrix(X);

            double[] u = null;
            int u_row = 0, u_col = 0;

            double[] s = null;
            int s_row = 0, s_col = 0;
            
            double[] v = null;
            int v_row = 0, v_col = 0;

            krdlab.law.func.dgesvd(x._body, x._rsize, x._csize
                                    , ref u, ref u_row, ref u_col
                                    , ref s, ref s_row, ref s_col
                                    , ref v, ref v_row, ref v_col);

            this._u = new Matrix(u, u_row, u_col);
            this._s = new Matrix(s, s_row, s_col);
            this._v = new Matrix(v, v_row, v_col);
        }
    }
}
