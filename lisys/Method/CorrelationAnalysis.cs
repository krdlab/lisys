using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// 相関分析
    /// </summary>
    public class CorrelationAnalysis
    {
        private Matrix _r = null;
        
        /// <summary>
        /// 相関行列を取得する．
        /// </summary>
        public Matrix R
        {
            get { return this._r; }
        }

        /// <summary>
        /// 相関分析を行う．
        /// </summary>
        /// <param name="X">データベクトルが格納された行列（read-only）</param>
        /// <param name="target">相関を求める対象（行ごとに相関 or 列ごとに相関）</param>
        public CorrelationAnalysis(Matrix X, Target target)
        {
            MatrixChecker.IsNotZeroSize(X);
            _CorrelationAnalysis(X, X, target);
        }

        /// <summary>
        /// 相関分析を行う．
        /// </summary>
        /// <param name="X">データベクトルの格納された行列（read-only）</param>
        /// <param name="Y">データベクトルの格納された行列（read-only）</param>
        /// <param name="target">相関を求める対象（行ごとに相関 or 列ごとに相関）</param>
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
        /// 2つのベクトルの相関を求める．
        /// </summary>
        /// <param name="vx">ベクトル</param>
        /// <param name="vy">ベクトル</param>
        /// <returns>相関</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// ベクトルのサイズが一致しないときにthrowされる．
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
