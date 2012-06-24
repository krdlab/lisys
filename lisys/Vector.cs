using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ベクトルクラス
    /// </summary>
    [Serializable]
    public class Vector : IVector
    {
        /// <summary>
        /// 実データ．law 連携のため限定公開．
        /// </summary>
        protected internal double[] _body = new double[0];

        /// <summary>
        /// 空のオブジェクトを作成する．
        /// </summary>
        public Vector()
        {
        }

        /// <summary>
        /// サイズ指定でゼロベクトルを作成する．
        /// </summary>
        /// <param name="size"></param>
        public Vector(Size size)
        {
            this._body = new double[size];
        }

        /// <summary>
        /// コピーを作成する．
        /// </summary>
        /// <param name="v"></param>
        public Vector(Vector v)
            : this(v._body)
        {
        }

        /// <summary>
        /// 元を直接指定してベクトルを作成する．
        /// </summary>
        /// <param name="arr">ベクトルの要素</param>
        public Vector(params double[] arr)
        {
            CopyFrom(arr);
        }

        /// <summary>
        /// シーケンスから得られる値を元に持つベクトルを作成する．
        /// </summary>
        public Vector(IEnumerable<double> sequence)
        {
            CopyFrom(sequence);
        }

        /// <summary>
        /// 配列からコピーする (破壊的変更)．
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        internal Vector CopyFrom(double[] arr)
        {
            this._body = new double[arr.Length];
            arr.CopyTo(this._body, 0);
            return this;
        }

        /// <summary>
        /// 指定されたシーケンスからコピーする (破壊的変更)．
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>自身への参照</returns>
        internal Vector CopyFrom(IEnumerable<double> sequence)
        {
            int size = sequence.Count();
            if (this.Size != size)
            {
                this._body = new double[size];
            }
            for (int i = 0; i < size; ++i)
            {
                this._body[i] = sequence.ElementAt(i);
            }
            return this;
        }

        /// <summary>
        /// 要素が等しいかどうかを返す．
        /// </summary>
        /// <param name="left"><see cref="Vector"/></param>
        /// <param name="right"><see cref="Vector"/></param>
        /// <param name="delta">
        /// 各要素の値が同等であるとみなされる差異の閾値（&gt; 0）
        /// （<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[i] - <paramref name="right"/>[i]) &lt; <paramref name="delta"/></c> であれば同等とみなす）
        /// </param>
        /// <returns>等しければ <c>true</c> を，それ以外の場合は <c>false</c> を返す．</returns>
        public static bool Equals(Vector left, Vector right, double delta)
        {
            if (delta < 0)
            {
                throw new ArgumentOutOfRangeException("delta");
            }
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            if (left == null || right == null)
            {
                return false;
            }
            if (left.Size != right.Size)
            {
                return false;
            }
            int size = left.Size;
            for (int i = 0; i < size; ++i)
            {
                if (delta < Math.Abs(left._body[i] - right._body[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 文字列表現を返す．
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Vectors.ToString(this);
        }

        /// <summary>
        /// インデックスの列挙子で指定する．
        /// </summary>
        public IVector this[IEnumerable<int> indexes]
        {
            get
            {
                int ownerSize = this.Size;
                if (ownerSize < indexes.Count())
                {
                    throw new ArgumentException("over limit: indexes.Count()");
                }
                if (indexes.Any(i => i < 0 || ownerSize <= i))
                {
                    throw new ArgumentException();
                }
                return new SubVector(this, indexes);
            }
        }

        /// <summary>
        /// 個々のインデックスで指定する．
        /// </summary>
        public IVector this[params int[] indexes]
        {
            get
            {
                int ownerSize = this.Size;
                if (ownerSize < indexes.Length)
                {
                    throw new ArgumentException("over limit: indexes.Length");
                }
                if (indexes.Any(i => i < 0 || ownerSize <= i))
                {
                    throw new ArgumentException();
                }
                return new SubVector(this, indexes);
            }
        }

        IRandomAccessible<double> IRandomAccessible<double>.this[IEnumerable<int> indexes]
        {
            get
            {
                return this[indexes];
            }
        }

        IRandomAccessible<double> IRandomAccessible<double>.this[params int[] indexes]
        {
            get
            {
                return this[indexes];
            }
        }

        /// <summary>
        /// 要素を設定/取得する．
        /// </summary>
        /// <param name="i">位置</param>
        /// <returns>値</returns>
        public double this[int i]
        {
            set { this._body[i] = value; }
            get { return this._body[i]; }
        }

        /// <summary>
        /// 要素数を返す．
        /// </summary>
        public Size Size
        {
            get { return new Size(this._body.Length); }
        }

        /// <summary>
        /// ベクトルのノルム．
        /// </summary>
        public double Norm
        {
            get { return krdlab.law.func.dnrm2(this._body); }
        }

        /// <summary>
        /// ベクトル要素の和．
        /// </summary>
        public double Sum
        {
            get { return Vectors.Sum(this); }
        }

        /// <summary>
        /// ベクトル要素の二乗和．
        /// </summary>
        public double SumSq
        {
            get { return Vectors.SumSq(this); } // XXX
        }

        /// <summary>
        /// ベクトル要素の平均．
        /// </summary>
        public double Average
        {
            get { return Vectors.Average(this); }
        }

        /// <summary>
        /// ベクトル要素の散布．
        /// </summary>
        public double Scatter
        {
            get { return Vectors.Scatter(this); } // XXX
        }

        /// <summary>
        /// ベクトル要素の標本分散．
        /// </summary>
        public double SampleVariance
        {
            get { return Vectors.SampleVariance(this); }
        }

        /// <summary>
        /// 不偏分散
        /// </summary>
        public double UnbiasedVariance
        {
            get { return Vectors.UnbiasedVariance(this); }
        }

        /// <summary>
        /// ベクトルの各要素の符号を反転する．
        /// </summary>
        /// <returns>自身への参照</returns>
        public Vector Flip()
        {
            return Vectors.Flip(this);
        }

        /// <summary>
        /// ベクトルの各要素を 0 にする．
        /// </summary>
        /// <returns>自身への参照</returns>
        public Vector Zero()
        {
            return Vectors.Zero(this);
        }

        /// <summary>
        /// 指定サイズのゼロベクトルを生成する．
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector Zero(int size)
        {
            return Fill(size, 0);
        }

        /// <summary>
        /// 指定サイズの <paramref name="value"/> で埋められたベクトルを生成する．
        /// </summary>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector Fill(int size, double value)
        {
            var v = new Vector(new Size(size));
            for (int i = 0; i < size; ++i)
            {
                v._body[i] = value;
            }
            return v;
        }

        /// <summary>
        /// ベクトルの要素を配列として取得する．
        /// </summary>
        /// <returns>要素値の配列</returns>
        public double[] ToArray()
        {
            var arr = new double[this._body.Length];
            this._body.CopyTo(arr, 0);
            return arr;
        }

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 列挙子を返す．
        /// </summary>
        /// <returns></returns>
        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < this._body.Length; ++i)
            {
                yield return this._body[i];
            }
        }

        #endregion

        #region 演算の定義

        /// <summary>
        /// 単項演算子
        /// </summary>
        /// <returns>引数のコピーオブジェクト</returns>
        public static Vector operator +(Vector v)
        {
            return new Vector(v);
        }

        /// <summary>
        /// 単項演算子
        /// </summary>
        /// <returns>引数をコピーして，符号を反転させたオブジェクト</returns>
        public static Vector operator -(Vector v)
        {
            return new Vector(v).Flip();
        }

        /// <summary>
        /// 2 つの <see cref="Vector"/> オブジェクトを加算する．
        /// </summary>
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(plus(v1, v2));
        }

        /// <summary>
        /// law を利用した加算
        /// </summary>
        protected static double[] plus(Vector l, Vector r)
        {
            VectorChecker.SizeEquals(l, r);
            double[] ret = null;
            krdlab.law.func.daxpy_r(ref ret, 1, l._body, r._body);
            return ret;
        }

        /// <summary>
        /// Vector + IVector
        /// </summary>
        public static IVector operator +(Vector l, IVector r)
        {
            return Vectors.Add(l, r, new Vector(l.Size));
        }

        /// <summary>
        /// IVector + Vector
        /// </summary>
        public static IVector operator +(IVector l, Vector r)
        {
            return Vectors.Add(l, r, new Vector(l.Size));
        }

        /// <summary>
        /// 2つの<see cref="Vector"/>オブジェクトを減算する．
        /// </summary>
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(sub(v1, v2));
        }

        /// <summary>
        /// law を利用した減算
        /// </summary>
        protected static double[] sub(Vector l, Vector r)
        {
            VectorChecker.SizeEquals(l, r);
            double[] ret = null;
            // ret := -r + l
            krdlab.law.func.daxpy_r(ref ret, -1, r._body, l._body);
            return ret;
        }

        /// <summary>
        /// Vector - IVector
        /// </summary>
        public static IVector operator -(Vector l, IVector r)
        {
            return Vectors.Sub(l, r, new Vector(l.Size));
        }

        /// <summary>
        /// IVector - Vector
        /// </summary>
        public static IVector operator -(IVector l, Vector r)
        {
            return Vectors.Sub(l, r, new Vector(l.Size));
        }

        /// <summary>
        /// 内積
        /// </summary>
        public static double operator *(Vector v1, Vector v2)
        {
            return dot(v1, v2);
        }

        /// <summary>
        /// law を利用した内積
        /// </summary>
        protected static double dot(Vector l, Vector r)
        {
            VectorChecker.SizeEquals(l, r);
            return krdlab.law.func.ddot(l._body, r._body);
        }

        /// <summary>
        /// Vector * IVector
        /// </summary>
        public static double operator *(Vector l, IVector r)
        {
            return Vectors.Dot(l, r);
        }

        /// <summary>
        /// IVector * Vector
        /// </summary>
        public static double operator *(IVector l, Vector r)
        {
            return Vectors.Dot(l, r);
        }

        /// <summary>
        /// スカラ倍
        /// </summary>
        public static Vector operator *(double d, Vector v)
        {
            return new Vector(scala(d, v));
        }

        /// <summary>
        /// law を利用したスケーリング
        /// </summary>
        protected static double[] scala(double d, Vector v)
        {
            VectorChecker.IsNotZeroSize(v);
            double[] ret = null;
            krdlab.law.func.dscal_r(ref ret, v._body, d);
            return ret;
        }

        /// <summary>
        /// スカラ倍
        /// </summary>
        public static Vector operator *(Vector v, double d)
        {
            return d * v;
        }

        /// <summary>
        /// 指定スカラの逆数倍
        /// </summary>
        public static Vector operator /(Vector v, double d)
        {
            return new Vector(scala(1 / d, v));
        }

        #endregion

        #region 代入演算メソッド (copy-less operator が無いので)

        /// <summary>
        /// 各要素に <paramref name="value"/> を加算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        public Vector Add(double value)
        {
            return Vectors.AddEq(this, value);
        }
        /// <summary>
        /// このベクトルオブジェクトに<paramref name="v"/>を加算する．
        /// </summary>
        /// <param name="v">ベクトルオブジェクト</param>
        /// <returns>自身への参照</returns>
        public Vector Add(Vector v)
        {
            return Vectors.AddEq(this, v);
        }
        /// <summary>
        /// このベクトルオブジェクトから<paramref name="v"/>を減算する．
        /// </summary>
        /// <param name="v">ベクトルオブジェクト</param>
        /// <returns>自身への参照</returns>
        public Vector Sub(Vector v)
        {
            return Vectors.SubEq(this, v);
        }
        /// <summary>
        /// 各要素に<paramref name="value"/>を乗算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        public Vector Mul(double value)
        {
            return Vectors.MulEq(this, value);
        }
        /// <summary>
        /// 各要素に<paramref name="value"/>を除算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        public Vector Div(double value)
        {
            return Vectors.DivEq(this, value);
        }

        #endregion

        /// <summary>
        /// 内部イテレータ．
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<int, double> action)
        {
            int size = this._body.Length;
            for (int i = 0; i < size; ++i)
            {
                action(i, this._body[i]);
            }
        }

        /// <summary>
        /// map 操作．
        /// </summary>
        /// <param name="f"></param>
        public void Apply(Func<int, double, double> f)
        {
            int size = this._body.Length;
            for (int i = 0; i < size; ++i)
            {
                this._body[i] = f(i, this._body[i]);
            }
        }
    }
}
