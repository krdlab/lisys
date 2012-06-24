using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �x�N�g���N���X
    /// </summary>
    [Serializable]
    public class Vector : IResizableVector, ICsv, IEquatable<Vector>
    {
        internal double[] _body = null;

        #region �R���X�g���N�V����

        /// <summary>
        /// ��̃I�u�W�F�N�g���쐬����D
        /// </summary>
        internal Vector()
        {
            Clear();
        }

        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�̃x�N�g�����쐬����D
        /// �e�v�f��0�ɏ����������D
        /// </summary>
        /// <param name="size">�x�N�g���̃T�C�Y�i�v�f���j</param>
        public Vector(int size)
        {
            Resize(size, 0.0);
        }

        /// <summary>
        /// �C���^�t�F�[�X��ʂ��āC�w�肳�ꂽ�I�u�W�F�N�g�̃R�s�[���쐬����D
        /// </summary>
        /// <param name="v">�R�s�[�����I�u�W�F�N�g</param>
        public Vector(IVector v)
        {
            CopyFrom(v);
        }

        /// <summary>
        /// �C�ӂ̌��̗v�f�𒼐ڎw�肵�ăx�N�g�����쐬����D
        /// </summary>
        /// <param name="arr">�C�ӂ̌��̃x�N�g���̗v�f</param>
        public Vector(params double[] arr)
        {
            CopyFrom(arr);
        }

        #endregion

        #region IEquatable<Vector> �����o

        /// <summary>
        /// �w�肳�ꂽ<see cref="Vector"/>���C���g�Ɠ��������ǂ����������D
        /// </summary>
        /// <param name="other"><see cref="Vector"/></param>
        /// <returns>�������ꍇ�� <c>true</c> ���C����ȊO�� <c>false</c> ��Ԃ��D</returns>
        public bool Equals(Vector other)
        {
            return Vector.Equals(this, other);
        }

        #endregion

        #region Object���\�b�h

        /// <summary>
        /// �w�肳�ꂽ<see cref="object"/>���C�����Ɠ��������ǂ����������D
        /// </summary>
        /// <param name="obj">��r�Ώ�</param>
        /// <returns>�������ꍇ�� <c>true</c> ���C����ȊO�� <c>false</c> ��Ԃ��D</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj, -1);
        }

        /// <summary>
        /// �w�肳�ꂽ<see cref="object"/>���C�����Ɠ��������ǂ����������D
        /// </summary>
        /// <param name="obj">��r�Ώ�</param>
        /// <param name="delta">臒l�i�ڍׂ� <see cref="Vector.Equals(Vector, Vector, double)"/>�j</param>
        /// <returns>�������ꍇ�� <c>true</c> ���C����ȊO�� <c>false</c> ��Ԃ��D</returns>
        public bool Equals(object obj, double delta)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            // obj is Vector �ł��邱�Ƃ��ۏ؂����
            return Vector.Equals(this, (Vector)obj, delta);
        }

        /// <summary>
        /// �w�肳�ꂽ 2��<see cref="Vector"/> �����������ǂ����������D
        /// </summary>
        /// <param name="left"><see cref="Vector"/></param>
        /// <param name="right"><see cref="Vector"/></param>
        /// <returns>���������<c>true</c>���C����ȊO�̏ꍇ��<c>false</c>��Ԃ��D</returns>
        public static bool Equals(Vector left, Vector right)
        {
            return Equals(left, right, -1);
        }

        /// <summary>
        /// �w�肳�ꂽ 2��<see cref="Vector"/> �����������ǂ����������D
        /// </summary>
        /// <param name="left"><see cref="Vector"/></param>
        /// <param name="right"><see cref="Vector"/></param>
        /// <param name="delta">
        /// �e�v�f�̒l�������ł���Ƃ݂Ȃ���鍷�ق�臒l�i&gt; 0�j
        /// �i<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[i] - <paramref name="right"/>[i]) &lt; <paramref name="delta"/></c>�ł���Γ����Ƃ݂Ȃ��j
        /// </param>
        /// <returns>���������<c>true</c>���C����ȊO�̏ꍇ��<c>false</c>��Ԃ��D</returns>
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
        /// ���̃I�u�W�F�N�g�̃n�b�V���l��Ԃ��D
        /// </summary>
        /// <returns>�n�b�V���l</returns>
        public override int GetHashCode()
        {
            return this.Size ^ (~((int)this[0]));
        }

        /// <summary>
        /// �x�N�g���̕�����\�����擾����D
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return VectorImpl.ToString(this);
        }

        #endregion

        #region �ŗL�̎���

        /// <summary>
        /// �w�肳�ꂽ�x�N�g�����R�s�[����D
        /// </summary>
        /// <param name="v">�R�s�[�����x�N�g��</param>
        /// <returns>���g�ւ̎Q��</returns>
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
        /// �w�肳�ꂽ�z����R�s�[����D
        /// </summary>
        /// <param name="arr">�R�s�[�����z��</param>
        /// <returns>���g�ւ̎Q��</returns>
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
        /// �x�N�g���̗v�f���N���A����i<c>Size == 0</c> �ɂȂ�j�D
        /// </summary>
        public void Clear()
        {
            this._body = new double[0];
        }


        #region IResizableVector �����o

        /// <summary>
        /// �x�N�g���̃T�C�Y���w�肳�ꂽ�T�C�Y�ɕύX����D
        /// </summary>
        /// <param name="size">�V�����T�C�Y</param>
        /// <returns>���T�C�Y��̎��g�ւ̎Q��</returns>
        public IVector Resize(int size)
        {
            this._body = new double[size];
            return this;
        }

        #endregion


        /// <summary>
        /// �x�N�g���̃T�C�Y���w�肳�ꂽ�T�C�Y�ɕύX���C�w�肳�ꂽ�l�Ŗ��߂�D
        /// </summary>
        /// <param name="size">�V�����T�C�Y</param>
        /// <param name="val">�v�f�Ɏw�肷��l</param>
        /// <returns>���T�C�Y��̎��g�ւ̎Q��</returns>
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
        /// �v�findex�̔z��Ŏw�肳�ꂽ�v�f�𒊏o����D
        /// </summary>
        /// <param name="indexes">���o�������v�f��<c>index</c>�z��</param>
        /// <returns>���o�����v�f�ɃA�N�Z�X�\��<see cref="IVector"/>�C���^�t�F�[�X</returns>
        /// <exception cref="Exception.IllegalArgumentException">
        /// �������s���̏ꍇ��throw�����D
        /// </exception>
        /// <exception cref="Exception.ZeroSizeException">
        /// �x�N�g���T�C�Y��0�̏ꍇ��throw�����D
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

        #region IVector �����o

        /// <summary>
        /// �w�肳�ꂽindex�̗v�f��ݒ�E�擾����D
        /// </summary>
        /// <param name="i">�v�f��index</param>
        /// <returns>index�Ŏw�肳�ꂽ�v�f�l</returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// <c><paramref name="i"/> &lt; 0 or <see cref="Size"/> &lt;= <paramref name="i"/></c> �̏ꍇ��throw�����D
        /// </exception>
        public double this[int i]
        {
            set { this._body[i] = value; }
            get { return this._body[i]; }
        }

        /// <summary>
        /// �x�N�g���̃T�C�Y���擾����D
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
        /// �x�N�g���̃m�������擾����D
        /// </summary>
        public double Norm
        {
            get { return VectorImpl.Norm(this); }
        }

        /// <summary>
        /// �x�N�g���̗v�f�̘a���擾����D
        /// </summary>
        public double Sum
        {
            get { return VectorImpl.Sum(this); }
        }

        /// <summary>
        /// �x�N�g���̊e�v�f��2�悵�C�����̘a���擾����D
        /// </summary>
        public double SumSq
        {
            get { return VectorImpl.SumSq(this); }
        }

        /// <summary>
        /// �x�N�g���̗v�f�̕��ς��擾����D
        /// </summary>
        public double Average
        {
            get { return VectorImpl.Average(this); }
        }

        /// <summary>
        /// �x�N�g���̗v�f�̎U�z���擾����D
        /// </summary>
        public double Scatter
        {
            get { return VectorImpl.Scatter(this); }
        }

        /// <summary>
        /// �x�N�g���̗v�f�̕W�{���U���擾����D
        /// </summary>
        public double Variance
        {
            get { return VectorImpl.SampleVariance(this); }
        }

        /// <summary>
        /// �x�N�g���̊e�v�f�̕����𔽓]����D
        /// </summary>
        /// <returns>���g�ւ̎Q��</returns>
        public IVector Flip()
        {
            return VectorImpl.Flip(this);
        }

        /// <summary>
        /// �x�N�g���̊e�v�f��0�ɂ���D
        /// </summary>
        /// <returns>���g�ւ̎Q��</returns>
        public IVector Zero()
        {
            return VectorImpl.Zero(this);
        }

        /// <summary>
        /// �x�N�g���̗v�f��z��Ƃ��Ď擾����D
        /// </summary>
        /// <returns>�v�f�l�̔z��</returns>
        public double[] ToArray()
        {
            return VectorImpl.ToArray(this);
        }

        /// <summary>
        /// �x�N�g���̊e�v�f�ɑ΂���action��K�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByVal"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        public IVector ForEach(ElementActionByVal action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// �x�N�g���̊e�v�f�ɑ΂���action��K�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRef"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        public IVector ForEach(ElementActionByRef action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// �x�N�g���̊e�v�f�ɑ΂���action��K�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByValWithIndex"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        public IVector ForEach(ElementActionByValWithIndex action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// �x�N�g���̊e�v�f�ɑ΂���action��K�p����D
        /// </summary>
        /// <param name="action">
        /// <see cref="ElementActionByRefWithIndex"/>�ɂ��K�肳�ꂽ�f���Q�[�g
        /// </param>
        /// <returns>���\�b�h�K�p��̎��g�ւ̎Q��</returns>
        public IVector ForEach(ElementActionByRefWithIndex action)
        {
            return VectorImpl.ForEach(this, action);
        }

        /// <summary>
        /// �����x�N�g����Ԃ��D
        /// </summary>
        /// <param name="startIndex">�����x�N�g���̊J�n�ʒu</param>
        /// <returns><paramref name="startIndex"/>����I�[�܂ł̕����x�N�g��</returns>
        /// <remarks>
        /// ���̃x�N�g�����ێ�����v�f�̕����W���ł��镔���x�N�g����Ԃ��D
        /// �����x�N�g�����ێ�����v�f�́C���̃x�N�g���̗v�f�Ɠ������̂ł���D
        /// </remarks>
        public IVector Subvector(int startIndex)
        {
            return VectorImpl.Subvector(this, startIndex);
        }

        /// <summary>
        /// �����x�N�g����Ԃ��D
        /// </summary>
        /// <param name="startIndex">�����x�N�g���̊J�n�ʒu</param>
        /// <param name="length">�J�n�ʒu����̒���</param>
        /// <returns>
        /// <paramref name="startIndex"/>����C����<paramref name="length"/>�̕����x�N�g����Ԃ��D
        /// <paramref name="length"/>��<see cref="IVector.Size"/>�𒴂���ꍇ�́C
        /// �I�[�܂Łi[<paramref name="startIndex"/>, <see cref="IVector.Size"/>)�j�̕����x�N�g����Ԃ��D
        /// </returns>
        /// <remarks>
        /// ���̃x�N�g�����ێ�����v�f�̕����W���ł��镔���x�N�g����Ԃ��D
        /// �����x�N�g�����ێ�����v�f�́C���̃x�N�g���̗v�f�Ɠ������̂ł���D
        /// </remarks>
        public IVector Subvector(int startIndex, int length)
        {
            return VectorImpl.Subvector(this, startIndex, length);
        }

        #endregion

        #region ICsv �����o

        /// <summary>
        /// CSV�`���̕�����ŏo�͂���D
        /// </summary>
        /// <returns>CSV�`���̕�����</returns>
        public string ToCsv()
        {
            return VectorImpl.ToCsv(this);
        }

        #endregion

        #region IEnumerable �����o

        IEnumerator IEnumerable.GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion

        #region IEnumerable<double> �����o

        /// <summary>
        /// �񋓎q���擾����D
        /// </summary>
        /// <returns>�񋓎q</returns>
        public IEnumerator<double> GetEnumerator()
        {
            return VectorImpl.Enumerator(this);
        }

        #endregion

        #region ���Z�̒�`

        /// <summary>
        /// �P�����Z�q
        /// </summary>
        /// <param name="v">���Z�q�K�p�Ώ�</param>
        /// <returns>�����̃R�s�[�I�u�W�F�N�g</returns>
        public static Vector operator +(Vector v)
        {
            return new Vector(v);
        }

        /// <summary>
        /// �P�����Z�q
        /// </summary>
        /// <param name="v">���Z�q�K�p�Ώ�</param>
        /// <returns>�������R�s�[���āC�����𔽓]�������I�u�W�F�N�g</returns>
        public static Vector operator -(Vector v)
        {
            return (Vector)new Vector(v).Flip();
        }

        /// <summary>
        /// 2��<see cref="Vector"/>�I�u�W�F�N�g�����Z����D
        /// </summary>
        /// <param name="v1">���Z���Z�q�̍���</param>
        /// <param name="v2">���Z���Z�q�̉E��</param>
        /// <returns>���Z�̌��ʂ�\��<see cref="Vector"/>�I�u�W�F�N�g</returns>
        /// <exception cref="System.Exception">
        /// �����ƉE���̃x�N�g���T�C�Y����v���Ȃ��ꍇ��throw�����D
        /// </exception>
        public static Vector operator +(Vector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="Vector"/>�I�u�W�F�N�g�ƁC<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�Ƃ����Z����D
        /// </summary>
        /// <param name="v1">���Z���Z�q�̍����i<see cref="Vector"/>�I�u�W�F�N�g�j</param>
        /// <param name="v2">���Z���Z�q�̉E���i<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�j</param>
        /// <returns>���Z�̌��ʂ�\��<see cref="Vector"/>�I�u�W�F�N�g</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// �����ƉE���̂̃x�N�g���T�C�Y����v���Ȃ��ꍇ��throw�����D
        /// </exception>
        public static Vector operator +(Vector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�ƁC<see cref="Vector"/>�I�u�W�F�N�g�Ƃ����Z����D
        /// </summary>
        /// <param name="v1">���Z���Z�q�̍����i<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�j</param>
        /// <param name="v2">���Z���Z�q�̉E���i<see cref="Vector"/>�I�u�W�F�N�g�j</param>
        /// <returns>���Z�̌��ʂ�\��<see cref="Vector"/>�I�u�W�F�N�g</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// �����ƉE���̂̃x�N�g���T�C�Y����v���Ȃ��ꍇ��throw�����D
        /// </exception>
        public static Vector operator +(IVector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 2��<see cref="Vector"/>�I�u�W�F�N�g�����Z����D
        /// </summary>
        /// <param name="v1">���Z���Z�q�̍����i�������鑤�j</param>
        /// <param name="v2">���Z���Z�q�̉E���i�����鑤�j</param>
        /// <returns>���Z�̌��ʂ�\��<see cref="Vector"/>�I�u�W�F�N�g</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// �����ƉE���̂̃x�N�g���T�C�Y����v���Ȃ��ꍇ��throw�����D
        /// </exception>
        public static Vector operator -(Vector v1, Vector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="Vector"/>�I�u�W�F�N�g�ƁC<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�Ƃ����Z����D
        /// </summary>
        /// <param name="v1">���Z���Z�q�̍����i�������鑤�F<see cref="Vector"/>�I�u�W�F�N�g�j</param>
        /// <param name="v2">���Z���Z�q�̉E���i�����鑤�F<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�j</param>
        /// <returns>���Z�̌��ʂ�\��<see cref="Vector"/>�I�u�W�F�N�g</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// �����ƉE���̂̃x�N�g���T�C�Y����v���Ȃ��ꍇ��throw�����D
        /// </exception>
        public static Vector operator -(Vector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�ƁC<see cref="Vector"/>�I�u�W�F�N�g�Ƃ����Z����D
        /// </summary>
        /// <param name="v1">���Z���Z�q�̍����i�������鑤�F<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�j</param>
        /// <param name="v2">���Z���Z�q�̉E���i�����鑤�F<see cref="Vector"/>�I�u�W�F�N�g�j</param>
        /// <returns>���Z�̌��ʂ�\��<see cref="Vector"/>�I�u�W�F�N�g</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// �����ƉE���̂̃x�N�g���T�C�Y����v���Ȃ��ꍇ��throw�����D
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

        #region ������Z���\�b�h

        /// <summary>
        /// �e�v�f��<paramref name="value"/>�����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        public IVector Add(double value)
        {
            return VectorImpl.AddEq(this, value);
        }
        /// <summary>
        /// ���̃x�N�g���I�u�W�F�N�g��<paramref name="v"/>�����Z����D
        /// </summary>
        /// <param name="v">�x�N�g���I�u�W�F�N�g</param>
        /// <returns>���g�ւ̎Q��</returns>
        public IVector Add(IVector v)
        {
            return VectorImpl.AddEq(this, v);
        }
        /// <summary>
        /// ���̃x�N�g���I�u�W�F�N�g����<paramref name="v"/>�����Z����D
        /// </summary>
        /// <param name="v">�x�N�g���I�u�W�F�N�g</param>
        /// <returns>���g�ւ̎Q��</returns>
        public IVector Sub(IVector v)
        {
            return VectorImpl.SubEq(this, v);
        }
        /// <summary>
        /// �e�v�f��<paramref name="value"/>����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        public IVector Mul(double value)
        {
            return VectorImpl.MulEq(this, value);
        }
        /// <summary>
        /// �e�v�f��<paramref name="value"/>�����Z����D
        /// </summary>
        /// <param name="value">�X�J���l</param>
        /// <returns>���g�ւ̎Q��</returns>
        public IVector Div(double value)
        {
            return VectorImpl.DivEq(this, value);
        }

        #endregion

        #region IVector�ɑ΂��鉉�Z���\�b�h

        /// <summary>
        /// 2��<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�����Z����
        /// <see cref="Vector"/>�I�u�W�F�N�g���쐬����D
        /// </summary>
        /// <param name="v1"><see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�i�����j</param>
        /// <param name="v2"><see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�i�E���j</param>
        /// <returns>���Z���ʂ��i�[����<see cref="Vector"/>�I�u�W�F�N�g</returns>
        public static Vector Add(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Add(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// 2��<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�Ɍ��Z��K�p���C
        /// <see cref="Vector"/>�I�u�W�F�N�g���쐬����D
        /// </summary>
        /// <param name="v1"><see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�i�����j</param>
        /// <param name="v2"><see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�i�E���j</param>
        /// <returns>���Z���ʂ��i�[����<see cref="Vector"/>�I�u�W�F�N�g</returns>
        public static Vector Sub(IVector v1, IVector v2)
        {
            VectorChecker.MismatchSize(v1, v2);
            return VectorImpl.Sub(new Vector(v1.Size), v1, v2);
        }

        /// <summary>
        /// <see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g���X�J���{���C
        /// <see cref="Vector"/>�I�u�W�F�N�g���쐬����D
        /// </summary>
        /// <param name="v"><see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g</param>
        /// <param name="d">�X�J���l</param>
        /// <returns>�X�J���{�������ʂ��i�[����<see cref="Vector"/>�I�u�W�F�N�g</returns>
        public static Vector Mul(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Mul(new Vector(v.Size), d, v);
        }

        /// <summary>
        /// <see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g���X�J���l�ŏ��Z���C
        /// <see cref="Vector"/>�I�u�W�F�N�g���쐬����D
        /// </summary>
        /// <param name="v"><see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g</param>
        /// <param name="d">���Z�l�i�X�J���j</param>
        /// <returns>1/d ���悶�����ʂ��i�[����<see cref="Vector"/>�I�u�W�F�N�g</returns>
        public static Vector Div(IVector v, double d)
        {
            VectorChecker.ZeroSize(v);
            return VectorImpl.Div(new Vector(v.Size), v, d);
        }

        #endregion


        /// <summary>
        /// �w�肳�ꂽ������<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g�ɂ����āC
        /// ����炪�����v�f�l�������Ă��邩�ǂ����𔻒肷��D
        /// </summary>
        /// <param name="vectors">�ό�<see cref="IVector"/>�C���^�t�F�[�X�����������I�u�W�F�N�g</param>
        /// <returns>
        /// �S�ẴI�u�W�F�N�g�ɂ����āC�S�Ă̗v�f�l����v�����<c>true</c>���C����ȊO��<c>false</c>��Ԃ��D
        /// </returns>
        public static bool HaveSameValues(params IVector[] vectors)
        {
            if (vectors == null)
            {
                return false;   // �l�������Ȃ�����
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
