using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace KrdLab.Lisys
{
    /// <summary>
    /// AX = B を解いた結果
    /// </summary>
    public class Solved
    {
        private readonly Matrix x;
        internal Solved(Matrix x) { this.x = x; }

        /// <summary>
        /// AX = B の X
        /// </summary>
        public Matrix X { get { return this.x; } }
    }

    /// <summary>
    /// 特異値分解の結果
    /// </summary>
    public class Svd
    {
        private readonly Matrix _u;
        private readonly Matrix _s;
        private readonly Matrix _v;

        internal Svd(Matrix u, Matrix s, Matrix v)
        {
            this._u = u;
            this._s = s;
            this._v = v;
        }

        /// <summary>
        /// 各特異値に対応する左特異ベクトルが各列に格納された行列
        /// </summary>
        public Matrix U
        {
            get { return this._u; }
        }

        /// <summary>
        /// 対角要素が特異値である行列
        /// </summary>
        public Matrix S
        {
            get { return this._s; }
        }

        /// <summary>
        /// 各特異値に対応する右特異ベクトルが各列に格納された行列
        /// </summary>
        public Matrix V
        {
            get { return this._v; }
        }
    }

    /// <summary>
    /// LU 分解の結果
    /// </summary>
    public class Lud
    {
        private readonly Matrix _p;
        private readonly Matrix _l;
        private readonly Matrix _u;
        private readonly int permutationCount;
        private readonly int zeroValueIndex;

        internal Lud(Matrix p, Matrix l, Matrix u, int permCount, int zeroIndex = -1)
        {
            this._p = p;
            this._l = l;
            this._u = u;
            this.permutationCount = permCount;
            this.zeroValueIndex = zeroIndex;
        }

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
        /// LU 分解で行われた行の置換回数を取得する．
        /// </summary>
        public int PermutationCount
        {
            get { return this.permutationCount; }
        }
    }

    /// <summary>
    /// 固有値分解の結果
    /// </summary>
    public class Eigen
    {
        private readonly Vector _reValues;
        private readonly Vector _imValues;
        private readonly List<Vector> _reVectors;
        private readonly List<Vector> _imVectors;

        internal Eigen(Vector reValues, Vector imValues, List<Vector> reVectors, List<Vector> imVectors)
        {
            this._reValues = reValues;
            this._imValues = imValues;
            this._reVectors = reVectors;
            this._imVectors = imVectors;
        }

        #region プロパティ

        /// <summary>
        /// 固有値の実数成分を取得する．
        /// </summary>
        public Vector RealEigenvalues
        {
            get { return this._reValues; }
        }

        /// <summary>
        /// 固有値の虚数成分を取得する．
        /// </summary>
        public Vector ImaginaryEigenvalues
        {
            get { return this._imValues; }
        }

        /// <summary>
        /// 固有ベクトルの実部を取得する．
        /// </summary>
        public List<Vector> RealEigenvectors
        {
            get { return this._reVectors; }
        }

        /// <summary>
        /// 固有ベクトルの虚部を取得する．
        /// </summary>
        public List<Vector> ImaginaryEigenvectors
        {
            get { return this._imVectors; }
        }

        #endregion

        /// <summary>
        /// ソートオーダ
        /// </summary>
        public enum SortOrder
        {
            /// <summary>
            /// 昇順（小→大）
            /// </summary>
            Ascending,

            /// <summary>
            /// 降順（大→小）
            /// </summary>
            Descending,
        }

        private class SortItem
        {
            public readonly Complex EigenValue;
            public readonly Vector ReVector;
            public readonly Vector ImVector;

            public SortItem(double reVal, double imVal, Vector reVec, Vector imVec)
            {
                this.EigenValue = new Complex(reVal, imVal);
                this.ReVector = reVec;
                this.ImVector = imVec;
            }
        }

        /// <summary>
        /// 固有値を基準に，固有値と固有ベクトルのペアをソートする (破壊的)．
        /// </summary>
        /// <param name="order">SortOrder列挙子で昇順・降順を指定する．</param>
        public void Sort(SortOrder order)
        {
            // ソート用の構造に詰める
            List<SortItem> items = new List<SortItem>();
            int num = this._reValues.Size;
            for (int i = 0; i < num; ++i)
            {
                items.Add(new SortItem(this._reValues[i], this._imValues[i], this._reVectors[i], this._imVectors[i]));
            }

            var direction = (order == SortOrder.Ascending) ? 1 : -1;
            Func<double, int> filter = v => direction * Math.Sign(v);
            // items に null は含まれない
            items.Sort((l, r) => filter(l.EigenValue.Magnitude - r.EigenValue.Magnitude));

            // itemsを詰め直す
            for (int i = 0; i < items.Count; ++i)
            {
                var item = items[i];
                this._reValues[i] = item.EigenValue.Real;
                this._imValues[i] = item.EigenValue.Imaginary;
                this._reVectors[i] = item.ReVector;
                this._imVectors[i] = item.ImVector;
            }
        }
    }

    /// <summary>
    /// 線形判別分析の結果
    /// </summary>
    public class Lda
    {
        private readonly Vector eigenvalues;
        private readonly List<Vector> eigenvectors;
        private readonly List<double> ratios;
        private readonly List<Vector> groupMeans;

        internal Lda(Vector eigenvalues, List<Vector> eigenvectors, List<Vector> groupMeans)
        {
            this.eigenvalues = eigenvalues;
            this.eigenvectors = eigenvectors;
            this.ratios = CalculateRatios(eigenvalues);
            this.groupMeans = groupMeans;
        }

        private List<double> CalculateRatios(Vector eigenvalues)
        {
            var sum = eigenvalues.Sum;
            return eigenvalues.Select(v => v / sum).ToList();
        }

        /// <summary>
        /// 固有値を取得する．降順にソートされている．
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// 固有ベクトル（係数ベクトル）を取得する．固有値の順序に対応している．
        /// </summary>
        public List<Vector> Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// 判別関数の寄与率を取得する．
        /// </summary>
        public List<double> Ratios
        {
            get { return this.ratios; }
        }

        /// <summary>
        /// 群平均を返す．
        /// </summary>
        public List<Vector> GroupMeans
        {
            get { return this.groupMeans; }
        }
    }

    /// <summary>
    /// 線形重回帰分析の結果
    /// </summary>
    public class Lr
    {
        private readonly Vector coefficients;
        private readonly Vector leverages;
        private readonly Vector residuals;
        private readonly double r2;
        private readonly double rc2;
        private readonly Vector ts;
        private readonly int dofP;  // 回帰モデルの自由度
        private readonly int dofE;  // 残差の自由度

        internal Lr(Vector coefficients, Vector leverages, Vector residuals, double r2, double rc2, Vector ts, int dofP, int dofE)
        {
            this.coefficients = coefficients;
            this.leverages = leverages;
            this.residuals = residuals;
            this.r2 = r2;
            this.rc2 = rc2;
            this.ts = ts;
            this.dofP = dofP;
            this.dofE = dofE;
        }

        /// <summary>
        /// 係数ベクトル ([0] には定数項，[1], [2], ... は各変数に対する係数)．
        /// </summary>
        public Vector CoefficientVector
        {
            get { return this.coefficients; }
        }

        /// <summary>
        /// 決定係数
        /// </summary>
        /// <remarks>
        /// <para>サンプリングデータ <c>y</c> と予測値 <c>y'</c> との相関 <c>R</c> を重相関係数と呼ぶ．</para>
        /// <para>
        /// 決定係数とは，この重相関係数の 2 乗値のことである．
        /// この値は，サンプリングデータ <c>y</c> が持つ分散のうち，予測値 <c>y'</c> の分散が占める割合を表している．
        /// この値が低い場合，誤差の大きな (うまくフィットしていない) モデルであるといえる．
        /// </para>
        /// </remarks>
        public double R2
        {
            get { return this.r2; }
        }

        /// <summary>
        /// 自由度調整済み決定係数
        /// </summary>
        /// <remarks>
        /// 通常，モデルのパラメータ数を増やすことで (たとえ意味のないパラメータであったとしても) 決定係数の値を大きくすることができる．
        /// しかし，これは意味のあるモデルを構築する上で好ましくない．
        /// そこで，決定係数に対して自由度による調整を行ったものが，この自由度調整済み決定係数である．
        /// </remarks>
        public double Rc2
        {
            get { return this.rc2; }
        }

        /// <summary>
        /// レバレッジ (テコ比)
        /// <remarks>hat-matrix ((予測値) = H (観測値)，における行列 H) の対角要素</remarks>
        /// </summary>
        public Vector Leverages
        {
            get { return this.leverages; }
        }

        /// <summary>
        /// 残差ベクトル
        /// </summary>
        public Vector Residuals
        {
            get { return this.residuals; }
        }

        /// <summary>
        /// 同定した各係数の絶対値に対する検定統計量 (t 値)
        /// </summary>
        public Vector TValues
        {
            get { return this.ts; }
        }

        /// <summary>
        /// 回帰モデルの自由度
        /// </summary>
        /// <remarks><c>phi_p = p</c></remarks>
        public int DofP
        {
            get { return this.dofP; }
        }

        /// <summary>
        /// 残差の自由度
        /// </summary>
        /// <remarks><c>phi_e = n - p - 1</c></remarks>
        public int DofE
        {
            get { return this.dofE; }
        }
    }

    /// <summary>
    /// 主成分分析の結果
    /// </summary>
    public class Pca
    {
        private readonly Vector eigenvalues;
        private readonly List<Vector> eigenvectors;
        private readonly double[] ratios;

        internal Pca(Vector eigenvalues, List<Vector> eigenvectors)
        {
            this.eigenvalues = eigenvalues;
            this.eigenvectors = eigenvectors;
            
            double sum = this.eigenvalues.Sum;
            this.ratios = eigenvalues.Select(ev => ev / sum).ToArray();
        }

        /// <summary>
        /// 固有値 (第 1，第 2，... という順序)．
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// 固有ベクトル (固有値の並びに対応)．
        /// </summary>
        public List<Vector> Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// 各主成分軸の寄与率 (第 1，第 2，... という順序)．
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
        /// 固有ベクトルから構成される主成分係数行列．
        /// </summary>
        /// <remarks>
        /// <para>
        /// このプロパティは <c>P = XC</c> で表される行列 <c>C</c> を返す (P: 主成分得点行列，X: データ行列 (ただし E[X] = 0))．
        /// </para>
        /// <para>
        /// 固有値の並び順（第 1 主成分，第 2 主成分，．．．）に対応している．
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

    /// <summary>
    /// 統計分析の各種手法 (便利関数込み)
    /// </summary>
    public static class Func
    {
        /// <summary>
        /// AX = B を解く．
        /// </summary>
        /// <param name="a">[n, n] 行列</param>
        /// <param name="b">[n, *] 行列</param>
        /// <returns></returns>
        public static Solved Solve(Matrix a, Matrix b)
        {
            MatrixChecker.IsSquare(a);
            Matrix _a = new Matrix(a);
            Matrix _b = new Matrix(b);
            Matrix x = new Matrix();
            krdlab.law.func.dgesv(ref x._body, ref x._rsize, ref x._csize,
                                    _a._body, _a._rsize, _a._csize, _b._body, _b._rsize, _b._csize);
            return new Solved(x);
        }

        /// <summary>
        /// 特異値分解
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Svd Svd(Matrix x)
        {
            if (x.RowSize < 2 || x.ColumnSize < 2)
            {
                throw new ArgumentException(
                    "Matrix size is less than 2 (RowSize=" + x.RowSize + ", ColumnSize=" + x.ColumnSize + ").");
            }

            Matrix _x = new Matrix(x);
            double[] u = null;
            int u_row = 0, u_col = 0;
            double[] s = null;
            int s_row = 0, s_col = 0;
            double[] v = null;
            int v_row = 0, v_col = 0;
            krdlab.law.func.dgesvd(_x._body, _x._rsize, _x._csize
                                    , ref u, ref u_row, ref u_col
                                    , ref s, ref s_row, ref s_col
                                    , ref v, ref v_row, ref v_col);
            return new Svd(
                new Matrix(u, u_row, u_col),
                new Matrix(s, s_row, s_col),
                new Matrix(v, v_row, v_col));
        }

        /// <summary>
        /// LU 分解
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Lud Lud(Matrix x)
        {
            MatrixChecker.IsNotZeroSize(x);

            Matrix lu = new Matrix(x);

            int[] p = new int[0];
            int zeroValueIndex = -1;

            int ret = krdlab.law.func.dgetrf(lu._body, lu._rsize, lu._csize, ref p);
            if (0 < ret)
            {
                zeroValueIndex = ret - 1;
            }

            // 置換行列を構成
            /*
             * 配列 p には，例えば2行目と3行目が置換された場合に，
             *     p = {1, 3, 3};
             *            ↑1回だけ発生したということが解る
             * と格納されている．
             */
            int permutationCount = 0;

            Matrix _p = Matrix.Identity(lu.RowSize);
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
                    _p.Rows.Swap(from, to);
                    //this.p[from, from] = this.p[to, to] = 0;
                    //this.p[to, from] = 1;
                    //this.p[from, to] = 1;

                    if (to < p.Length && p[to] == from)
                    {
                        p[to] = -1;  // 置換が完了していることを示すフラグ
                    }

                    // 置換が発生したときだけカウントアップ
                    ++permutationCount;
                }
            }

            int minSize = Math.Min(lu.RowSize, lu.ColumnSize);
            Matrix _l = Matrix.Zero(lu.RowSize, minSize);
            Matrix _u = Matrix.Zero(minSize, lu.ColumnSize);

            // set L
            for (int r = 0; r < lu.RowSize; ++r)
            {
                for (int c = 0; c <= r; ++c)
                {
                    if (_l.ColumnSize <= c)
                    {
                        break;
                    }
                    _l[r, c] = (r == c ? 1.0 : lu[r, c]);
                }
            }

            // set U
            for (int r = 0; r < _u.RowSize; ++r)
            {
                for (int c = r; c < lu.ColumnSize; ++c)
                {
                    _u[r, c] = lu[r, c];
                }
            }
            return new Lud(_p, _l, _u, permutationCount, zeroValueIndex);
        }

        /// <summary>
        /// 固有値分解
        /// </summary>
        /// <param name="x">正方行列</param>
        /// <returns></returns>
        public static Eigen Eigen(Matrix x)
        {
            MatrixChecker.IsSquare(x);

            if (x.RowSize < 2)
            {
                throw new ArgumentException("Matrix size is less than 2.");
            }

            Matrix tmp = new Matrix(x);

            Vector _reValues = new Vector();
            Vector _imValues = new Vector();

            double[][] r_vecs = null;
            double[][] i_vecs = null;

            krdlab.law.func.dgeev(tmp._body, tmp._rsize, tmp._csize,
                                    ref _reValues._body, ref _imValues._body, ref r_vecs, ref i_vecs);

            List<Vector> _reVectors = new List<Vector>();
            List<Vector> _imVectors = new List<Vector>();

            foreach (double[] vec in r_vecs)
            {
                _reVectors.Add(new Vector(vec));
            }

            foreach (double[] vec in i_vecs)
            {
                _imVectors.Add(new Vector(vec));
            }
            return new Eigen(_reValues, _imValues, _reVectors, _imVectors);
        }

        /// <summary>
        /// 線形判別分析
        /// </summary>
        /// <param name="Xs">
        /// <para>各郡の Matrix オブジェクト (行がデータ，列が変数に対応する) の配列</para>
        /// <para>
        /// 1 つの群データは 1 つの Matrix で表される．
        /// 本処理により書き換えられることはない (read-only)．
        /// </para>
        /// </param>
        public static Lda Lda(Matrix[] Xs)
        {
            int C = Xs.Length;          // 群数
            int[] ns = new int[C];      // 各群のデータ数
            int p = Xs[0].ColumnSize;   // 次元数

            // 各郡のデータ数を取得
            for (int k = 0; k < C; ++k)
            {
                ns[k] = Xs[k].RowSize;
            }

            Vector tAvg = new Vector(new Size(p));    // 全データにおける各次元の平均値
            tAvg.Zero();

            int tN = 0; // 全データ数

            Vector[] avgs = new Vector[C];  // 各群における各次元の平均値
            for (int k = 0; k < C; ++k)
            {
                tN += Xs[k].RowSize;
                avgs[k] = Xs[k].ColumnAverages;
                tAvg.Apply((int i, double val) => val + Xs[k].Columns[i].Sum);
            }
            tAvg /= tN;

            Matrix B = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                IVector dv = avgs[k] - tAvg;
                B += (ns[k] * new ColumnVector(dv) * new RowVector(dv));
            }
            B /= (C - 1);

            Matrix W = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                for (int i = 0; i < ns[k]; ++i)
                {
                    IVector dv = Xs[k].Rows[i] - avgs[k];
                    W += (new ColumnVector(dv) * new RowVector(dv));
                }
            }
            W /= (tN - C);

            Eigen evd = Eigen(Matrix.Inverse(W) * B);
            evd.Sort(KrdLab.Lisys.Eigen.SortOrder.Descending);
            return new Lda(evd.RealEigenvalues, evd.RealEigenvectors, avgs.ToList());
        }

        /// <summary>
        /// 線形重回帰分析 (y = Xβ + ε)
        /// </summary>
        /// <param name="y">観測値</param>
        /// <param name="x">入力値からなる行列</param>
        public static Lr Lr(IVector y, Matrix x)
        {
            if (y.Size != x.RowSize)
            {
                throw new ArgumentException("sizes of input/output are not equal");
            }

            ColumnVector _y = new ColumnVector(y);
            int p = x.ColumnSize;   // 変数の数

            // 各変数の平均値を算出
            IVector colAvgs = x.ColumnAverages;
            x.Rows.ForEach((ri, rv) => rv.Apply((int i, double val) => val - colAvgs[i]));

            // 第1列目に"1"を挿入
            Matrix tmp = new Matrix(x.RowSize, x.ColumnSize + 1);
            for (int r = 0; r < tmp.RowSize; ++r)
            {
                tmp[r, 0] = 1.0;
                for (int c = 1; c < tmp.ColumnSize; ++c)
                {
                    tmp[r, c] = x[r, c - 1];
                }
            }
            x = tmp;

            // 係数の同定
            Matrix tX = Matrix.Transpose(x);
            Matrix itXX = Matrix.Inverse(tX * x);
            var C = itXX * tX * _y;
            // 定数項を求める
            for (int i = 1; i < C.Size; ++i)
            {
                C[0] -= C[i] * colAvgs[i - 1];
            }
            var coefficients = C;

            // レバレッジ
            Matrix H = x * itXX * tX;
            var leverages = new Vector(H.ColumnSize);
            H.Rows.ForEach(delegate(int r, IVector v)
            {
                leverages[r] = v[r];
            });

            // 残差ベクトル
            ColumnVector e = _y - H * _y;
            var residuals = new Vector(e);

            int phi_t = _y.Size - 1;        // n - 1
            int phi_e = _y.Size - p - 1;    // n - p - 1
            double Se = e * e;

            var dofP = p;
            var dofE = phi_e;

            double y_avg = _y.Average;
            double Syy = 0.0;
            _y.ForEach((int i, double val) =>
                Syy += (val - y_avg) * (val - y_avg));

            // 決定係数
            var r2 = 1.0 - Se / Syy;

            // 自由度補正済み決定係数
            var rc2 = 1.0 - (Se / phi_e) / (Syy / phi_t);

            // 各係数のt値
            double Ve = Se / phi_e;
            IVector std_coeff = new Vector(coefficients.Size);
            std_coeff.Apply((int i, double val) =>
                Math.Sqrt(Ve * itXX[i, i])); // Sqrt(Ve/Sxx)
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

            var t = new Vector(std_coeff.Size);
            t.Apply((int i, double val) =>
                val = Math.Abs(coefficients[i]) / std_coeff[i] // 帰無仮説はβ=0
            );
            var ts = t;
            return new Lr(coefficients, leverages, residuals, r2, rc2, ts, dofP, dofE);
        }

        /// <summary>
        /// 主成分分析
        /// </summary>
        /// <param name="data">
        /// [n, d] の行列（データ数：n，次元数：d，書き換えられることはない）
        /// </param>
        /// <param name="cor">
        /// 相関行列で分析するかどうか (デフォルトは <c>false</c> = 分散・共分散行列)
        /// </param>
        public static Pca Pca(Matrix data, bool cor = false)
        {
            Matrix x = new Matrix(data);
            Matrix s;
            if (cor)
            {
                s = Correlate(x, Target.EachColumn);
            }
            else
            {
                // 各変数の平均値を 0 にする
                Vector colAvgs = x.ColumnAverages;
                x.Columns.ForEach((c, vec) => vec.Apply((i, val) => val - colAvgs[c]));
                // 分散・共分散行列
                s = Matrix.Transpose(x) * x / (x.RowSize - 1);
            }
            // 固有値分解
            Eigen evd = Eigen(s);
            evd.Sort(KrdLab.Lisys.Eigen.SortOrder.Descending);
            // 対称行列の固有値は全て実数
            return new Pca(evd.RealEigenvalues, evd.RealEigenvectors);
        }

        /// <summary>
        /// 正規化
        /// </summary>
        /// <param name="values">元になるベクトル (変更されない)</param>
        /// <param name="average"></param>
        /// <param name="stdev"></param>
        /// <returns>正規化後の新しいインスタンスを返す</returns>
        public static Vector Standardize(IVector values, double average, double stdev)
        {
            return Standardize(values, average, stdev, new Vector(values.Size));
        }

        /// <summary>
        /// 正規化
        /// </summary>
        public static D Standardize<S, D>(S src, double average, double stdev, D dst)
            where S : IRandomAccessible<double>
            where D : IRandomAccessible<double>
        {
            int size = src.Size;
            for (int i = 0; i < size; ++i)
            {
                dst[i] = (src[i] - average) / stdev;
            }
            return dst;
        }

        /// <summary>
        /// 処理の対象
        /// </summary>
        public enum Target
        {
            /// <summary>
            /// 行単位を対象とする．
            /// </summary>
            EachRow,

            /// <summary>
            /// 列単位を対象とする．
            /// </summary>
            EachColumn,
        }

        /// <summary>
        /// 2つのベクトルの相関を求める．
        /// </summary>
        /// <param name="vx">ベクトル</param>
        /// <param name="vy">ベクトル</param>
        /// <returns>相関</returns>
        /// <exception cref="System.ArgumentException">
        /// ベクトルのサイズが一致しないときにthrowされる．
        /// </exception>
        public static double Correlate(IVector vx, IVector vy)
        {
            VectorChecker.SizeEquals(vx, vy);

            double sxy = 0.0;
            double avg_x = vx.Average;
            double avg_y = vy.Average;
            for (int i = 0; i < vx.Size; ++i)
            {
                sxy += ((vx[i] - avg_x) * (vy[i] - avg_y));
            }
            return sxy / Math.Sqrt(vx.Scatter * vy.Scatter);
        }

        /// <summary>
        /// 各ベクトルの相関を求める．
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Matrix Correlate(Matrix X, Matrix Y, Target target)
        {
            MatrixChecker.SizeEquals(X, Y);
            Matrix ret = null;
            if (target == Target.EachRow)
            {
                ret = new Matrix(X.RowSize, X.RowSize);
                for (int ry = 0; ry < ret.RowSize; ++ry)
                {
                    for (int rx = 0; rx < ret.ColumnSize; ++rx)
                    {
                        ret[ry, rx] = Correlate(Y.Rows[ry], X.Rows[rx]);
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
                        ret[cy, cx] = Correlate(Y.Columns[cy], X.Columns[cx]);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 各ベクトルの相関を求める．
        /// </summary>
        /// <param name="X"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Matrix Correlate(Matrix X, Target target)
        {
            return Correlate(X, X, target);
        }

        //// 歪度
        //public static double Skewness()
        //{
        //}

        //// 尖度
        //public static double Kurtosis()
        //{
        //}
    }
}
