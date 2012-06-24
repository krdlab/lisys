using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// 重判別分析（正準判別分析: Canonical Discriminant Analysis）
    /// </summary>
    public class MultipleDiscriminantAnalysis
    {
        private Vector eigenvalues = null;
        private Vector[] eigenvectors = null;

        /// <summary>
        /// 判別関数（係数ベクトル）を求める．
        /// </summary>
        /// <param name="Xs">
        /// <para>各郡のMatrixオブジェクト（各行はデータに，各列は各変数に対応する）の配列</para>
        /// <para>
        /// 1つの群データは，1つの<see cref="Matrix"/>オブジェクトであらわされており，
        /// 本処理により書き換えられることはない（read-only）．
        /// </para>
        /// </param>
        public MultipleDiscriminantAnalysis(Matrix[] Xs)
        {
            int C = Xs.Length;          // 群数
            int[] ns = new int[C];      // 各群のデータ数
            int p = Xs[0].ColumnSize;   // 次元数

            // 各郡のデータ数を取得
            for (int k = 0; k < C; ++k)
            {
                ns[k] = Xs[k].RowSize;
            }

            Vector tAvg = new Vector(p);    // 全データにおける各次元の平均値
            tAvg.Zero();

            int tN = 0; // 全データ数

            Vector[] avgs = new Vector[C];  // 各群における各次元の平均値
            for (int k = 0; k < C; ++k)
            {
                tN += Xs[k].RowSize;
                avgs[k] = Xs[k].ColumnAverages;

                tAvg.ForEach(delegate(int i, ref double val)
                {
                    val += Xs[k].Columns[i].Sum;
                });
            }
            tAvg /= tN;

            Matrix B = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                IVector dv = avgs[k] - tAvg;
                B += (ns[k] * new ColumnVector(dv) * new RowVector(dv));
            }

            Matrix W = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                for (int i = 0; i < ns[k]; ++i)
                {
                    IVector dv = Xs[k].Rows[i] - avgs[k];
                    W += (new ColumnVector(dv) * new RowVector(dv));
                }
            }

            EigenvalueDecomposition evd = new EigenvalueDecomposition(Matrix.Inverse(W) * B);
            evd.Sort(EigenvalueDecomposition.SortOrder.Descending);

            this.eigenvalues = evd.RealEigenvalues;
            this.eigenvectors = evd.RealEigenvectors;
        }

        /// <summary>
        /// 固有値を取得する．降順（大→小）にソートされている．
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// 固有ベクトル（係数ベクトル）を取得する．固有値の順序に対応している．
        /// </summary>
        public Vector[] Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// 判別関数の寄与率を取得する．
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
    }
}
