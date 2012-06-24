using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// ���֕���
    /// </summary>
    public class CorrelationAnalysis
    {
        private Matrix _r = null;
        
        /// <summary>
        /// ���֍s����擾����D
        /// </summary>
        public Matrix R
        {
            get { return this._r; }
        }

        /// <summary>
        /// ���֕��͂��s���D
        /// </summary>
        /// <param name="X">�f�[�^�x�N�g�����i�[���ꂽ�s��iread-only�j</param>
        /// <param name="target">���ւ����߂�Ώہi�s���Ƃɑ��� or �񂲂Ƃɑ��ցj</param>
        public CorrelationAnalysis(Matrix X, Target target)
        {
            MatrixChecker.IsNotZeroSize(X);
            _CorrelationAnalysis(X, X, target);
        }

        /// <summary>
        /// ���֕��͂��s���D
        /// </summary>
        /// <param name="X">�f�[�^�x�N�g���̊i�[���ꂽ�s��iread-only�j</param>
        /// <param name="Y">�f�[�^�x�N�g���̊i�[���ꂽ�s��iread-only�j</param>
        /// <param name="target">���ւ����߂�Ώہi�s���Ƃɑ��� or �񂲂Ƃɑ��ցj</param>
        public CorrelationAnalysis(Matrix X, Matrix Y, Target target)
        {
            MatrixChecker.SizeEquals(X, Y);
            _CorrelationAnalysis(X, Y, target);
        }

        private void _CorrelationAnalysis(Matrix X, Matrix Y, Target target)
        {
            Matrix ret = null;
            if (target == Target.EachRow)
            {
                ret = new Matrix(X.RowSize, X.RowSize);
                for (int ry = 0; ry < ret.RowSize; ++ry)
                {
                    for (int rx = 0; rx < ret.ColumnSize; ++rx)
                    {
                        ret[ry, rx] = Correl(Y.Rows[ry], X.Rows[rx]);
                    }
                }
            }
            else
            {
                ret = new Matrix(X.ColumnSize, X.ColumnSize);
                for (int cy = 0; cy < ret.RowSize; ++cy)
                {
                    for (int cx = 0; cx < ret.ColumnSize; ++cx)
                    {
                        ret[cy, cx] = Correl(Y.Columns[cy], X.Columns[cx]);
                    }
                }
            }

            this._r = ret;
        }

        /// <summary>
        /// 2�̃x�N�g���̑��ւ����߂�D
        /// </summary>
        /// <param name="vx">�x�N�g��</param>
        /// <param name="vy">�x�N�g��</param>
        /// <returns>����</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// �x�N�g���̃T�C�Y����v���Ȃ��Ƃ���throw�����D
        /// </exception>
        public static double Correl(IVector vx, IVector vy)
        {
            VectorChecker.MismatchSize(vx, vy);

            double sxy = 0.0;
            double avg_x = vx.Average;
            double avg_y = vy.Average;
            for (int i = 0; i < vx.Size; ++i)
            {
                sxy += ((vx[i] - avg_x) * (vy[i] - avg_y));
            }
            return sxy / Math.Sqrt(vx.Scatter * vy.Scatter);
        }
    }
}
