using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 共通の実装を提供する．外部公開されるメソッドが一部含まれる．
    /// </summary>
    public static class Vectors
    {
        /// <summary>
        /// ノルム
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double Norm(IVector v)
        {
            return Math.Sqrt(v.SumSq);
        }

        /// <summary>
        /// 要素の和
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double Sum(IVector v)
        {
            double sum = 0.0;
            for (int i = 0; i < v.Size; ++i) sum += v[i];
            return sum;
        }

        /// <summary>
        /// 要素の 2 乗和
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double SumSq(IVector v)
        {
            double sum = 0.0;
            for (int i = 0; i < v.Size; ++i) sum += (v[i] * v[i]);
            return sum;
        }

        /// <summary>
        /// 要素の平均
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double Average(IVector v)
        {
            if (v.Size == 0) return 0;
            return v.Sum / v.Size;
        }

        /// <summary>
        /// 要素の散布
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double Scatter(IVector v)
        {
            double avg = v.Average;
            double sum = 0.0;
            double elem;
            for (int i = 0; i < v.Size; ++i)
            {
                elem = v[i];
                sum += ((elem - avg) * (elem - avg));
            }
            return sum;
        }

        /// <summary>
        /// 要素の標本分散
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double SampleVariance(IVector v)
        {
            if (v.Size == 0) return 0;
            return v.Scatter / v.Size;
        }

        /// <summary>
        /// 要素の不偏分散
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double UnbiasedVariance(IVector v)
        {
            if (v.Size == 0) return 0;
            if (v.Size == 1) return Double.NaN;
            return v.Scatter / (v.Size - 1);
        }

        /// <summary>
        /// 符号反転
        /// </summary>
        internal static T Flip<T>(T v)
            where T : IRandomAccessible<double>
        {
            int size = v.Size;
            for (int i = 0; i < size; ++i)
            {
                v[i] = -v[i];
            }
            return v;
        }

        /// <summary>
        /// オール 0
        /// </summary>
        internal static T Zero<T>(T v)
            where T : IRandomAccessible<double>
        {
            return Fill(v, 0);
        }

        /// <summary>
        /// 指定値で埋める
        /// </summary>
        internal static T Fill<T>(T v, double val)
            where T : IRandomAccessible<double>
        {
            int size = v.Size;
            for (int i = 0; i < size; ++i)
            {
                v[i] = val;
            }
            return v;
        }

        internal static string ToString<T>(IEnumerable<T> v)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(Join(", ", v)).Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// <paramref name="sep"/> を区切り文字として <paramref name="seq"/> の各要素を連結する．
        /// </summary>
        internal static string Join<T>(string sep, IEnumerable<T> seq)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T val in seq)
            {
                if (0 < sb.Length)
                {
                    sb.Append(sep);
                }
                sb.Append(val);
            }
            return sb.ToString();
        }

        internal static IEnumerator<double> GetEnumerator(IRandomAccessible<double> values)
        {
            int size = values.Size;
            for (int i = 0; i < size; ++i)
            {
                yield return values[i];
            }
        }

        #region 代入演算相当

        internal static V AddEq<V>(V v1, double val)
            where V : IRandomAccessible<double>
        {
            VectorChecker.IsNotZeroSize(v1);
            int size = v1.Size;
            for (int i = 0; i < size; ++i)
            {
                v1[i] += val;
            }
            return v1;
        }

        internal static V AddEq<V>(V v1, V v2)
            where V : IRandomAccessible<double>
        {
            VectorChecker.SizeEquals(v1, v2);
            int size = v1.Size;
            for (int i = 0; i < size; ++i)
            {
                v1[i] += v2[i];
            }
            return v1;
        }

        internal static V SubEq<V>(V v1, V v2)
            where V : IRandomAccessible<double>
        {
            VectorChecker.SizeEquals(v1, v2);
            int size = v1.Size;
            for (int i = 0; i < size; ++i)
            {
                v1[i] -= v2[i];
            }
            return v1;
        }

        internal static V MulEq<V>(V v1, double val)
            where V : IRandomAccessible<double>
        {
            VectorChecker.IsNotZeroSize(v1);
            int size = v1.Size;
            for (int i = 0; i < size; ++i)
            {
                v1[i] *= val;
            }
            return v1;
        }

        internal static V DivEq<V>(V v1, double val)
            where V : IRandomAccessible<double>
        {
            VectorChecker.IsNotZeroSize(v1);
            int size = v1.Size;
            for (int i = 0; i < v1.Size; ++i)
            {
                v1[i] /= val;
            }
            return v1;
        }

        #endregion

        #region 基本演算

        internal static T Add<T>(IVector v1, IVector v2, T ret) where T : IVector
        {
            for (int i = 0; i < ret.Size; ++i)
            {
                ret[i] = v1[i] + v2[i];
            }
            return ret;
        }
        
        internal static T Sub<T>(IVector v1, IVector v2, T ret) where T : IVector
        {
            for (int i = 0; i < ret.Size; ++i)
            {
                ret[i] = v1[i] - v2[i];
            }
            return ret;
        }
        
        internal static double Dot(IVector v1, IVector v2)
        {
            double sum = 0.0;
            for (int i = 0; i < v1.Size; ++i)
            {
                sum += v1[i] * v2[i];
            }
            return sum;
        }
        
        internal static T Mul<T>(double d, IVector v, T ret) where T : IVector
        {
            for (int i = 0; i < v.Size; ++i)
            {
                ret[i] = d * v[i];
            }
            return ret;
        }

        internal static T Div<T>(IVector v, double d, T ret) where T : IVector
        {
            for (int i = 0; i < v.Size; ++i)
            {
                ret[i] = v[i] / d;
            }
            return ret;
        }

        #endregion

        #region Public

        /// <summary>
        /// 誤差を考慮した等価判定．
        /// </summary>
        /// <remarks>
        /// （<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[i] - <paramref name="right"/>[i]) &lt; <paramref name="delta"/></c> であれば同等とみなす）
        /// </remarks>
        /// <returns></returns>
        public static bool Equals(IVector left, IVector right, double delta)
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
            return left.Zip(right, (f, s) => Math.Abs(f - s)).All(d => d < delta);
        }

        /// <summary>
        /// CSV 文字列化する．
        /// </summary>
        public static string ToCsv<T>(IEnumerable<T> v)
        {
            return Join(",", v);
        }

        /// <summary>
        /// <see cref="Vector"/> を <see cref="RowVector"/> として出力する．
        /// </summary>
        public static RowVector ToRow(this Vector v)
        {
            return new RowVector(v);
        }

        public static RowVector ToRow(this IEnumerable<double> vals)
        {
            return new RowVector(vals);
        }

        /// <summary>
        /// <see cref="Vector"/> を <see cref="ColumnVector"/> として出力する．
        /// </summary>
        public static ColumnVector ToColumn(this Vector v)
        {
            return new ColumnVector(v);
        }

        public static ColumnVector ToColumn(this IEnumerable<double> vals)
        {
            return new ColumnVector(vals);
        }

        /// <summary>
        /// <see cref="IVector"/> を <see cref="Vector"/> として出力する．
        /// </summary>
        public static Vector ToVector(this IVector v)
        {
            return new Vector(v);
        }

        public static Vector ToVector(this IEnumerable<double> vals)
        {
            return new Vector(vals);
        }

        #endregion
    }
}
