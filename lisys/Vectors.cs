using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ���ʂ̎�����񋟂���D�O�����J����郁�\�b�h���ꕔ�܂܂��D
    /// </summary>
    public static class Vectors
    {
        /// <summary>
        /// �m����
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double Norm(IVector v)
        {
            return Math.Sqrt(v.SumSq);
        }

        /// <summary>
        /// �v�f�̘a
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
        /// �v�f�� 2 ��a
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
        /// �v�f�̕���
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double Average(IVector v)
        {
            if (v.Size == 0) return 0;
            return v.Sum / v.Size;
        }

        /// <summary>
        /// �v�f�̎U�z
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
        /// �v�f�̕W�{���U
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static double SampleVariance(IVector v)
        {
            if (v.Size == 0) return 0;
            return v.Scatter / v.Size;
        }

        /// <summary>
        /// �v�f�̕s�Ε��U
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
        /// �������]
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
        /// �I�[�� 0
        /// </summary>
        internal static T Zero<T>(T v)
            where T : IRandomAccessible<double>
        {
            return Fill(v, 0);
        }

        /// <summary>
        /// �w��l�Ŗ��߂�
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
        /// <paramref name="sep"/> ����؂蕶���Ƃ��� <paramref name="seq"/> �̊e�v�f��A������D
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

        #region ������Z����

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

        #region ��{���Z

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
        /// �덷���l��������������D
        /// </summary>
        /// <remarks>
        /// �i<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[i] - <paramref name="right"/>[i]) &lt; <paramref name="delta"/></c> �ł���Γ����Ƃ݂Ȃ��j
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
        /// CSV �����񉻂���D
        /// </summary>
        public static string ToCsv<T>(IEnumerable<T> v)
        {
            return Join(",", v);
        }

        /// <summary>
        /// <see cref="Vector"/> �� <see cref="RowVector"/> �Ƃ��ďo�͂���D
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
        /// <see cref="Vector"/> �� <see cref="ColumnVector"/> �Ƃ��ďo�͂���D
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
        /// <see cref="IVector"/> �� <see cref="Vector"/> �Ƃ��ďo�͂���D
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
