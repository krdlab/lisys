using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 共通の実装を提供する．
    /// </summary>
    internal static class VectorImpl
    {
        #region IVectorのメンバ

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
        /// 要素の2乗和
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
            return v.Scatter / (v.Size - 1);
        }

        /// <summary>
        /// 符号反転
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static IVector Flip(IVector v)
        {
            for (int i = 0; i < v.Size; ++i)
            {
                v[i] = -v[i];
            }
            return v;
        }

        /// <summary>
        /// オール0
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static IVector Zero(IVector v)
        {
            for (int i = 0; i < v.Size; ++i)
            {
                v[i] = 0;
            }
            return v;
        }

        /// <summary>
        /// 配列を出力
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double[] ToArray(IVector v)
        {
            if (v.Size == 0) return null;
            double[] ret = new double[v.Size];
            for (int i = 0; i < v.Size; ++i)
            {
                ret[i] = v[i];
            }
            return ret;
        }

        /// <summary>
        /// 文字列表現を出力
        /// </summary>
        /// <param name="v">出力対象</param>
        /// <returns>文字列</returns>
        internal static string ToString(IVector v)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(");
            int size = v.Size;
            for (int i = 0; i < size; ++i)
            {
                sb.Append(v[i] + ((i < size - 1) ? (", ") : ("")));
            }
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// CSV形式で出力
        /// </summary>
        /// <param name="v">出力対象</param>
        /// <returns>CSV形式の文字列</returns>
        internal static string ToCsv(IVector v)
        {
            const string sep = ",";

            StringBuilder sb = new StringBuilder();
            foreach (double e in v)
            {
                sb.Append(e);
                sb.Append(sep);
            }

            sb.Remove(sb.Length - sep.Length, sep.Length);

            return sb.ToString();
        }

        /// <summary>
        /// 動作の適用1（ByVal）
        /// </summary>
        /// <param name="v"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static IVector ForEach(IVector v, ElementActionByVal action)
        {
            for (int i = 0; i < v.Size; ++i)
            {
                action(v[i]);
            }
            return v;
        }

        /// <summary>
        /// 動作の適用1（ByRef）
        /// </summary>
        /// <param name="v">アクションの適用対象（書き換えられる）</param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static IVector ForEach(IVector v, ElementActionByRef action)
        {
            for (int i = 0; i < v.Size; ++i)
            {
                double val = v[i];
                action(ref val);
                v[i] = val;
            }
            return v;
        }

        /// <summary>
        /// 動作の適用2（ByVal）
        /// </summary>
        /// <param name="v"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static IVector ForEach(IVector v, ElementActionByValWithIndex action)
        {
            for (int i = 0; i < v.Size; ++i)
            {
                action(i, v[i]);
            }
            return v;
        }

        /// <summary>
        /// 動作の適用2（ByRef）
        /// </summary>
        /// <param name="v">アクションの適用対象（書き換えられる）</param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static IVector ForEach(IVector v, ElementActionByRefWithIndex action)
        {
            for (int i = 0; i < v.Size; ++i)
            {
                double val = v[i];
                action(i, ref val);
                v[i] = val;
            }
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal static IVector Subvector(IVector v, int startIndex)
        {
            return Subvector(v, startIndex, v.Size - startIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static IVector Subvector(IVector v, int startIndex, int length)
        {
            if (v.Size <= startIndex)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (length < 1)
            {
                throw new ArgumentException();
            }

            int endIndex = (v.Size < startIndex + length) ? v.Size : startIndex + length;

            int[] idxs = new int[endIndex - startIndex];
            for (int i = 0; i < idxs.Length; ++i)
            {
                idxs[i] = i + startIndex;
            }

            return new SubVector(v, idxs);
        }

        #endregion

        #region Enumerator

        /// <summary>
        /// 反復子による列挙子の実装を提供する．
        /// </summary>
        /// <param name="iv">IVectorインタフェースを実装したオブジェクト</param>
        /// <returns>列挙子</returns>
        internal static IEnumerator<double> Enumerator(IVector iv)
        {
            for (int i = 0; i < iv.Size; ++i)
            {
                yield return iv[i];
            }
        }

        #endregion

        #region 演算子の定義

        internal static IVector AddEq(IVector v1, double val)
        {
            VectorChecker.ZeroSize(v1);
            for (int i = 0; i < v1.Size; ++i)
            {
                v1[i] += val;
            }
            return v1;
        }

        internal static IVector AddEq(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            for (int i = 0; i < v1.Size; ++i)
            {
                v1[i] += v2[i];
            }
            return v1;
        }

        internal static IVector SubEq(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            for (int i = 0; i < v1.Size; ++i)
            {
                v1[i] -= v2[i];
            }
            return v1;
        }

        internal static IVector MulEq(IVector v1, double val)
        {
            VectorChecker.ZeroSize(v1);
            for (int i = 0; i < v1.Size; ++i)
            {
                v1[i] *= val;
            }
            return v1;
        }

        internal static IVector DivEq(IVector v1, double val)
        {
            VectorChecker.ValueIsLessThanLimit(val);
            VectorChecker.ZeroSize(v1);
            for (int i = 0; i < v1.Size; ++i)
            {
                v1[i] /= val;
            }
            return v1;
        }

        internal static T Add<T>(T ret, IVector v1, IVector v2) where T : IVector
        {
            for (int i = 0; i < ret.Size; ++i)
            {
                ret[i] = v1[i] + v2[i];
            }
            return ret;
        }
        internal static T Sub<T>(T ret, IVector v1, IVector v2) where T : IVector
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
        internal static T Mul<T>(T ret, double d, IVector v) where T : IVector
        {
            for (int i = 0; i < v.Size; ++i)
            {
                ret[i] = d * v[i];
            }
            return ret;
        }
        internal static T Div<T>(T ret, IVector v, double d) where T : IVector
        {
            for (int i = 0; i < v.Size; ++i)
            {
                ret[i] = v[i] / d;
            }
            return ret;
        }

        #endregion

        #region 比較

        internal static bool HaveSameValues(IVector left, IVector right)
        {
            for (int i = 0; i < left.Size; ++i)
            {
                if (!krdlab.law.CalculationChecker.IsLessThanLimit(Math.Abs(left[i] - right[i])))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 指定された閾値未満の差異であれば同じであると見なす．
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="delta">閾値</param>
        /// <returns>同じ値を持つと判断された場合は<c>true</c>を，それ以外の場合は<c>false</c>を返す．</returns>
        internal static bool HaveSameValues(IVector left, IVector right, double delta)
        {
            for (int i = 0; i < left.Size; ++i)
            {
                if (!(Math.Abs(left[i] - right[i]) < delta))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
