using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ���ʂ̎�����񋟂���D
    /// </summary>
    internal static class VectorImpl
    {
        #region IVector�̃����o

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
        /// �v�f��2��a
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
            return v.Scatter / (v.Size - 1);
        }

        /// <summary>
        /// �������]
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
        /// �I�[��0
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
        /// �z����o��
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
        /// ������\�����o��
        /// </summary>
        /// <param name="v">�o�͑Ώ�</param>
        /// <returns>������</returns>
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
        /// CSV�`���ŏo��
        /// </summary>
        /// <param name="v">�o�͑Ώ�</param>
        /// <returns>CSV�`���̕�����</returns>
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
        /// ����̓K�p1�iByVal�j
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
        /// ����̓K�p1�iByRef�j
        /// </summary>
        /// <param name="v">�A�N�V�����̓K�p�Ώہi������������j</param>
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
        /// ����̓K�p2�iByVal�j
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
        /// ����̓K�p2�iByRef�j
        /// </summary>
        /// <param name="v">�A�N�V�����̓K�p�Ώہi������������j</param>
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
        /// �����q�ɂ��񋓎q�̎�����񋟂���D
        /// </summary>
        /// <param name="iv">IVector�C���^�t�F�[�X�����������I�u�W�F�N�g</param>
        /// <returns>�񋓎q</returns>
        internal static IEnumerator<double> Enumerator(IVector iv)
        {
            for (int i = 0; i < iv.Size; ++i)
            {
                yield return iv[i];
            }
        }

        #endregion

        #region ���Z�q�̒�`

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

        #region ��r

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
        /// �w�肳�ꂽ臒l�����̍��قł���Γ����ł���ƌ��Ȃ��D
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="delta">臒l</param>
        /// <returns>�����l�����Ɣ��f���ꂽ�ꍇ��<c>true</c>���C����ȊO�̏ꍇ��<c>false</c>��Ԃ��D</returns>
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
