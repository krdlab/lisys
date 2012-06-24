using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// 重回帰分析
    /// </summary>
    public class MultipleLinearRegressionAnalysis
    {
        private IVector coefficients = null;
        private IVector leverages = null;
        private IVector residuals = null;
        private double r2 = 0;
        private double rc2 = 0;
        private IVector ts = null;
        private int dofP = -1;  // 回帰モデルの自由度
        private int dofE = -1;  // 残差の自由度


        /// <summary>
        /// 係数ベクトルを取得する．
        /// </summary>
        /// <remarks>
        /// <para>今，係数ベクトルが<c>c</c>に格納されているとする．</para>
        /// <para>このとき，</para>
        /// <para><c>c[0]</c>には，定数項</para>
        /// <para><c>c[1], c[2], ...</c>には，各変数に対する係数</para>
        /// が，それぞれ格納されている．
        /// </remarks>
        public IVector CoefficientVector
        {
            get { return this.coefficients; }
        }

        /// <summary>
        /// 決定係数を取得する．
        /// </summary>
        /// <remarks>
        /// <para>サンプリングデータ<c>y</c>と予測値<c>y'</c>との相関<c>R</c>を，重相関係数と呼ぶ．</para>
        /// <para>
        /// 決定係数とは，この重相関係数の2乗値のことである．
        /// この値は，サンプリングデータ<c>y</c>が持つ分散のうち，予測値<c>y'</c>の分散が占める割合を表している．
        /// この値が低い場合，誤差の大きな（うまくフィットしていない）モデルであるといえる．
        /// </para>
        /// </remarks>
        public double R2
        {
            get { return this.r2; }
        }

        /// <summary>
        /// 自由度調整済み決定係数を取得する．
        /// </summary>
        /// <remarks>
        /// 通常，モデルのパラメータ数を増やすことで（たとえ意味のないパラメータであったとしても）
        /// 決定係数の値を大きくすることができる．
        /// しかし，これは意味のあるモデルを構築する上で好ましくない．
        /// そこで，決定係数に対して自由度による調整を行ったものが，この自由度調整済み決定係数である．
        /// </remarks>
        public double Rc2
        {
            get { return this.rc2; }
        }

        /// <summary>
        /// テコ比を取得する．
        /// </summary>
        public IVector LeverageVector
        {
            get { return this.leverages; }
        }

        /// <summary>
        /// 残差ベクトルを取得する．
        /// </summary>
        public IVector ResidualVector
        {
            get { return this.residuals; }
        }

        /// <summary>
        /// 同定した各係数の絶対値に対する検定統計量（t値）を取得する．
        /// </summary>
        public IVector TValueVector
        {
            get { return this.ts; }
        }

        /// <summary>
        /// 回帰モデルの自由度を取得する．
        /// </summary>
        /// <remarks><c>phi_p = p</c></remarks>
        public int DofP
        {
            get { return this.dofP; }
        }

        /// <summary>
        /// 残差の自由度を取得する．
        /// </summary>
        /// <remarks><c>phi_e = n - p - 1</c></remarks>
        public int DofE
        {
            get { return this.dofE; }
        }

        /// <summary>
        /// 重回帰分析を適用する．
        /// </summary>
        /// <param name="y">出力値のベクトル</param>
        /// <param name="xs">入力ベクトルのセット</param>
        public MultipleLinearRegressionAnalysis(IVector y, IVector[] xs)
        {
            if (y.Size != xs.Length)
            {
                throw new Exception.MismatchSizeException();
            }

            ColumnVector Y = new ColumnVector(y);
            Matrix X = new Matrix(VectorType.RowVector, xs);
            int p = X.ColumnSize;   // 変数の数

            // 各変数の平均値を算出
            IVector colAvgs = X.ColumnAverages;
            X.Rows.ForEach(delegate(IVector rv)
            {
                rv.ForEach(delegate(int i, ref double val)
                {
                    val -= colAvgs[i];
                });
            });

            // 第1列目に"1"を挿入
            Matrix tmp = new Matrix(X.RowSize, X.ColumnSize + 1);
            for (int r = 0; r < tmp.RowSize; ++r)
            {
                tmp[r, 0] = 1.0;
                for (int c = 1; c < tmp.ColumnSize; ++c)
                {
                    tmp[r, c] = X[r, c - 1];
                }
            }
            X = tmp;

            // 係数の同定
            Matrix tX = Matrix.Transpose(X);
            Matrix itXX = Matrix.Inverse(tX * X);
            IVector C = itXX * tX * Y;

            // 定数項を求める
            for (int i = 1; i < C.Size; ++i)
            {
                C[0] -= C[i] * colAvgs[i - 1];
            }

            this.coefficients = C;

            // テコ比
            Matrix H = X * itXX * tX;
            this.leverages = new Vector(H.ColumnSize);
            H.Rows.ForEach(delegate(int r, IVector v)
            {
                this.leverages[r] = v[r];
            });

            // 残差ベクトル
            ColumnVector e = Y - H * Y;
            this.residuals = new Vector(e);

            int phi_t = Y.Size - 1;        // n - 1
            int phi_e = Y.Size - p - 1;    // n - p - 1
            double Se = e * e;

            this.dofP = p;
            this.dofE = phi_e;

            double y_avg = Y.Average;
            double Syy = 0.0;
            Y.ForEach(delegate(double val)
            {
                Syy += (val - y_avg) * (val - y_avg);
            });

            // 決定係数
            this.r2 = 1.0 - Se / Syy;

            // 自由度補正済み決定係数
            this.rc2 = 1.0 - (Se / phi_e) / (Syy / phi_t);

            // 各係数のt値
            double Ve = Se / phi_e;
            IVector std_coeff = new Vector(this.coefficients.Size);
            std_coeff.ForEach(delegate(int i, ref double val)
            {
                val = Math.Sqrt(Ve * itXX[i, i]);
            }); // Sqrt(Ve/Sxx)
            // まだ，定数項の標準偏差は使えない（Yの平均値のものになっている）

            // 定数項の検定統計量を算出
            double var0 = itXX[0, 0];   // 1/n
            for (int i = 1; i < p + 1; ++i)
            {
                for (int j = 1; j < p + 1; ++j)
                {
                    var0 += colAvgs[i - 1] * colAvgs[j - 1] * itXX[i, j];
                }
            }
            std_coeff[0] = Math.Sqrt(var0 * Ve);

            IVector t = new Vector(std_coeff.Size);
            t.ForEach(delegate(int i, ref double val)
            {
                val = Math.Abs(this.coefficients[i]) / std_coeff[i]; // 帰無仮説はβ=0
            });
            this.ts = t;
        }
    }
}
