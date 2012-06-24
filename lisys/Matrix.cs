using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 行列クラス
    /// </summary>
    [Serializable]
    [DebuggerVisualizer(typeof(Visualizer.MatrixVisualizer), Target = typeof(Matrix), Description = "Matrix Visualizer")]
    public partial class Matrix : IEnumerable<double>
    {
        /// <summary>
        /// 実データ．law 連携のため限定公開．
        /// </summary>
        internal double[] _body = null;
        internal int _rsize = 0;
        internal int _csize = 0;

        /// <summary>
        /// 空のオブジェクトを作成する．
        /// </summary>
        internal Matrix()
        {
            Clear();
        }

        /// <summary>
        /// 指定された配列をコピーして，新しい行列を作成する．
        /// </summary>
        /// <param name="body">コピーされる配列</param>
        /// <param name="rowSize">新しい行数</param>
        /// <param name="columnSize">新しい列数</param>
        internal Matrix(double[] body, int rowSize, int columnSize)
        {
            CopyFrom(body, rowSize, columnSize);
        }

        /// <summary>
        /// 指定されたサイズの行列を作成する．
        /// 各要素は 0 に初期化される．
        /// </summary>
        /// <param name="rowSize">行数</param>
        /// <param name="columnSize">列数</param>
        public Matrix(int rowSize, int columnSize)
        {
            Resize(rowSize, columnSize).Fill(0);
        }

        /// <summary>
        /// コピーして新しい行列を作成する．
        /// </summary>
        /// <param name="m">コピーされる行列</param>
        public Matrix(Matrix m)
        {
            CopyFrom(m);
        }

        /// <summary>
        /// ベクトル配列から新しい行列を作成する．
        /// 指定された各ベクトルは，新しい行列の各行にコピーされる．
        /// </summary>
        /// <param name="arr"></param>
        public Matrix(params RowVector[] arr)
        {
            // 入力の検証
            VectorChecker.IsNotZeroSize(arr[0]);
            int csize = arr[0].Size;
            MatrixChecker.EqualAllSizes(csize, arr);
            // 構築
            int rsize = arr.Length;
            Resize(rsize, csize);
            for (int r = 0; r < rsize; ++r)
            {
                this.Rows[r] = arr[r];
            }
        }

        /// <summary>
        /// 指定されたベクトルから新しい行列を作成する．
        /// 指定された各ベクトルは，新しい行列の各列にコピーされる．
        /// </summary>
        /// <param name="arr"></param>
        public Matrix(params ColumnVector[] arr)
        {
            // 入力の検証
            VectorChecker.IsNotZeroSize(arr[0]);
            int rsize = arr[0].Size;
            MatrixChecker.EqualAllSizes(rsize, arr);
            // 構築
            int csize = arr.Length;
            Resize(rsize, csize);
            for (int c = 0; c < csize; ++c)
            {
                this.Columns[c] = arr[c];
            }
        }

        /// <summary>
        /// 2次元配列から新しい行列を作成する．
        /// </summary>
        /// <param name="arr">行列の要素を格納した2次元配列</param>
        public Matrix(double[,] arr)
        {
            int rsize = arr.GetLength(0);
            int csize = arr.GetLength(1);

            Resize(rsize, csize);

            for (int r = 0; r < rsize; ++r)
            {
                for (int c = 0; c < csize; ++c)
                {
                    this[r, c] = arr[r, c];
                }
            }
        }

        /// <summary>
        /// 指定された 2つの<see cref="Matrix"/>が等しいかどうかを示す．
        /// </summary>
        /// <param name="left"><see cref="Matrix"/></param>
        /// <param name="right"><see cref="Matrix"/></param>
        /// <param name="delta">
        /// 各要素の値が同等であるとみなされる差異の閾値（&gt; 0）
        /// （<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[r, c] - <paramref name="right"/>[r, c]) &lt; <paramref name="delta"/></c>であれば同等とみなす）
        /// </param>
        /// <returns>等しい場合は<c>true</c>を，それ以外の場合は<c>false</c>を返す．</returns>
        public static bool Equals(Matrix left, Matrix right, double delta)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            if (left.RowSize != right.RowSize || left.ColumnSize != right.ColumnSize)
            {
                return false;
            }

            for (int r = 0; r < left.RowSize; ++r)
            {
                for (int c = 0; c < left.ColumnSize; ++c)
                {
                    if (!(Math.Abs(left[r, c] - right[r, c]) < delta))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Matrixの文字列表現を取得する．
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            this.Rows.ForEach((i, row) => sb.Append(row.ToString()));
            sb.Append(")");
            return sb.ToString();
        }

        #region プロパティの定義

        /// <summary>
        /// この行列の各要素を設定，取得する．
        /// </summary>
        /// <param name="row">行index（範囲：[0, <see cref="RowSize"/>) ）</param>
        /// <param name="col">列index（範囲：[0, <see cref="ColumnSize"/>) ）</param>
        /// <returns>要素の値</returns>
        public double this[int row, int col]
        {
            get
            {
                if (row < 0 || this.RowSize <= row || col < 0 || this.ColumnSize <= col)
                {
                    throw new IndexOutOfRangeException();
                }
                return this._body[row + col * this._rsize];
            }
            set
            {
                if (row < 0 || this.RowSize <= row || col < 0 || this.ColumnSize <= col)
                {
                    throw new IndexOutOfRangeException();
                }
                this._body[row + col * this._rsize] = value;
            }
        }

        /// <summary>
        /// このオブジェクトの行コレクションを取得する．
        /// </summary>
        public IVectorCollection<RowCollection> Rows
        {
            get
            {
                return new RowCollection(this._body, this._rsize, this._csize);
            }
        }

        /// <summary>
        /// このオブジェクトの列コレクションを取得する．
        /// </summary>
        public IVectorCollection<ColumnCollection> Columns
        {
            get
            {
                return new ColumnCollection(this._body, this._rsize, this._csize);
            }
        }

        /// <summary>
        /// このオブジェクトの行数を取得する．
        /// </summary>
        public Size RowSize
        {
            get { return new Size(this._rsize); }
        }

        /// <summary>
        /// このオブジェクトの列数を取得する．
        /// </summary>
        public Size ColumnSize
        {
            get { return new Size(this._csize); }
        }

        /// <summary>
        /// (行, 列) サイズ
        /// </summary>
        public Sizes Sizes
        {
            get { return new Sizes(this._rsize, this._csize); }
        }

        /// <summary>
        /// 各行の平均値が格納されたベクトルを取得する．
        /// </summary>
        public ColumnVector RowAverages
        {
            get { return new ColumnVector(this.Rows.Select(v => v.Average)); }
        }

        /// <summary>
        /// 各列の平均値が格納されたベクトルを取得する．
        /// </summary>
        public RowVector ColumnAverages
        {
            get { return new RowVector(this.Columns.Select(v => v.Average)); }
        }

        /// <summary>
        /// 各行の標本分散を格納したベクトルを取得する．
        /// </summary>
        public ColumnVector RowSampleVariances
        {
            get { return new ColumnVector(this.Rows.Select(v => v.SampleVariance)); }
        }

        /// <summary>
        /// 各行の不偏分散を格納したベクトルを取得する．
        /// </summary>
        public ColumnVector RowUnbiasedVariances
        {
            get { return new ColumnVector(this.Rows.Select(v => v.UnbiasedVariance)); }
        }

        /// <summary>
        /// 各列の標本分散を格納したベクトルを取得する．
        /// </summary>
        public RowVector ColumnSampleVariances
        {
            get { return new RowVector(this.Columns.Select(v => v.SampleVariance)); }
        }

        /// <summary>
        /// 各列の不偏分散を格納したベクトルを取得する．
        /// </summary>
        public RowVector ColumnUnbiasedVariances
        {
            get { return new RowVector(this.Columns.Select(v => v.UnbiasedVariance)); }
        }

        /// <summary>
        /// この行列のトレースを取得する．
        /// </summary>
        /// <remarks>
        /// 対角要素の和 = トレース = 固有値の和
        /// </remarks>
        public double Trace
        {
            get
            {
                MatrixChecker.IsSquare(this);
                double sum = 0.0;
                for (int i = 0; i < this.RowSize; ++i)
                {
                    sum += this[i, i];
                }
                return sum;
            }
        }

        /// <summary>
        /// この行列の行列式を取得する．
        /// </summary>
        /// <remarks>
        /// <para>内部では，4×4 以上の行列に対して LU 分解を利用している</para>
        /// <para>固有値の積 = 行列式</para>
        /// <para>行列式 = 0 ⇔ 少なくとも 1 つの固有値が 0</para>
        /// </remarks>
        public double Determinant
        {
            get
            {
                MatrixChecker.IsSquare(this);
                if (this.RowSize < 2)
                {
                    throw new InvalidOperationException("RowSize < 2");
                }

                if (this.RowSize == 2)
                {
                    return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
                }
                else if (this.RowSize == 3)
                {
                    Matrix m = this;
                    return m[0, 0] * m[1, 1] * m[2, 2]
                            - m[0, 0] * m[1, 2] * m[2, 1]
                            - m[0, 1] * m[1, 0] * m[2, 2]
                            + m[0, 1] * m[1, 2] * m[2, 0]
                            + m[0, 2] * m[1, 0] * m[2, 1]
                            - m[0, 2] * m[1, 1] * m[2, 0];
                }
                else
                {
                    // 4以上

                    Lud lud = Func.Lud(this);

                    // 置換回数から符号を確定する
                    int sign = lud.PermutationCount % 2 == 0 ? +1 : -1;

                    // 行列式を計算
                    double det = 1.0;
                    for (int r = 0; r < lud.U.RowSize; ++r)
                    {
                        det *= lud.U[r, r];
                    }
                    return sign * det;
                }
            }
        }

        #endregion

        /// <summary>
        /// このオブジェクトをクリアする（<c>RowSize == 0 and ColumnSize == 0</c> になる）．
        /// </summary>
        public void Clear()
        {
            this._body = new double[0];
            this._rsize = 0;
            this._csize = 0;
        }

        /// <summary>
        /// リサイズする．リサイズ後の各要素値は0になる．
        /// </summary>
        /// <param name="rowSize">新しい行数</param>
        /// <param name="columnSize">新しい列数</param>
        /// <returns>リサイズ後の自身への参照</returns>
        public Matrix Resize(int rowSize, int columnSize)
        {
            this._body = new double[rowSize * columnSize];
            this._rsize = rowSize;
            this._csize = columnSize;
            return this;
        }

        /// <summary>
        /// 指定値で埋める．
        /// </summary>
        /// <param name="val"></param>
        /// <returns>自分自身</returns>
        public Matrix Fill(double val)
        {
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] = val;
            }
            return this;
        }

        /// <summary>
        /// 指定された行列をコピーする．
        /// </summary>
        /// <param name="m">コピーされる行列</param>
        /// <returns>コピー後の自身への参照</returns>
        public Matrix CopyFrom(Matrix m)
        {
            return CopyFrom(m._body, m._rsize, m._csize);
        }

        /// <summary>
        /// <para>指定された1次元配列を，指定された行列形式でコピーする．</para>
        /// <para>配列のサイズと「rowSize * columnSize」は一致しなければならない．</para>
        /// </summary>
        /// <param name="body">コピーされる配列</param>
        /// <param name="rowSize">行数</param>
        /// <param name="columnSize">列数</param>
        /// <returns>コピー後の自身への参照</returns>
        internal Matrix CopyFrom(double[] body, int rowSize, int columnSize)
        {
            // 入力の検証
            if (body.Length != rowSize * columnSize)
            {
                throw new ArgumentException("body.Length != rowSize * columnSize");
            }

            // バッファ確保
            if (this._rsize == rowSize && this._csize == columnSize)
            {
                // 何もしない
            }
            else if (this._body != null && this._body.Length == rowSize * columnSize)
            {
                this._rsize = rowSize;
                this._csize = columnSize;
            }
            else
            {
                Resize(rowSize, columnSize);
            }

            // コピー
            body.CopyTo(this._body, 0);
            return this;
        }

        #region 変換出力

        /// <summary>
        /// 行列の各行を <see cref="RowVector"/> の列挙子として出力する．
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RowVector> ToRowVectors()
        {
            return this.Rows.Select(v => new RowVector(v));
        }

        /// <summary>
        /// 行列の各列を <see cref="ColumnVector"/> の列挙子として出力する．
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnVector> ToColumnVectors()
        {
            return this.Columns.Select(v => new ColumnVector(v));
        }

        /// <summary>
        /// 行列を 2 次元配列として出力する．
        /// </summary>
        /// <returns>2 次元配列（<c>array[r, c] == matrix[r, c]</c>）</returns>
        public double[,] ToArray()
        {
            double[,] ret = new double[this.RowSize, this.ColumnSize];
            for (int r = 0; r < this.RowSize; ++r)
            {
                for (int c = 0; c < this.ColumnSize; ++c)
                {
                    ret[r, c] = this[r, c];
                }
            }
            return ret;
        }

        #endregion

        /// <summary>
        /// この行列をゼロ行列にする．
        /// </summary>
        /// <returns></returns>
        public Matrix Zero()
        {
            int size = this._body.Length;
            for (int i = 0; i < size; ++i)
            {
                this._body[i] = 0;
            }
            return this;
        }

        /// <summary>
        /// この行列を単位行列（I = diag(1,1,...,1)）にする．
        /// </summary>
        /// <returns></returns>
        public Matrix Identity()
        {
            MatrixChecker.IsSquare(this);
            this.Zero();
            for (int i = 0; i < this.RowSize; ++i)
            {
                this[i, i] = 1;
            }
            return this;
        }

        /// <summary>
        /// 全ての要素の符号を反転する．
        /// </summary>
        /// <returns></returns>
        public Matrix Flip()
        {
            int size = this._body.Length;
            for (int i = 0; i < size; ++i)
            {
                this._body[i] = -this._body[i];
            }
            return this;
        }

        /// <summary>
        /// この行列を転置する．
        /// </summary>
        /// <returns>転置後の自身への参照</returns>
        public Matrix Transpose()
        {
            Matrix t = new Matrix(this._csize, this._rsize);
            for (int r = 0; r < this._rsize; ++r)
            {
                for (int c = 0; c < this._csize; ++c)
                {
                    t[c, r] = this[r, c];
                }
            }

            this.Clear();
            this._body = t._body;
            this._rsize = t._rsize;
            this._csize = t._csize;
            return this;
        }

        /// <summary>
        /// 逆行列化する．
        /// </summary>
        /// <returns>逆行列化後の自身への参照</returns>
        /// <exception cref="System.ArgumentException">
        /// 正方行列でないときに throw される．
        /// </exception>
        public Matrix Inverse()
        {
            MatrixChecker.IsSquare(this);

            Matrix A = new Matrix(this);
            this.Identity();

            krdlab.law.func.dgesv(ref this._body, ref this._rsize, ref this._csize,
                A._body, A._rsize, A._csize, this._body, this._rsize, this._csize);

            return this;
        }

        /// <summary>
        /// 全ての要素に指定した値を加算する．
        /// </summary>
        /// <param name="value">加算する値</param>
        /// <returns>自身への参照</returns>
        public Matrix Add(double value)
        {
            int size = this._body.Length;
            for (int i = 0; i < size; ++i)
            {
                this._body[i] += value;
            }
            return this;
        }

        /// <summary>
        /// <see cref="Matrix"/> オブジェクトを直接加算する．
        /// <code>this += m;</code> を表しているが，代入演算子のように結果オブジェクトが生成されることはない．
        /// </summary>
        /// <param name="m">加算する <see cref="Matrix"/> オブジェクト</param>
        /// <returns>加算後の自身への参照</returns>
        /// <exception cref="System.ArgumentException">
        /// 行列のサイズが一致しなかった場合に throw される．
        /// </exception>
        public Matrix Add(Matrix m)
        {
            MatrixChecker.SizeEquals(this, m);
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] += m._body[i];
            }
            return this;
        }

        /// <summary>
        /// <see cref="Matrix"/> オブジェクトを直接減算する．
        /// <code>this -= m;</code> を表しているが，代入演算子のように結果オブジェクトが生成されることはない．
        /// </summary>
        /// <param name="m">減算する <see cref="Matrix"/> オブジェクト</param>
        /// <returns>減算後の自身への参照</returns>
        /// <exception cref="System.ArgumentException">
        /// 行列のサイズが一致しなかった場合に throw される．
        /// </exception>
        public Matrix Sub(Matrix m)
        {
            MatrixChecker.SizeEquals(this, m);
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] -= m._body[i];
            }
            return this;
        }

        /// <summary>
        /// 直接スカラ倍する．
        /// <code>this *= d;</code> を表しているが，代入演算子のように結果オブジェクトが生成されることはない．
        /// </summary>
        /// <param name="d">スカラ値</param>
        /// <returns>スカラ倍後の自身への参照</returns>
        public Matrix Mul(double d)
        {
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] *= d;
            }
            return this;
        }

        /// <summary>
        /// 直接スカラ値で除算する．
        /// <code>this /= d;</code> を表しているが，代入演算子のように結果オブジェクトが生成されることはない．
        /// </summary>
        /// <param name="d">スカラ値</param>
        /// <returns>除算後の自身への参照</returns>
        public Matrix Div(double d)
        {
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] /= d;
            }
            return this;
        }


        #region 演算子の定義

        /// <summary>
        /// 行列のコピーをそのまま返す演算子．
        /// <c>-<paramref name="m"/></c> の動作と対になるように定義している．
        /// </summary>
        /// <param name="m"></param>
        /// <returns><c>+<paramref name="m"/></c></returns>
        public static Matrix operator +(Matrix m)
        {
            return new Matrix(m);
        }
        /// <summary>
        /// 符号を反転させた結果を返す演算子．
        /// </summary>
        /// <param name="m"></param>
        /// <returns><c>-<paramref name="m"/></c></returns>
        public static Matrix operator -(Matrix m)
        {
            return (new Matrix(m)).Flip();
        }
        /// <summary>
        /// 行列どうしを加算する．
        /// </summary>
        /// <param name="m1">左項となる行列</param>
        /// <param name="m2">右項となる行列</param>
        /// <returns>加算結果の行列</returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return (new Matrix(m1)).Add(m2);
        }
        /// <summary>
        /// 行列から行列を減算する．
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns>減算結果の行列</returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return (new Matrix(m1)).Sub(m2);
        }
        /// <summary>
        /// 行列どうしを乗算する．
        /// </summary>
        /// <param name="m1">左項となる行列</param>
        /// <param name="m2">右項となる行列</param>
        /// <returns>乗算結果の行列</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            MatrixChecker.CanMultiply(m1, m2);
            double[] ret = null;
            int rsize = 0;
            int csize = 0;

            krdlab.law.func.dgemm(ref ret, ref rsize, ref csize,
                                    m1._body, m1._rsize, m1._csize,
                                    m2._body, m2._rsize, m2._csize);

            return new Matrix(ret, rsize, csize);
        }
        /// <summary>
        /// 行列をスカラ倍する．
        /// </summary>
        /// <param name="m">行列</param>
        /// <param name="d">スカラ値</param>
        /// <returns>スカラ倍された行列</returns>
        public static Matrix operator *(Matrix m, double d)
        {
            return (new Matrix(m)).Mul(d);
        }
        /// <summary>
        /// 行列をスカラ倍する．
        /// </summary>
        /// <param name="d">スカラ値</param>
        /// <param name="m">行列</param>
        /// <returns>スカラ倍された行列</returns>
        public static Matrix operator *(double d, Matrix m)
        {
            return m * d;
        }
        /// <summary>
        /// 行列をスカラ値で除算する．
        /// </summary>
        /// <param name="m">行列</param>
        /// <param name="d">スカラ値</param>
        /// <returns>除算結果の行列</returns>
        public static Matrix operator /(Matrix m, double d)
        {
            return (new Matrix(m)).Div(d);
        }


        #region 出力の型が変化する演算の定義

        /// <summary>
        /// 行列と列ベクトルの乗算
        /// </summary>
        /// <param name="m">行列</param>
        /// <param name="cv">列ベクトル</param>
        /// <returns>乗算結果の列ベクトル</returns>
        public static ColumnVector operator *(Matrix m, ColumnVector cv)
        {
            MatrixChecker.CanMultiply(m, cv);
            ColumnVector ret = new ColumnVector();

            krdlab.law.func.dgemv(ref ret._body, m._body, m._rsize, m._csize, cv._body);

            return ret;
        }
        /// <summary>
        /// 行ベクトルと行列の乗算
        /// </summary>
        /// <param name="rv">行ベクトル</param>
        /// <param name="m">行列</param>
        /// <returns>乗算結果の行ベクトル</returns>
        public static RowVector operator *(RowVector rv, Matrix m)
        {
            MatrixChecker.CanMultiply(rv, m);
            RowVector ret = new RowVector();

            krdlab.law.func.dgemv(ref ret._body, rv._body, m._body, m._rsize, m._csize);

            return ret;
        }

        #endregion

        #endregion

        public IEnumerator<double> GetEnumerator()
        {
            int rows = RowSize;
            int cols = ColumnSize;
            for (int r = 0; r < rows; ++r)
            {
                for (int c = 0; c < cols; ++c)
                {
                    yield return this[r, c];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
