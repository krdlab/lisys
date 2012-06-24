using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 複素数（Parse系メソッドは未実装）
    /// </summary>
    [Serializable]
    public struct Complex : IEquatable<Complex>
    {
        private double real;
        private double imaginary;

        #region プロパティ

        /// <summary>
        /// 実数部を設定・取得する．
        /// </summary>
        public double Real
        {
            set { this.real = value; }
            get { return this.real; }
        }

        /// <summary>
        /// 虚数部を設定・取得する．
        /// </summary>
        public double Imaginary
        {
            set { this.imaginary = value; }
            get { return this.imaginary; }
        }

        /// <summary>
        /// 複素平面上での大きさを取得する．
        /// </summary>
        public double Size
        {
            get { return Complex.Abs(this); }
        }

        /// <summary>
        /// 複素平面上での偏角を取得する．
        /// </summary>
        public double Angle
        {
            get { return Complex.Arg(this); }
        }

        #endregion

        /// <summary>
        /// 別の複素数オブジェクトから作成する．
        /// </summary>
        /// <param name="c"></param>
        public Complex(Complex c)
        {
            this.real = c.real;
            this.imaginary = c.imaginary;
        }

        /// <summary>
        /// 各要素を指定して作成する．
        /// </summary>
        /// <param name="real">実部</param>
        /// <param name="imaginary">虚部</param>
        public Complex(double real, double imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        #region 基本メソッド

        /// <summary>
        /// このオブジェクトと等価であるか否かを判定する．
        /// </summary>
        /// <param name="obj">判定したいオブジェクト</param>
        /// <returns>等価である場合はtrueを，それ以外の場合はfalseを返す．</returns>
        public override bool Equals(object obj)
        {
            if (obj is Complex)
            {
                return Complex.Equals(this, (Complex)obj);
            }
            return false;
        }

        #region IEquatable<Complex> メンバ

        /// <summary>
        /// このオブジェクトと等価であるか否かを判定する．
        /// </summary>
        /// <param name="other">判定したいオブジェクト</param>
        /// <returns>等価である場合はtrueを，それ以外の場合はfalseを返す．</returns>
        public bool Equals(Complex other)
        {
            return Complex.Equals(this, other);
        }

        #endregion

        /// <summary>
        /// 2つのオブジェクトが等価であるか否かを判定する．
        /// 許容誤差には，<see cref="LisysConfig.CalculationLowerLimit"/>が使用される．
        /// </summary>
        /// <param name="left"><see cref="Complex"/></param>
        /// <param name="right"><see cref="Complex"/></param>
        /// <returns>等価である場合はtrueを，それ以外の場合はfalseを返す．</returns>
        public static bool Equals(Complex left, Complex right)
        {
            return Complex.Equals(left, right, LisysConfig.CalculationLowerLimit);
        }

        /// <summary>
        /// 2つのオブジェクトが等価であるか否かを判定する．
        /// </summary>
        /// <param name="left"><see cref="Complex"/></param>
        /// <param name="right"><see cref="Complex"/></param>
        /// <param name="delta">許容誤差（この数値未満の差異は無視する）</param>
        /// <returns>等価である場合はtrueを，それ以外の場合はfalseを返す．</returns>
        public static bool Equals(Complex left, Complex right, double delta)
        {
            if (Math.Abs(left.real - right.real) < delta
                && Math.Abs(left.imaginary - right.imaginary) < delta)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ハッシュコードを取得する．
        /// （※現在の実装は，同一のハッシュコードが発行されやすいので注意せよ）
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            return (int)Math.Round(this.real + this.imaginary);
        }

        /// <summary>
        /// このオブジェクトの文字列表現を返す．
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public override string ToString()
        {
            return string.Format("{0} + {1}i", this.real, this.imaginary);
        }

        #endregion

        /// <summary>
        /// 絶対値を計算する．
        /// </summary>
        /// <param name="c"><see cref="Complex"/></param>
        /// <returns>引数の絶対値</returns>
        public static double Abs(Complex c)
        {
            return Math.Sqrt(c.real * c.real + c.imaginary * c.imaginary);
        }

        /// <summary>
        /// 偏角を計算する．
        /// </summary>
        /// <param name="c"><see cref="Complex"/></param>
        /// <returns>引数の偏角</returns>
        /// <seealso cref="Math.Atan2"/>
        public static double Arg(Complex c)
        {
            return Math.Atan2(c.imaginary, c.real);
        }

        #region 代入演算

        private Complex Add(Complex right)
        {
            this.real += right.real;
            this.imaginary += right.imaginary;
            return this;
        }

        private Complex Sub(Complex right)
        {
            this.real -= right.real;
            this.imaginary -= right.imaginary;
            return this;
        }

        private Complex Mul(Complex right)
        {
            double a = this.real;
            double b = this.imaginary;
            
            this.real = a * right.real - b * right.imaginary;
            this.imaginary = a * right.imaginary + b * right.real;
            return this;
        }

        private Complex Div(Complex right)
        {
            double a = this.real;
            double b = this.imaginary;
            double c = right.real;
            double d = right.imaginary;
            double denominator = c * c + d * d;

            this.real = (a * c + b * d) / denominator;
            this.imaginary = (b * c - a * d) / denominator;
            return this;
        }

        #endregion

        #region 演算子

        /// <summary>
        /// <see cref="double"/>型に対する明示的な型変換を定義する．
        /// </summary>
        /// <param name="value">変換対象となる<see cref="double"/>オブジェクト</param>
        /// <returns><see cref="Complex"/></returns>
        public static explicit operator Complex(double value)
        {
            return new Complex(value, 0.0);
        }

        /// <summary>
        /// 複素数どうしの加算を定義する．
        /// </summary>
        /// <param name="left">左項の<see cref="Complex"/></param>
        /// <param name="right">右項の<see cref="Complex"/></param>
        /// <returns>加算結果の<see cref="Complex"/></returns>
        public static Complex operator +(Complex left, Complex right)
        {
            return new Complex(left).Add(right);
        }

        /// <summary>
        /// 複素数どうしの減算を定義する．
        /// </summary>
        /// <param name="left">左項の<see cref="Complex"/></param>
        /// <param name="right">右項の<see cref="Complex"/></param>
        /// <returns>減算結果の<see cref="Complex"/></returns>
        public static Complex operator -(Complex left, Complex right)
        {
            return new Complex(left).Sub(right);
        }

        /// <summary>
        /// 複素数どうしの乗算を定義する．
        /// </summary>
        /// <param name="left">左項の<see cref="Complex"/></param>
        /// <param name="right">右項の<see cref="Complex"/></param>
        /// <returns>乗算結果の<see cref="Complex"/></returns>
        public static Complex operator *(Complex left, Complex right)
        {
            return new Complex(left).Mul(right);
        }

        /// <summary>
        /// 複素数どうしの除算を定義する．
        /// </summary>
        /// <param name="left">左項の<see cref="Complex"/></param>
        /// <param name="right">右項の<see cref="Complex"/></param>
        /// <returns>除算結果の<see cref="Complex"/></returns>
        public static Complex operator /(Complex left, Complex right)
        {
            return new Complex(left).Div(right);
        }

        #endregion
    }
}
