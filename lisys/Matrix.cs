using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �s��N���X
    /// </summary>
    [Serializable]
    [DebuggerVisualizer(typeof(Visualizer.MatrixVisualizer), Target = typeof(Matrix), Description = "Matrix Visualizer")]
    public partial class Matrix : IEnumerable<double>
    {
        /// <summary>
        /// ���f�[�^�Dlaw �A�g�̂��ߌ�����J�D
        /// </summary>
        internal double[] _body = null;
        internal int _rsize = 0;
        internal int _csize = 0;

        /// <summary>
        /// ��̃I�u�W�F�N�g���쐬����D
        /// </summary>
        internal Matrix()
        {
            Clear();
        }

        /// <summary>
        /// �w�肳�ꂽ�z����R�s�[���āC�V�����s����쐬����D
        /// </summary>
        /// <param name="body">�R�s�[�����z��</param>
        /// <param name="rowSize">�V�����s��</param>
        /// <param name="columnSize">�V������</param>
        internal Matrix(double[] body, int rowSize, int columnSize)
        {
            CopyFrom(body, rowSize, columnSize);
        }

        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�̍s����쐬����D
        /// �e�v�f�� 0 �ɏ����������D
        /// </summary>
        /// <param name="rowSize">�s��</param>
        /// <param name="columnSize">��</param>
        public Matrix(int rowSize, int columnSize)
        {
            Resize(rowSize, columnSize).Fill(0);
        }

        /// <summary>
        /// �R�s�[���ĐV�����s����쐬����D
        /// </summary>
        /// <param name="m">�R�s�[�����s��</param>
        public Matrix(Matrix m)
        {
            CopyFrom(m);
        }

        /// <summary>
        /// �x�N�g���z�񂩂�V�����s����쐬����D
        /// �w�肳�ꂽ�e�x�N�g���́C�V�����s��̊e�s�ɃR�s�[�����D
        /// </summary>
        /// <param name="arr"></param>
        public Matrix(params RowVector[] arr)
        {
            // ���͂̌���
            VectorChecker.IsNotZeroSize(arr[0]);
            int csize = arr[0].Size;
            MatrixChecker.EqualAllSizes(csize, arr);
            // �\�z
            int rsize = arr.Length;
            Resize(rsize, csize);
            for (int r = 0; r < rsize; ++r)
            {
                this.Rows[r] = arr[r];
            }
        }

        /// <summary>
        /// �w�肳�ꂽ�x�N�g������V�����s����쐬����D
        /// �w�肳�ꂽ�e�x�N�g���́C�V�����s��̊e��ɃR�s�[�����D
        /// </summary>
        /// <param name="arr"></param>
        public Matrix(params ColumnVector[] arr)
        {
            // ���͂̌���
            VectorChecker.IsNotZeroSize(arr[0]);
            int rsize = arr[0].Size;
            MatrixChecker.EqualAllSizes(rsize, arr);
            // �\�z
            int csize = arr.Length;
            Resize(rsize, csize);
            for (int c = 0; c < csize; ++c)
            {
                this.Columns[c] = arr[c];
            }
        }

        /// <summary>
        /// 2�����z�񂩂�V�����s����쐬����D
        /// </summary>
        /// <param name="arr">�s��̗v�f���i�[����2�����z��</param>
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
        /// �w�肳�ꂽ 2��<see cref="Matrix"/>�����������ǂ����������D
        /// </summary>
        /// <param name="left"><see cref="Matrix"/></param>
        /// <param name="right"><see cref="Matrix"/></param>
        /// <param name="delta">
        /// �e�v�f�̒l�������ł���Ƃ݂Ȃ���鍷�ق�臒l�i&gt; 0�j
        /// �i<c><see cref="System.Math.Abs(double)"/>(<paramref name="left"/>[r, c] - <paramref name="right"/>[r, c]) &lt; <paramref name="delta"/></c>�ł���Γ����Ƃ݂Ȃ��j
        /// </param>
        /// <returns>�������ꍇ��<c>true</c>���C����ȊO�̏ꍇ��<c>false</c>��Ԃ��D</returns>
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
        /// Matrix�̕�����\�����擾����D
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

        #region �v���p�e�B�̒�`

        /// <summary>
        /// ���̍s��̊e�v�f��ݒ�C�擾����D
        /// </summary>
        /// <param name="row">�sindex�i�͈́F[0, <see cref="RowSize"/>) �j</param>
        /// <param name="col">��index�i�͈́F[0, <see cref="ColumnSize"/>) �j</param>
        /// <returns>�v�f�̒l</returns>
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
        /// ���̃I�u�W�F�N�g�̍s�R���N�V�������擾����D
        /// </summary>
        public IVectorCollection<RowCollection> Rows
        {
            get
            {
                return new RowCollection(this._body, this._rsize, this._csize);
            }
        }

        /// <summary>
        /// ���̃I�u�W�F�N�g�̗�R���N�V�������擾����D
        /// </summary>
        public IVectorCollection<ColumnCollection> Columns
        {
            get
            {
                return new ColumnCollection(this._body, this._rsize, this._csize);
            }
        }

        /// <summary>
        /// ���̃I�u�W�F�N�g�̍s�����擾����D
        /// </summary>
        public Size RowSize
        {
            get { return new Size(this._rsize); }
        }

        /// <summary>
        /// ���̃I�u�W�F�N�g�̗񐔂��擾����D
        /// </summary>
        public Size ColumnSize
        {
            get { return new Size(this._csize); }
        }

        /// <summary>
        /// (�s, ��) �T�C�Y
        /// </summary>
        public Sizes Sizes
        {
            get { return new Sizes(this._rsize, this._csize); }
        }

        /// <summary>
        /// �e�s�̕��ϒl���i�[���ꂽ�x�N�g�����擾����D
        /// </summary>
        public ColumnVector RowAverages
        {
            get { return new ColumnVector(this.Rows.Select(v => v.Average)); }
        }

        /// <summary>
        /// �e��̕��ϒl���i�[���ꂽ�x�N�g�����擾����D
        /// </summary>
        public RowVector ColumnAverages
        {
            get { return new RowVector(this.Columns.Select(v => v.Average)); }
        }

        /// <summary>
        /// �e�s�̕W�{���U���i�[�����x�N�g�����擾����D
        /// </summary>
        public ColumnVector RowSampleVariances
        {
            get { return new ColumnVector(this.Rows.Select(v => v.SampleVariance)); }
        }

        /// <summary>
        /// �e�s�̕s�Ε��U���i�[�����x�N�g�����擾����D
        /// </summary>
        public ColumnVector RowUnbiasedVariances
        {
            get { return new ColumnVector(this.Rows.Select(v => v.UnbiasedVariance)); }
        }

        /// <summary>
        /// �e��̕W�{���U���i�[�����x�N�g�����擾����D
        /// </summary>
        public RowVector ColumnSampleVariances
        {
            get { return new RowVector(this.Columns.Select(v => v.SampleVariance)); }
        }

        /// <summary>
        /// �e��̕s�Ε��U���i�[�����x�N�g�����擾����D
        /// </summary>
        public RowVector ColumnUnbiasedVariances
        {
            get { return new RowVector(this.Columns.Select(v => v.UnbiasedVariance)); }
        }

        /// <summary>
        /// ���̍s��̃g���[�X���擾����D
        /// </summary>
        /// <remarks>
        /// �Ίp�v�f�̘a = �g���[�X = �ŗL�l�̘a
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
        /// ���̍s��̍s�񎮂��擾����D
        /// </summary>
        /// <remarks>
        /// <para>�����ł́C4�~4 �ȏ�̍s��ɑ΂��� LU �����𗘗p���Ă���</para>
        /// <para>�ŗL�l�̐� = �s��</para>
        /// <para>�s�� = 0 �� ���Ȃ��Ƃ� 1 �̌ŗL�l�� 0</para>
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
                    // 4�ȏ�

                    Lud lud = Func.Lud(this);

                    // �u���񐔂��畄�����m�肷��
                    int sign = lud.PermutationCount % 2 == 0 ? +1 : -1;

                    // �s�񎮂��v�Z
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
        /// ���̃I�u�W�F�N�g���N���A����i<c>RowSize == 0 and ColumnSize == 0</c> �ɂȂ�j�D
        /// </summary>
        public void Clear()
        {
            this._body = new double[0];
            this._rsize = 0;
            this._csize = 0;
        }

        /// <summary>
        /// ���T�C�Y����D���T�C�Y��̊e�v�f�l��0�ɂȂ�D
        /// </summary>
        /// <param name="rowSize">�V�����s��</param>
        /// <param name="columnSize">�V������</param>
        /// <returns>���T�C�Y��̎��g�ւ̎Q��</returns>
        public Matrix Resize(int rowSize, int columnSize)
        {
            this._body = new double[rowSize * columnSize];
            this._rsize = rowSize;
            this._csize = columnSize;
            return this;
        }

        /// <summary>
        /// �w��l�Ŗ��߂�D
        /// </summary>
        /// <param name="val"></param>
        /// <returns>�������g</returns>
        public Matrix Fill(double val)
        {
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] = val;
            }
            return this;
        }

        /// <summary>
        /// �w�肳�ꂽ�s����R�s�[����D
        /// </summary>
        /// <param name="m">�R�s�[�����s��</param>
        /// <returns>�R�s�[��̎��g�ւ̎Q��</returns>
        public Matrix CopyFrom(Matrix m)
        {
            return CopyFrom(m._body, m._rsize, m._csize);
        }

        /// <summary>
        /// <para>�w�肳�ꂽ1�����z����C�w�肳�ꂽ�s��`���ŃR�s�[����D</para>
        /// <para>�z��̃T�C�Y�ƁurowSize * columnSize�v�͈�v���Ȃ���΂Ȃ�Ȃ��D</para>
        /// </summary>
        /// <param name="body">�R�s�[�����z��</param>
        /// <param name="rowSize">�s��</param>
        /// <param name="columnSize">��</param>
        /// <returns>�R�s�[��̎��g�ւ̎Q��</returns>
        internal Matrix CopyFrom(double[] body, int rowSize, int columnSize)
        {
            // ���͂̌���
            if (body.Length != rowSize * columnSize)
            {
                throw new ArgumentException("body.Length != rowSize * columnSize");
            }

            // �o�b�t�@�m��
            if (this._rsize == rowSize && this._csize == columnSize)
            {
                // �������Ȃ�
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

            // �R�s�[
            body.CopyTo(this._body, 0);
            return this;
        }

        #region �ϊ��o��

        /// <summary>
        /// �s��̊e�s�� <see cref="RowVector"/> �̗񋓎q�Ƃ��ďo�͂���D
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RowVector> ToRowVectors()
        {
            return this.Rows.Select(v => new RowVector(v));
        }

        /// <summary>
        /// �s��̊e��� <see cref="ColumnVector"/> �̗񋓎q�Ƃ��ďo�͂���D
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnVector> ToColumnVectors()
        {
            return this.Columns.Select(v => new ColumnVector(v));
        }

        /// <summary>
        /// �s��� 2 �����z��Ƃ��ďo�͂���D
        /// </summary>
        /// <returns>2 �����z��i<c>array[r, c] == matrix[r, c]</c>�j</returns>
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
        /// ���̍s����[���s��ɂ���D
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
        /// ���̍s���P�ʍs��iI = diag(1,1,...,1)�j�ɂ���D
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
        /// �S�Ă̗v�f�̕����𔽓]����D
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
        /// ���̍s���]�u����D
        /// </summary>
        /// <returns>�]�u��̎��g�ւ̎Q��</returns>
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
        /// �t�s�񉻂���D
        /// </summary>
        /// <returns>�t�s�񉻌�̎��g�ւ̎Q��</returns>
        /// <exception cref="System.ArgumentException">
        /// �����s��łȂ��Ƃ��� throw �����D
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
        /// �S�Ă̗v�f�Ɏw�肵���l�����Z����D
        /// </summary>
        /// <param name="value">���Z����l</param>
        /// <returns>���g�ւ̎Q��</returns>
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
        /// <see cref="Matrix"/> �I�u�W�F�N�g�𒼐ډ��Z����D
        /// <code>this += m;</code> ��\���Ă��邪�C������Z�q�̂悤�Ɍ��ʃI�u�W�F�N�g����������邱�Ƃ͂Ȃ��D
        /// </summary>
        /// <param name="m">���Z���� <see cref="Matrix"/> �I�u�W�F�N�g</param>
        /// <returns>���Z��̎��g�ւ̎Q��</returns>
        /// <exception cref="System.ArgumentException">
        /// �s��̃T�C�Y����v���Ȃ������ꍇ�� throw �����D
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
        /// <see cref="Matrix"/> �I�u�W�F�N�g�𒼐ڌ��Z����D
        /// <code>this -= m;</code> ��\���Ă��邪�C������Z�q�̂悤�Ɍ��ʃI�u�W�F�N�g����������邱�Ƃ͂Ȃ��D
        /// </summary>
        /// <param name="m">���Z���� <see cref="Matrix"/> �I�u�W�F�N�g</param>
        /// <returns>���Z��̎��g�ւ̎Q��</returns>
        /// <exception cref="System.ArgumentException">
        /// �s��̃T�C�Y����v���Ȃ������ꍇ�� throw �����D
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
        /// ���ڃX�J���{����D
        /// <code>this *= d;</code> ��\���Ă��邪�C������Z�q�̂悤�Ɍ��ʃI�u�W�F�N�g����������邱�Ƃ͂Ȃ��D
        /// </summary>
        /// <param name="d">�X�J���l</param>
        /// <returns>�X�J���{��̎��g�ւ̎Q��</returns>
        public Matrix Mul(double d)
        {
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] *= d;
            }
            return this;
        }

        /// <summary>
        /// ���ڃX�J���l�ŏ��Z����D
        /// <code>this /= d;</code> ��\���Ă��邪�C������Z�q�̂悤�Ɍ��ʃI�u�W�F�N�g����������邱�Ƃ͂Ȃ��D
        /// </summary>
        /// <param name="d">�X�J���l</param>
        /// <returns>���Z��̎��g�ւ̎Q��</returns>
        public Matrix Div(double d)
        {
            for (int i = 0; i < this._body.Length; ++i)
            {
                this._body[i] /= d;
            }
            return this;
        }


        #region ���Z�q�̒�`

        /// <summary>
        /// �s��̃R�s�[�����̂܂ܕԂ����Z�q�D
        /// <c>-<paramref name="m"/></c> �̓���Ƒ΂ɂȂ�悤�ɒ�`���Ă���D
        /// </summary>
        /// <param name="m"></param>
        /// <returns><c>+<paramref name="m"/></c></returns>
        public static Matrix operator +(Matrix m)
        {
            return new Matrix(m);
        }
        /// <summary>
        /// �����𔽓]���������ʂ�Ԃ����Z�q�D
        /// </summary>
        /// <param name="m"></param>
        /// <returns><c>-<paramref name="m"/></c></returns>
        public static Matrix operator -(Matrix m)
        {
            return (new Matrix(m)).Flip();
        }
        /// <summary>
        /// �s��ǂ��������Z����D
        /// </summary>
        /// <param name="m1">�����ƂȂ�s��</param>
        /// <param name="m2">�E���ƂȂ�s��</param>
        /// <returns>���Z���ʂ̍s��</returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return (new Matrix(m1)).Add(m2);
        }
        /// <summary>
        /// �s�񂩂�s������Z����D
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns>���Z���ʂ̍s��</returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return (new Matrix(m1)).Sub(m2);
        }
        /// <summary>
        /// �s��ǂ�������Z����D
        /// </summary>
        /// <param name="m1">�����ƂȂ�s��</param>
        /// <param name="m2">�E���ƂȂ�s��</param>
        /// <returns>��Z���ʂ̍s��</returns>
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
        /// �s����X�J���{����D
        /// </summary>
        /// <param name="m">�s��</param>
        /// <param name="d">�X�J���l</param>
        /// <returns>�X�J���{���ꂽ�s��</returns>
        public static Matrix operator *(Matrix m, double d)
        {
            return (new Matrix(m)).Mul(d);
        }
        /// <summary>
        /// �s����X�J���{����D
        /// </summary>
        /// <param name="d">�X�J���l</param>
        /// <param name="m">�s��</param>
        /// <returns>�X�J���{���ꂽ�s��</returns>
        public static Matrix operator *(double d, Matrix m)
        {
            return m * d;
        }
        /// <summary>
        /// �s����X�J���l�ŏ��Z����D
        /// </summary>
        /// <param name="m">�s��</param>
        /// <param name="d">�X�J���l</param>
        /// <returns>���Z���ʂ̍s��</returns>
        public static Matrix operator /(Matrix m, double d)
        {
            return (new Matrix(m)).Div(d);
        }


        #region �o�͂̌^���ω����鉉�Z�̒�`

        /// <summary>
        /// �s��Ɨ�x�N�g���̏�Z
        /// </summary>
        /// <param name="m">�s��</param>
        /// <param name="cv">��x�N�g��</param>
        /// <returns>��Z���ʂ̗�x�N�g��</returns>
        public static ColumnVector operator *(Matrix m, ColumnVector cv)
        {
            MatrixChecker.CanMultiply(m, cv);
            ColumnVector ret = new ColumnVector();

            krdlab.law.func.dgemv(ref ret._body, m._body, m._rsize, m._csize, cv._body);

            return ret;
        }
        /// <summary>
        /// �s�x�N�g���ƍs��̏�Z
        /// </summary>
        /// <param name="rv">�s�x�N�g��</param>
        /// <param name="m">�s��</param>
        /// <returns>��Z���ʂ̍s�x�N�g��</returns>
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
