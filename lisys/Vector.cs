using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �x�N�g���N���X
    /// </summary>
    [Serializable]
    public class Vector : IVector
    {
        /// <summary>
        /// ���f�[�^�Dlaw �A�g�̂��ߌ�����J�D
        /// </summary>
        protected internal double[] _body = new double[0];

        /// <summary>
        /// ��̃I�u�W�F�N�g���쐬����D
        /// </summary>
        public Vector()
        {
        }

        /// <summary>
        /// �T�C�Y�w��Ń[���x�N�g�����쐬����D
        /// </summary>
        /// <param name="size"></param>
        public Vector(Size size)
        {
            this._body = new double[size];
        }

        /// <summary>
        /// �R�s�[���쐬����D
        /// </summary>
        /// <param name="v"></param>
        public Vector(Vector v)
            : this(v._body)
        {
        }

        /// <summary>
        /// ���𒼐ڎw�肵�ăx�N�g�����쐬����D
        /// </summary>
        /// <param name="arr">�x�N�g���̗v�f</param>
        public Vector(params double[] arr)
        {
            CopyFrom(arr);
        }

        /// <summary>
        /// �V�[�P���X���瓾����l�����Ɏ��x�N�g�����쐬����D
        /// </summary>
        public Vector(IEnumerable<double> sequence)
        {
            CopyFrom(sequence);
        }

        /// <summary>
        /// �z�񂩂�R�s�[���� (�j��I�ύX)�D
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
        /// �w�肳�ꂽ�V�[�P���X����R�s�[���� (�j��I�ύX)�D
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>���g�ւ̎Q��</returns>
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
        /// �v�f�����������ǂ�����Ԃ��D
        /// </summary>
        /// <param name="left"><see cref="Vector"/></param>
        /// <param name="right"><see cref="Vector"/></param>
        /// <param name="delta">
        /// �e�v�f�̒l�������ł���Ƃ݂Ȃ���鍷�ق�臒l�i&gt; 0�j
        /// �i<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[i] - <paramref name="right"/>[i]) &lt; <paramref name="delta"/></c> �ł���Γ����Ƃ݂Ȃ��j
        /// </param>
        /// <returns>��������� <c>true</c> ���C����ȊO�̏ꍇ�� <c>false</c> ��Ԃ��D</returns>
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
        /// ������\����Ԃ��D
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Vectors.ToString(this);
        }

        /// <summary>
        /// �C���f�b�N�X�̗񋓎q�Ŏw�肷��D
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
        /// �X�̃C���f�b�N�X�Ŏw�肷��D
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
        /// �v�f��ݒ�/�擾����D
        /// </summary>
        /// <param name="i">�ʒu</param>
        /// <returns>�l</returns>
        public double this[int i]
        {
            set { this._body[i] = value; }
            get { return this._body[i]; }
        }

        /// <summary>
        /// �v�f����Ԃ��D
        /// </summary>
        public Size Size
        {
            get { return new Size(this._body.Length); }
        }

        /// <summary>
        /// �x�N�g���̃m�����D
        /// </summary>
        public double Norm
        {
            get { return krdlab.law.func.dnrm2(this._body); }
        }

        /// <summary>
        /// �x�N�g���v�f�̘a�D
        /// </summary>
        public double Sum
        {
            get { return Vectors.Sum(this); }
        }

        /// <summary>
        /// �x�N�g���v�f�̓��a�D
        /// </summary>
        public double SumSq
        {
            get { return Vectors.SumSq(this); } // XXX
        }

        /// <summary>
        /// �x�N�g���v�f�̕��ρD
        /// </summary>
        public double Average
        {
            get { return Vectors.Average(this); }
        }

        /// <summary>
        /// �x�N�g���v�f�̎U�z�D
        /// </summary>
        public double Scatter
        {
            get { return Vectors.Scatter(this); } // XXX
        }

        /// <summary>
        /// �x�N�g���v�f�̕W�{���U�D
        /// </summary>
        public double SampleVariance
        {
            get { return Vectors.SampleVariance(this); }
        }

        /// <summary>
        /// �s�Ε��U
        /// </summary>
        public double UnbiasedVariance
        {
            get { return Vectors.UnbiasedVariance(this); }
        }

        /// <summary>
        /// �x�N�g���̊e�v�f�̕����𔽓]����D
        /// </summary>
        /// <returns>���g�ւ̎Q��</returns>
        public Vector Flip()
        {
            return Vectors.Flip(this);
        }

        /// <summary>
        /// �x�N�g���̊e�v�f�� 0 �ɂ���D
        /// </summary>
        /// <returns>���g�ւ̎Q��</returns>
        public Vector Zero()
        {
            return Vectors.Zero(this);
        }

        /// <summary>
        /// �w��T�C�Y�̃[���x�N�g���𐶐�����D
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector Zero(int size)
        {
            return Fill(size, 0);
        }

        /// <summary>
        /// �w��T�C�Y�� <paramref name="value"/> �Ŗ��߂�ꂽ�x�N�g���𐶐�����D
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
        /// �x�N�g���̗v�f��z��Ƃ��Ď擾����D
        /// </summary>
        /// <returns>�v�f�l�̔z��</returns>
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
        /// �񋓎q��Ԃ��D
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

        #region ���Z�̒�`

        /// <summary>
        /// �P�����Z�q
        /// </summary>
        /// <returns>�����̃R�s�[�I�u�W�F�N�g</returns>
        public static Vector operator +(Vector v)
        {
            return new Vector(v);
        }

        /// <summary>
        /// �P�����Z�q
        /// </summary>
        /// <returns>�������R�s�[���āC�����𔽓]�������I�u�W�F�N�g</returns>
        public static Vector operator -(Vector v)
        {
            return new Vector(v).Flip();
        }

        /// <summary>
        /// 2 �� <see cref="Vector"/> �I�u�W�F�N�g�����Z����D
        /// </summary>
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(plus(v1, v2));
        }

        /// <summary>
        /// law �𗘗p�������Z
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
        /// 2��<see cref="Vector"/>�I�u�W�F�N�g�����Z����D
        /// </summary>
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(sub(v1, v2));
        }

        /// <summary>
        /// law �𗘗p�������Z
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
        /// ����
        /// </summary>
        public static double operator *(Vector v1, Vector v2)
        {
            return dot(v1, v2);
        }

        /// <summary>
        /// law �𗘗p��������
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
        /// �X�J���{
        /// </summary>
        public static Vector operator *(double d, Vector v)
        {
            return new Vector(scala(d, v));
        }

        /// <summary>
        /// law �𗘗p�����X�P�[�����O
        /// </summary>
        protected static double[] scala(double d, Vector v)
        {
            VectorChecker.IsNotZeroSize(v);
            double[] ret = null;
            krdlab.law.func.dscal_r(ref ret, v._body, d);
            return ret;
        }

        /// <summary>
        /// �X�J���{
        /// </summary>
        public static Vector operator *(Vector v, double d)
        {
            return d * v;
        }

        /// <summary>
        /// �w��X�J���̋t���{
        /// </summary>
        public static Vector operator /(Vector v, double d)
        {
            return new Vector(scala(1 / d, v));
        }

        #endregion

        #region ������Z���\�b�h (copy-less operator �������̂�)

        /// <summary>
        /// �e�v�f�� <paramref name="value"/> �����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        public Vector Add(double value)
        {
            return Vectors.AddEq(this, value);
        }
        /// <summary>
        /// ���̃x�N�g���I�u�W�F�N�g��<paramref name="v"/>�����Z����D
        /// </summary>
        /// <param name="v">�x�N�g���I�u�W�F�N�g</param>
        /// <returns>���g�ւ̎Q��</returns>
        public Vector Add(Vector v)
        {
            return Vectors.AddEq(this, v);
        }
        /// <summary>
        /// ���̃x�N�g���I�u�W�F�N�g����<paramref name="v"/>�����Z����D
        /// </summary>
        /// <param name="v">�x�N�g���I�u�W�F�N�g</param>
        /// <returns>���g�ւ̎Q��</returns>
        public Vector Sub(Vector v)
        {
            return Vectors.SubEq(this, v);
        }
        /// <summary>
        /// �e�v�f��<paramref name="value"/>����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        public Vector Mul(double value)
        {
            return Vectors.MulEq(this, value);
        }
        /// <summary>
        /// �e�v�f��<paramref name="value"/>�����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        public Vector Div(double value)
        {
            return Vectors.DivEq(this, value);
        }

        #endregion

        /// <summary>
        /// �����C�e���[�^�D
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
        /// map ����D
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
