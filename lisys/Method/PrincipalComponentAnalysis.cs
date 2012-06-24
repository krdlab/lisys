using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// 主成分分析
    /// </summary>
    public class PrincipalComponentAnalysis
    {
        private Vector eigenvalues = null;
        private Vector[] eigenvectors = null;

        /// <summary>
        /// 行列 X に主成分分析を適用する．
        /// </summary>
        /// <param name="X">
        /// <para>[n, d]の行列（書き換えられることはない）</para>
        /// <para>データ数：n，次元数：dとしたとき，行列 X の形式は n×d でなければならない．</para>
        /// </param>
        /// <param name="varType">分散の種類</param>
        public PrincipalComponentAnalysis(Matrix X, VarianceType varType)
        {
            Matrix _X = new Matrix(X);

            // 各変数の平均値を0にする
            Vector colAvg = _X.ColumnAverages;
            _X.Columns.ForEach(delegate(int i, IVector v)
            {
                v.ForEach(delegate(ref double val)
                {
                    val -= colAvg[i];
                });
            });

            // 分散・共分散行列
            Matrix S = Matrix.Transpose(_X) * _X;
            if (varType == VarianceType.DivN)
            {
                S /= _X.RowSize;
            }
            else
            {
                S /= (_X.RowSize - 1);
            }

            // 固有値分解
            EigenvalueDecomposition evd = new EigenvalueDecomposition(S);
            evd.Sort(EigenvalueDecomposition.SortOrder.Descending);

            this.eigenvalues = evd.RealEigenvalues;
            this.eigenvectors = evd.RealEigenvectors;
        }

        /// <summary>
        /// 固有値を取得する．
        /// 第1，第2，…という順序で並んでいる．
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// 固有ベクトルを取得する．
        /// 固有値の並びに対応している．
        /// </summary>
        public Vector[] Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// 各主成分軸の寄与率を取得する．
        /// 第1，第2，…という順序で並んでいる．
        /// </summary>
        public double[] Ratios
        {
            get
            {
                double sum = this.eigenvalues.Sum;
                double[] ratios = new double[this.eigenvalues.Size];

                for (int i = 0; i < ratios.Length; ++i)
                {
                    ratios[i] = this.eigenvalues[i] / sum;
                }
                return ratios;
            }
        }

        /// <summary>
        /// 固有ベクトルから構成される行列を取得する（主成分得点係数行列とも呼ばれる）．
        /// </summary>
        /// <remarks>
        /// <para>
        /// このプロパティから取得できる行列は，
        /// 主成分得点行列をP，PCAの対象となった行列をX，主成分得点係数行列をCとしたとき，
        /// </para>
        /// <para><c>P = XC</c></para>
        /// <para>
        /// の関係となる行列<c>C</c>である．
        /// 並び順は，固有値の並び順（第1主成分，第2主成分，．．．）に対応している．
        /// </para>
        /// </remarks>
        public Matrix CoefficientMatrix
        {
            get
            {
                int row = this.eigenvectors[0].Size;
                int col = this.eigenvalues.Size;
                Matrix ret = new Matrix(row, col);
                for (int c = 0; c < col; ++c)
                {
                    ret.Columns[c] = this.eigenvectors[c];
                }
                return ret;
            }
        }
    }
}
