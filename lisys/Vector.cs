using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ベクトルクラス
    /// </summary>
    [Serializable]
    public class Vector : IResizableVector, ICsv, IEquatable<Vector>
    {
        internal double[] _body = null;

        #region コンストラクション

        /// <summary>
        /// 空のオブジェクトを作成する．
        /// </summary>
        internal Vector()
        {
            Clear();
        }

        /// <summary>
        /// 指定されたサイズのベクトルを作成する．
        /// 各要素は0に初期化される．
        /// </summary>
        /// <param name="size">ベクトルのサイズ（要素数）</param>
        public Vector(int size)
        {
            Resize(size, 0.0);
        }

        /// <summary>
        /// インタフェースを通して，指定されたオブジェクトのコピーを作成する．
        /// </summary>
        /// <param name="v">コピーされるオブジェクト</param>
        public Vector(IVector v)
        {
            CopyFrom(v);
        }

        /// <summary>
        /// 任意の個数の要素を直接指定してベクトルを作成する．
        /// </summary>
        /// <param name="arr">任意の個数のベクトルの要素</param>
        public Vector(params double[] arr)
        {
            CopyFrom(arr);
        }

        #endregion

        #region IEquatable<Vector> メンバ

        /// <summary>
        /// 指定された<see cref="Vector"/>が，自身と等しいかどうかを示す．
        /// </summary>
        /// <param name="other"><see cref="Vector"/></param>
        /// <returns>等しい場合は <c>true</c> を，それ以外は <c>false</c> を返す．</returns>
        public bool Equals(Vector other)
        {
            return Vector.Equals(this, other);
        }

        #endregion

        #region Objectメソッド

        /// <summary>
        /// 指定された<see cref="object"/>が，自分と等しいかどうかを示す．
        /// </summary>
        /// <param name="obj">比較対象</param>
        /// <returns>等しい場合は <c>true</c> を，それ以外は <c>false</c> を返す．</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj, -1);
        }

        /// <summary>
        /// 指定された<see cref="object"/>が，自分と等しいかどうかを示す．
        /// </summary>
        /// <param name="obj">比較対象</param>
        /// <param name="delta">閾値（詳細は <see cref="Vector.Equals(Vector, Vector, double)"/>）</param>
        /// <returns>等しい場合は <c>true</c> を，それ以外は <c>false</c> を返す．</returns>
        public bool Equals(object obj, double delta)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            // obj is Vector であることが保証される
            return Vector.Equals(this, (Vector)obj, delta);
        }

        /// <summary>
        /// 指定された 2つの<see cref="Vector"/> が等しいかどうかを示す．
        /// </summary>
        /// <param name="left"><see cref="Vector"/></param>
        /// <param name="right"><see cref="Vector"/></param>
        /// <returns>等しければ<c>true</c>を，それ以外の場合は<c>false</c>を返す．</returns>
        public static bool Equals(Vector left, Vector right)
        {
            return Equals(left, right, -1);
        }

        /// <summary>
        /// 指定された 2つの<see cref="Vector"/> が等しいかどうかを示す．
        /// </summary>
        /// <param name="left"><see cref="Vector"/></param>
        /// <param name="right"><see cref="Vector"/></param>
        /// <param name="delta">
        /// 各要素の値が同等であるとみなされる差異の閾値（&gt; 0）
        /// （<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[i] - <paramref name="right"/>[i]) &lt; <paramref name="delta"/></c>であれば同等とみなす）
        /// </param>
        /// <returns>等しければ<c>true</c>を，それ以外の場合は<c>false</c>を返す．</returns>
        public static bool Equals(Vector left, Vector right, double delta)
        {
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

            return delta < 0 ?
                  VectorImpl.HaveSameValues(left, right)
                : VectorImpl.HaveSameValues(left, right, delta);
        }

        /// <summary>
        /// このオブジェクトのハッシュ値を返す．
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            return this.Size ^ (~((int)this[0]));
        }

        /// <summary>
        /// ベクトルの文字列表現を取得する．
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return VectorImpl.ToString(this);
        }

        #endregion

        #region 固有の実装

        /// <summary>
        /// 指定されたベクトルをコピーする．
        /// </summary>
        /// <param name="v">コピーされるベクトル</param>
        /// <returns>自身への参照</returns>
        public Vector CopyFrom(IVector v)
        {
            if (!this.Equals(v))
            {
                if (this.Size != v.Size)
                {
                    this._body = new double[v.Size];
                }

                for (int i = 0; i < v.Size; ++i)
                {
                    this._body[i] = v[i];
                }
            }
            return this;
        }

        /// <summary>
        /// 指定された配列をコピーする．
        /// </summary>
        /// <param name="arr">コピーされる配列</param>
        /// <returns>自身への参照</returns>
        public Vector CopyFrom(double[] arr)
        {
            if (this.Size != arr.Length)
            {
                this._body = new double[arr.Length];
            }

            for (int i = 0; i < arr.Length; ++i)
            {
                this._body[i] = arr[i];
            }
            return this;
        }

        /// <summary>
        /// ベクトルの要素をクリアする（<c>Size == 0</c> になる）．
        /// </summary>
        public void Clear()
        {
            this._body = new double[0];
        }


        #region IResizableVector メンバ

        /// <summary>
        /// ベクトルのサイズを指定されたサイズに変更する．
        /// </summary>
        /// <param name="size">新しいサイズ</param>
        /// <returns>リサイズ後の自身への参照</returns>
        public IVector Resize(int size)
        {
            this._body = new double[size];
            return this;
        }

        #endregion


        /// <summary>
        /// ベクトルのサイズを指定されたサイズに変更し，指定された値で埋める．
        /// </summary>
        /// <param name="size">新しいサイズ</param>
        /// <param name="val">要素に指定する値</param>
        /// <returns>リサイズ後の自身への参照</returns>
        public IVector Resize(int size, double val)
        {
            this._body = new double[size];
            for (int i = 0; i < size; ++i)
            {
                this._body[i] = val;
            }
            return this;
        }


        /// <summary>
        /// 要素indexの配列で指定された要素を抽出する．
        /// </summary>
        /// <param name="indexes">抽出したい要素の<c>index</c>配列</param>
        /// <returns>抽出した要素にアクセス可能な<see cref="IVector"/>インタフェース</returns>
        /// <exception cref="Exception.IllegalArgumentException">
        /// 引数が不正の場合にthrowされる．
        /// </exception>
        /// <exception cref="Exception.ZeroSizeException">
        /// ベクトルサイズが0の場合にthrowされる．
        /// </exception>
        public IVector this[params int[] indexes]
        {
            get
            {
                if (indexes == null)
                {
                    throw new Exception.IllegalArgumentException("\"indexes\" is null.");
                }

                VectorChecker.ZeroSize(this);

                if (this.Size < indexes.Length)
                {
                    throw new Exception.IllegalArgumentException("indexes.Length is greater than this.Size");
                }
                foreach (int i in indexes)
                {
                    if (i < 0 || this.Size <= i)
                    {
                        throw new Exception.IllegalArgumentException(
                                    "Index=" + i + " (which is included in the indexes) is out of range.");
                    }
                }
                return new SubVector(this, indexes);
            }
        }

        #endregion

        #region IVector メンバ

        /// <summary>
        /// 指定されたindexの要素を設定・取得する．
        /// </summary>
        /// <param name="i">要素のindex</param>
        /// <returns>indexで指定された要素値</returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// <c><paramref name="i"/> &lt; 0 or <see cref="Size"/> &lt;= <paramref name="i"/></c> の場合にthrowされる．
        /// </exception>
        public double this[int i]
        {
            set { this._body[i] = value; }
            get { return this._body[i]; }
        }

        /// <summary>
        /// ベクトルのサイズを取得する．
        /// </summary>
        public int Size
        {
            get
            {
                if (this._body == null)
                {
                    return 0;
                }
                return this._body.Length;
            }
        }

        /// <summary>
        /// ベクトルのノルムを取得する．
        /// </summary>
        public double Norm
        {
            get { return VectorImpl.Norm(this); }
        }

        /// <summary>
        /// ベクトルの要素の和を取得する．
        /// </summary>
        public double Sum
        {
            get { return VectorImpl.Sum(this); }
        }

        /// <summary>
        /// ベクトルの各要素を2乗し，それらの和を取得する．
        /// </summary>
        public double SumSq
        {
            get { return VectorImpl.SumSq(this); }
        }

        /// <summary>
        /// ベクトルの要素の平均を取得する．
        /// </summary>
        public double Average
        {
            get { return VectorImpl.Average(this); }
        }

        /// <summary>
        /// ベクトルの要素の散布を取得する．
        /// </summary>
        public double Scatter
        {
            get { return VectorImpl.Scatter(this); }
        }

        /// <summary>
        /// ベクトルの要素の標本分散を取得する．
        /// </summary>
        public double Variance
        {
            get { return VectorImpl.SampleVariance(this); }
        }

        /// <summary>
        /// ベクトルの各要素の符号を反転する．
        /// </summary>
        /// <returns>自身への参照</returns>
        public IVector Flip()
        {
            return VectorImpl.Flip(this);
        }

        /// <summary>
        /// ベクトルの各要素を0にする．
        /// </summary>
        /// <returns>自身への参照</returns>
        public IVector Zero()
        {
            return VectorImpl.Zero(this);
        }

        /// <summary>
        /// ベクトルの要素を配列として取得する．
        /// </summary>
        /// <returns>要素値の配列</returns>
        public double[] ToArray()
        {
            return VectorImpl.ToArray(this);
        }

        /// <summary>
        /// ベクトルの各要素に対してactionを適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByVal"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        public IVector ForEach(ElementActionByVal action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// ベクトルの各要素に対してactionを適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRef"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        public IVector ForEach(ElementActionByRef action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// ベクトルの各要素に対してactionを適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByValWithIndex"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        public IVector ForEach(ElementActionByValWithIndex action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// ベクトルの各要素に対してactionを適用する．
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRefWithIndex"/>により規定されたデリゲート
        /// </param>
        /// <returns>メソッド適用後の自身への参照</returns>
        public IVector ForEach(ElementActionByRefWithIndex action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// 部分ベクトルを返す．
        /// </summary>
        /// <param name="startIndex">部分ベクトルの開始位置</param>
        /// <returns><paramref name="startIndex"/>から終端までの部分ベクトル</returns>
        /// <remarks>
        /// このベクトルが保持する要素の部分集合である部分ベクトルを返す．
        /// 部分ベクトルが保持する要素は，このベクトルの要素と同じものである．
        /// </remarks>
        public IVector Subvector(int startIndex)
        {
            return VectorImpl.Subvector(this, startIndex);
        }

        /// <summary>
        /// 部分ベクトルを返す．
        /// </summary>
        /// <param name="startIndex">部分ベクトルの開始位置</param>
        /// <param name="length">開始位置からの長さ</param>
        /// <returns>
        /// <paramref name="startIndex"/>から，長さ<paramref name="length"/>の部分ベクトルを返す．
        /// <paramref name="length"/>が<see cref="IVector.Size"/>を超える場合は，
        /// 終端まで（[<paramref name="startIndex"/>, <see cref="IVector.Size"/>)）の部分ベクトルを返す．
        /// </returns>
        /// <remarks>
        /// このベクトルが保持する要素の部分集合である部分ベクトルを返す．
        /// 部分ベクトルが保持する要素は，このベクトルの要素と同じものである．
        /// </remarks>
        public IVector Subvector(int startIndex, int length)
        {
            return VectorImpl.Subvector(this, startIndex, length);
        }

        #endregion

        #region ICsv メンバ

        /// <summary>
        /// CSV形式の文字列で出力する．
        /// </summary>
        /// <returns>CSV形式の文字列</returns>
        public string ToCsv()
        {
            return VectorImpl.ToCsv(this);
        }

        #endregion

        #region IEnumerable メンバ

        IEnumerator IEnumerable.GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion

        #region IEnumerable<double> メンバ

        /// <summary>
        /// 列挙子を取得する．
        /// </summary>
        /// <returns>列挙子</returns>
        public IEnumerator<double> GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion

        #region 演算の定義

        /// <summary>
        /// 単項演算子
        /// </summary>
        /// <param name="v">演算子適用対象</param>
        /// <returns>引数のコピーオブジェクト</returns>
        public static Vector operator +(Vector v)
        {
            return new Vector(v);
        }

        /// <summary>
        /// 単項演算子
        /// </summary>
        /// <param name="v">演算子適用対象</param>
        /// <returns>引数をコピーして，符号を反転させたオブジェクト</returns>
        public static Vector operator -(Vector v)
        {
            return (Vector)new Vector(v).Flip();
        }

        /// <summary>
        /// 2つの<see cref="Vector"/>オブジェクトを加算する．
        /// </summary>
        /// <param name="v1">加算演算子の左項</param>
        /// <param name="v2">加算演算子の右項</param>
        /// <returns>演算の結果を表す<see cref="Vector"/>オブジェクト</returns>
        /// <exception cref="System.Exception">
        /// 左項と右項のベクトルサイズが一致しない場合にthrowされる．
        /// </exception>
        public static Vector operator +(Vector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="Vector"/>オブジェクトと，<see cref="IVector"/>インタフェースを実装したオブジェクトとを加算する．
        /// </summary>
        /// <param name="v1">加算演算子の左項（<see cref="Vector"/>オブジェクト）</param>
        /// <param name="v2">加算演算子の右項（<see cref="IVector"/>インタフェースを実装したオブジェクト）</param>
        /// <returns>演算の結果を表す<see cref="Vector"/>オブジェクト</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// 左項と右項ののベクトルサイズが一致しない場合にthrowされる．
        /// </exception>
        public static Vector operator +(Vector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="IVector"/>インタフェースを実装したオブジェクトと，<see cref="Vector"/>オブジェクトとを加算する．
        /// </summary>
        /// <param name="v1">加算演算子の左項（<see cref="IVector"/>インタフェースを実装したオブジェクト）</param>
        /// <param name="v2">加算演算子の右項（<see cref="Vector"/>オブジェクト）</param>
        /// <returns>演算の結果を表す<see cref="Vector"/>オブジェクト</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// 左項と右項ののベクトルサイズが一致しない場合にthrowされる．
        /// </exception>
        public static Vector operator +(IVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 2つの<see cref="Vector"/>オブジェクトを減算する．
        /// </summary>
        /// <param name="v1">減算演算子の左項（減じられる側）</param>
        /// <param name="v2">減算演算子の右項（減じる側）</param>
        /// <returns>演算の結果を表す<see cref="Vector"/>オブジェクト</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// 左項と右項ののベクトルサイズが一致しない場合にthrowされる．
        /// </exception>
        public static Vector operator -(Vector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="Vector"/>オブジェクトと，<see cref="IVector"/>インタフェースを実装したオブジェクトとを減算する．
        /// </summary>
        /// <param name="v1">減算演算子の左項（減じられる側：<see cref="Vector"/>オブジェクト）</param>
        /// <param name="v2">減算演算子の右項（減じる側：<see cref="IVector"/>インタフェースを実装したオブジェクト）</param>
        /// <returns>演算の結果を表す<see cref="Vector"/>オブジェクト</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// 左項と右項ののベクトルサイズが一致しない場合にthrowされる．
        /// </exception>
        public static Vector operator -(Vector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="IVector"/>インタフェースを実装したオブジェクトと，<see cref="Vector"/>オブジェクトとを減算する．
        /// </summary>
        /// <param name="v1">減算演算子の左項（減じられる側：<see cref="IVector"/>インタフェースを実装したオブジェクト）</param>
        /// <param name="v2">減算演算子の右項（減じる側：<see cref="Vector"/>オブジェクト）</param>
        /// <returns>演算の結果を表す<see cref="Vector"/>オブジェクト</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// 左項と右項ののベクトルサイズが一致しない場合にthrowされる．
        /// </exception>
        public static Vector operator -(IVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(IVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Dot(v1, v2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector operator *(double d, Vector v)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new Vector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector operator *(Vector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new Vector(v.Size), d, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector operator /(Vector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new Vector(v.Size), v, d);
        }

        #endregion

        #region 代入演算メソッド

        /// <summary>
        /// 各要素に<paramref name="value"/>を加算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        public IVector Add(double value)
        {
            return VectorImpl.AddEq(this, value);
        }
        /// <summary>
        /// このベクトルオブジェクトに<paramref name="v"/>を加算する．
        /// </summary>
        /// <param name="v">ベクトルオブジェクト</param>
        /// <returns>自身への参照</returns>
        public IVector Add(IVector v)
        {
            return VectorImpl.AddEq(this, v);
        }
        /// <summary>
        /// このベクトルオブジェクトから<paramref name="v"/>を減算する．
        /// </summary>
        /// <param name="v">ベクトルオブジェクト</param>
        /// <returns>自身への参照</returns>
        public IVector Sub(IVector v)
        {
            return VectorImpl.SubEq(this, v);
        }
        /// <summary>
        /// 各要素に<paramref name="value"/>を乗算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        public IVector Mul(double value)
        {
            return VectorImpl.MulEq(this, value);
        }
        /// <summary>
        /// 各要素に<paramref name="value"/>を除算する．
        /// </summary>
        /// <param name="value">スカラ値</param>
        /// <returns>自身への参照</returns>
        public IVector Div(double value)
        {
            return VectorImpl.DivEq(this, value);
        }

        #endregion

        #region IVectorに対する演算メソッド

        /// <summary>
        /// 2つの<see cref="IVector"/>インタフェースを実装したオブジェクトを加算して
        /// <see cref="Vector"/>オブジェクトを作成する．
        /// </summary>
        /// <param name="v1"><see cref="IVector"/>インタフェースを実装したオブジェクト（左項）</param>
        /// <param name="v2"><see cref="IVector"/>インタフェースを実装したオブジェクト（右項）</param>
        /// <returns>加算結果を格納した<see cref="Vector"/>オブジェクト</returns>
        public static Vector Add(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 2つの<see cref="IVector"/>インタフェースを実装したオブジェクトに減算を適用し，
        /// <see cref="Vector"/>オブジェクトを作成する．
        /// </summary>
        /// <param name="v1"><see cref="IVector"/>インタフェースを実装したオブジェクト（左項）</param>
        /// <param name="v2"><see cref="IVector"/>インタフェースを実装したオブジェクト（右項）</param>
        /// <returns>減算結果を格納した<see cref="Vector"/>オブジェクト</returns>
        public static Vector Sub(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="IVector"/>インタフェースを実装したオブジェクトをスカラ倍し，
        /// <see cref="Vector"/>オブジェクトを作成する．
        /// </summary>
        /// <param name="v"><see cref="IVector"/>インタフェースを実装したオブジェクト</param>
        /// <param name="d">スカラ値</param>
        /// <returns>スカラ倍した結果を格納した<see cref="Vector"/>オブジェクト</returns>
        public static Vector Mul(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new Vector(v.Size), d, v);
        }

        /// <summary>
        /// <see cref="IVector"/>インタフェースを実装したオブジェクトをスカラ値で除算し，
        /// <see cref="Vector"/>オブジェクトを作成する．
        /// </summary>
        /// <param name="v"><see cref="IVector"/>インタフェースを実装したオブジェクト</param>
        /// <param name="d">除算値（スカラ）</param>
        /// <returns>1/d を乗じた結果を格納した<see cref="Vector"/>オブジェクト</returns>
        public static Vector Div(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new Vector(v.Size), v, d);
        }

        #endregion


        /// <summary>
        /// 指定された複数の<see cref="IVector"/>インタフェースを実装したオブジェクトにおいて，
        /// それらが同じ要素値を持っているかどうかを判定する．
        /// </summary>
        /// <param name="vectors">可変個の<see cref="IVector"/>インタフェースを実装したオブジェクト</param>
        /// <returns>
        /// 全てのオブジェクトにおいて，全ての要素値が一致すれば<c>true</c>を，それ以外は<c>false</c>を返す．
        /// </returns>
        public static bool HaveSameValues(params IVector[] vectors)
        {
            if (vectors == null)
            {
                return false;   // 値を持たないから
            }

            if (vectors.Length <= 1)
            {
                return true;
            }

            IVector criterion = vectors[0];
            for (int i = 1; i < vectors.Length; ++i)
            {
                if (!VectorImpl.HaveSameValues(criterion, vectors[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
