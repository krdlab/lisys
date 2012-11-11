using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace KrdLab.Lisys
{
    /// <summary>
    /// AX = B ������������
    /// </summary>
    public class Solved
    {
        private readonly Matrix x;
        internal Solved(Matrix x) { this.x = x; }

        /// <summary>
        /// AX = B �� X
        /// </summary>
        public Matrix X { get { return this.x; } }
    }

    /// <summary>
    /// ���ْl�����̌���
    /// </summary>
    public class Svd
    {
        private readonly Matrix _u;
        private readonly Matrix _s;
        private readonly Matrix _v;

        internal Svd(Matrix u, Matrix s, Matrix v)
        {
            this._u = u;
            this._s = s;
            this._v = v;
        }

        /// <summary>
        /// �e���ْl�ɑΉ����鍶���كx�N�g�����e��Ɋi�[���ꂽ�s��
        /// </summary>
        public Matrix U
        {
            get { return this._u; }
        }

        /// <summary>
        /// �Ίp�v�f�����ْl�ł���s��
        /// </summary>
        public Matrix S
        {
            get { return this._s; }
        }

        /// <summary>
        /// �e���ْl�ɑΉ�����E���كx�N�g�����e��Ɋi�[���ꂽ�s��
        /// </summary>
        public Matrix V
        {
            get { return this._v; }
        }
    }

    /// <summary>
    /// LU �����̌���
    /// </summary>
    public class Lud
    {
        private readonly Matrix _p;
        private readonly Matrix _l;
        private readonly Matrix _u;
        private readonly int permutationCount;
        private readonly int zeroValueIndex;

        internal Lud(Matrix p, Matrix l, Matrix u, int permCount, int zeroIndex = -1)
        {
            this._p = p;
            this._l = l;
            this._u = u;
            this.permutationCount = permCount;
            this.zeroValueIndex = zeroIndex;
        }

        /// <summary>
        /// �u���s����擾����D
        /// </summary>
        public Matrix P
        {
            get { return this._p; }
        }

        /// <summary>
        /// ���O�p�s����擾����D
        /// </summary>
        public Matrix L
        {
            get { return this._l; }
        }

        /// <summary>
        /// ��O�p�s����擾����D
        /// </summary>
        public Matrix U
        {
            get { return this._u; }
        }

        /// <summary>
        /// ��O�p�s�񂪓��قł������ꍇ�C<c>U[i, i] == 0</c> �ł��� <c>i</c> ���擾����D
        /// ����ȊO�̏ꍇ�́C<c>-1</c> ��Ԃ��D
        /// </summary>
        public int ZeroValueIndexOfU
        {
            get { return this.zeroValueIndex; }
        }

        /// <summary>
        /// LU �����ōs��ꂽ�s�̒u���񐔂��擾����D
        /// </summary>
        public int PermutationCount
        {
            get { return this.permutationCount; }
        }
    }

    /// <summary>
    /// �ŗL�l�����̌���
    /// </summary>
    public class Eigen
    {
        private readonly Vector _reValues;
        private readonly Vector _imValues;
        private readonly List<Vector> _reVectors;
        private readonly List<Vector> _imVectors;

        internal Eigen(Vector reValues, Vector imValues, List<Vector> reVectors, List<Vector> imVectors)
        {
            this._reValues = reValues;
            this._imValues = imValues;
            this._reVectors = reVectors;
            this._imVectors = imVectors;
        }

        #region �v���p�e�B

        /// <summary>
        /// �ŗL�l�̎����������擾����D
        /// </summary>
        public Vector RealEigenvalues
        {
            get { return this._reValues; }
        }

        /// <summary>
        /// �ŗL�l�̋����������擾����D
        /// </summary>
        public Vector ImaginaryEigenvalues
        {
            get { return this._imValues; }
        }

        /// <summary>
        /// �ŗL�x�N�g���̎������擾����D
        /// </summary>
        public List<Vector> RealEigenvectors
        {
            get { return this._reVectors; }
        }

        /// <summary>
        /// �ŗL�x�N�g���̋������擾����D
        /// </summary>
        public List<Vector> ImaginaryEigenvectors
        {
            get { return this._imVectors; }
        }

        #endregion

        /// <summary>
        /// �\�[�g�I�[�_
        /// </summary>
        public enum SortOrder
        {
            /// <summary>
            /// �����i������j
            /// </summary>
            Ascending,

            /// <summary>
            /// �~���i�偨���j
            /// </summary>
            Descending,
        }

        private class SortItem
        {
            public readonly Complex EigenValue;
            public readonly Vector ReVector;
            public readonly Vector ImVector;

            public SortItem(double reVal, double imVal, Vector reVec, Vector imVec)
            {
                this.EigenValue = new Complex(reVal, imVal);
                this.ReVector = reVec;
                this.ImVector = imVec;
            }
        }

        /// <summary>
        /// �ŗL�l����ɁC�ŗL�l�ƌŗL�x�N�g���̃y�A���\�[�g���� (�j��I)�D
        /// </summary>
        /// <param name="order">SortOrder�񋓎q�ŏ����E�~�����w�肷��D</param>
        public void Sort(SortOrder order)
        {
            // �\�[�g�p�̍\���ɋl�߂�
            List<SortItem> items = new List<SortItem>();
            int num = this._reValues.Size;
            for (int i = 0; i < num; ++i)
            {
                items.Add(new SortItem(this._reValues[i], this._imValues[i], this._reVectors[i], this._imVectors[i]));
            }

            var direction = (order == SortOrder.Ascending) ? 1 : -1;
            Func<double, int> filter = v => direction * Math.Sign(v);
            // items �� null �͊܂܂�Ȃ�
            items.Sort((l, r) => filter(l.EigenValue.Magnitude - r.EigenValue.Magnitude));

            // items���l�ߒ���
            for (int i = 0; i < items.Count; ++i)
            {
                var item = items[i];
                this._reValues[i] = item.EigenValue.Real;
                this._imValues[i] = item.EigenValue.Imaginary;
                this._reVectors[i] = item.ReVector;
                this._imVectors[i] = item.ImVector;
            }
        }
    }

    /// <summary>
    /// ���`���ʕ��͂̌���
    /// </summary>
    public class Lda
    {
        private readonly Vector eigenvalues;
        private readonly List<Vector> eigenvectors;
        private readonly List<double> ratios;
        private readonly List<Vector> groupMeans;

        internal Lda(Vector eigenvalues, List<Vector> eigenvectors, List<Vector> groupMeans)
        {
            this.eigenvalues = eigenvalues;
            this.eigenvectors = eigenvectors;
            this.ratios = CalculateRatios(eigenvalues);
            this.groupMeans = groupMeans;
        }

        private List<double> CalculateRatios(Vector eigenvalues)
        {
            var sum = eigenvalues.Sum;
            return eigenvalues.Select(v => v / sum).ToList();
        }

        /// <summary>
        /// �ŗL�l���擾����D�~���Ƀ\�[�g����Ă���D
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// �ŗL�x�N�g���i�W���x�N�g���j���擾����D�ŗL�l�̏����ɑΉ����Ă���D
        /// </summary>
        public List<Vector> Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// ���ʊ֐��̊�^�����擾����D
        /// </summary>
        public List<double> Ratios
        {
            get { return this.ratios; }
        }

        /// <summary>
        /// �Q���ς�Ԃ��D
        /// </summary>
        public List<Vector> GroupMeans
        {
            get { return this.groupMeans; }
        }
    }

    /// <summary>
    /// ���`�d��A���͂̌���
    /// </summary>
    public class Lr
    {
        private readonly Vector coefficients;
        private readonly Vector leverages;
        private readonly Vector residuals;
        private readonly double r2;
        private readonly double rc2;
        private readonly Vector ts;
        private readonly int dofP;  // ��A���f���̎��R�x
        private readonly int dofE;  // �c���̎��R�x

        internal Lr(Vector coefficients, Vector leverages, Vector residuals, double r2, double rc2, Vector ts, int dofP, int dofE)
        {
            this.coefficients = coefficients;
            this.leverages = leverages;
            this.residuals = residuals;
            this.r2 = r2;
            this.rc2 = rc2;
            this.ts = ts;
            this.dofP = dofP;
            this.dofE = dofE;
        }

        /// <summary>
        /// �W���x�N�g�� ([0] �ɂ͒萔���C[1], [2], ... �͊e�ϐ��ɑ΂���W��)�D
        /// </summary>
        public Vector CoefficientVector
        {
            get { return this.coefficients; }
        }

        /// <summary>
        /// ����W��
        /// </summary>
        /// <remarks>
        /// <para>�T���v�����O�f�[�^ <c>y</c> �Ɨ\���l <c>y'</c> �Ƃ̑��� <c>R</c> ���d���֌W���ƌĂԁD</para>
        /// <para>
        /// ����W���Ƃ́C���̏d���֌W���� 2 ��l�̂��Ƃł���D
        /// ���̒l�́C�T���v�����O�f�[�^ <c>y</c> �������U�̂����C�\���l <c>y'</c> �̕��U����߂銄����\���Ă���D
        /// ���̒l���Ⴂ�ꍇ�C�덷�̑傫�� (���܂��t�B�b�g���Ă��Ȃ�) ���f���ł���Ƃ�����D
        /// </para>
        /// </remarks>
        public double R2
        {
            get { return this.r2; }
        }

        /// <summary>
        /// ���R�x�����ς݌���W��
        /// </summary>
        /// <remarks>
        /// �ʏ�C���f���̃p�����[�^���𑝂₷���Ƃ� (���Ƃ��Ӗ��̂Ȃ��p�����[�^�ł������Ƃ��Ă�) ����W���̒l��傫�����邱�Ƃ��ł���D
        /// �������C����͈Ӗ��̂��郂�f�����\�z�����ōD�܂����Ȃ��D
        /// �����ŁC����W���ɑ΂��Ď��R�x�ɂ�钲�����s�������̂��C���̎��R�x�����ς݌���W���ł���D
        /// </remarks>
        public double Rc2
        {
            get { return this.rc2; }
        }

        /// <summary>
        /// ���o���b�W (�e�R��)
        /// <remarks>hat-matrix ((�\���l) = H (�ϑ��l)�C�ɂ�����s�� H) �̑Ίp�v�f</remarks>
        /// </summary>
        public Vector Leverages
        {
            get { return this.leverages; }
        }

        /// <summary>
        /// �c���x�N�g��
        /// </summary>
        public Vector Residuals
        {
            get { return this.residuals; }
        }

        /// <summary>
        /// ���肵���e�W���̐�Βl�ɑ΂��錟�蓝�v�� (t �l)
        /// </summary>
        public Vector TValues
        {
            get { return this.ts; }
        }

        /// <summary>
        /// ��A���f���̎��R�x
        /// </summary>
        /// <remarks><c>phi_p = p</c></remarks>
        public int DofP
        {
            get { return this.dofP; }
        }

        /// <summary>
        /// �c���̎��R�x
        /// </summary>
        /// <remarks><c>phi_e = n - p - 1</c></remarks>
        public int DofE
        {
            get { return this.dofE; }
        }
    }

    /// <summary>
    /// �听�����͂̌���
    /// </summary>
    public class Pca
    {
        private readonly Vector eigenvalues;
        private readonly List<Vector> eigenvectors;
        private readonly double[] ratios;

        internal Pca(Vector eigenvalues, List<Vector> eigenvectors)
        {
            this.eigenvalues = eigenvalues;
            this.eigenvectors = eigenvectors;
            
            double sum = this.eigenvalues.Sum;
            this.ratios = eigenvalues.Select(ev => ev / sum).ToArray();
        }

        /// <summary>
        /// �ŗL�l (�� 1�C�� 2�C... �Ƃ�������)�D
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// �ŗL�x�N�g�� (�ŗL�l�̕��тɑΉ�)�D
        /// </summary>
        public List<Vector> Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// �e�听�����̊�^�� (�� 1�C�� 2�C... �Ƃ�������)�D
        /// </summary>
        public double[] Ratios
        {
            get
            {
                double sum = this.eigenvalues.Sum;
                double[] ratios = new double[this.eigenvalues.Size];

                for (int i = 0; i < ratios.Length; ++i)
                {
                    ratios[i] = this.eigenvalues[i] / sum;
                }
                return ratios;
            }
        }

        /// <summary>
        /// �ŗL�x�N�g������\�������听���W���s��D
        /// </summary>
        /// <remarks>
        /// <para>
        /// ���̃v���p�e�B�� <c>P = XC</c> �ŕ\�����s�� <c>C</c> ��Ԃ� (P: �听�����_�s��CX: �f�[�^�s�� (������ E[X] = 0))�D
        /// </para>
        /// <para>
        /// �ŗL�l�̕��я��i�� 1 �听���C�� 2 �听���C�D�D�D�j�ɑΉ����Ă���D
        /// </para>
        /// </remarks>
        public Matrix CoefficientMatrix
        {
            get
            {
                int row = this.eigenvectors[0].Size;
                int col = this.eigenvalues.Size;
                Matrix ret = new Matrix(row, col);
                for (int c = 0; c < col; ++c)
                {
                    ret.Columns[c] = this.eigenvectors[c];
                }
                return ret;
            }
        }
    }

    /// <summary>
    /// ���v���͂̊e���@ (�֗��֐�����)
    /// </summary>
    public static class Func
    {
        /// <summary>
        /// AX = B �������D
        /// </summary>
        /// <param name="a">[n, n] �s��</param>
        /// <param name="b">[n, *] �s��</param>
        /// <returns></returns>
        public static Solved Solve(Matrix a, Matrix b)
        {
            MatrixChecker.IsSquare(a);
            Matrix _a = new Matrix(a);
            Matrix _b = new Matrix(b);
            Matrix x = new Matrix();
            krdlab.law.func.dgesv(ref x._body, ref x._rsize, ref x._csize,
                                    _a._body, _a._rsize, _a._csize, _b._body, _b._rsize, _b._csize);
            return new Solved(x);
        }

        /// <summary>
        /// ���ْl����
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Svd Svd(Matrix x)
        {
            if (x.RowSize < 2 || x.ColumnSize < 2)
            {
                throw new ArgumentException(
                    "Matrix size is less than 2 (RowSize=" + x.RowSize + ", ColumnSize=" + x.ColumnSize + ").");
            }

            Matrix _x = new Matrix(x);
            double[] u = null;
            int u_row = 0, u_col = 0;
            double[] s = null;
            int s_row = 0, s_col = 0;
            double[] v = null;
            int v_row = 0, v_col = 0;
            krdlab.law.func.dgesvd(_x._body, _x._rsize, _x._csize
                                    , ref u, ref u_row, ref u_col
                                    , ref s, ref s_row, ref s_col
                                    , ref v, ref v_row, ref v_col);
            return new Svd(
                new Matrix(u, u_row, u_col),
                new Matrix(s, s_row, s_col),
                new Matrix(v, v_row, v_col));
        }

        /// <summary>
        /// LU ����
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Lud Lud(Matrix x)
        {
            MatrixChecker.IsNotZeroSize(x);

            Matrix lu = new Matrix(x);

            int[] p = new int[0];
            int zeroValueIndex = -1;

            int ret = krdlab.law.func.dgetrf(lu._body, lu._rsize, lu._csize, ref p);
            if (0 < ret)
            {
                zeroValueIndex = ret - 1;
            }

            // �u���s����\��
            /*
             * �z�� p �ɂ́C�Ⴆ��2�s�ڂ�3�s�ڂ��u�����ꂽ�ꍇ�ɁC
             *     p = {1, 3, 3};
             *            ��1�񂾂����������Ƃ������Ƃ�����
             * �Ɗi�[����Ă���D
             */
            int permutationCount = 0;

            Matrix _p = Matrix.Identity(lu.RowSize);
            // row==column, row < column �̏ꍇ�� p.Length == lu.RowSize
            // row > column              �̏ꍇ�� p.Length <  lu.RowSize

            // �ǂ������������ւ��Ȃ��Ƃ����Ȃ��݂���
            // p[0] = 2
            // p[1] = 1
            // p[2] = 4 <- �u0�Ɠ���ւ������2�ƌ����v�Ȃ̂��u����ւ���O��2�ƌ����v�Ȃ̂��H => �ǂ�����҂̂悤��
            for (int from = p.Length - 1; 0 <= from; --from)
            {
                int to = p[from];
                if (0 <= to && from != to)
                {
                    _p.Rows.Swap(from, to);
                    //this.p[from, from] = this.p[to, to] = 0;
                    //this.p[to, from] = 1;
                    //this.p[from, to] = 1;

                    if (to < p.Length && p[to] == from)
                    {
                        p[to] = -1;  // �u�����������Ă��邱�Ƃ������t���O
                    }

                    // �u�������������Ƃ������J�E���g�A�b�v
                    ++permutationCount;
                }
            }

            int minSize = Math.Min(lu.RowSize, lu.ColumnSize);
            Matrix _l = Matrix.Zero(lu.RowSize, minSize);
            Matrix _u = Matrix.Zero(minSize, lu.ColumnSize);

            // set L
            for (int r = 0; r < lu.RowSize; ++r)
            {
                for (int c = 0; c <= r; ++c)
                {
                    if (_l.ColumnSize <= c)
                    {
                        break;
                    }
                    _l[r, c] = (r == c ? 1.0 : lu[r, c]);
                }
            }

            // set U
            for (int r = 0; r < _u.RowSize; ++r)
            {
                for (int c = r; c < lu.ColumnSize; ++c)
                {
                    _u[r, c] = lu[r, c];
                }
            }
            return new Lud(_p, _l, _u, permutationCount, zeroValueIndex);
        }

        /// <summary>
        /// �ŗL�l����
        /// </summary>
        /// <param name="x">�����s��</param>
        /// <returns></returns>
        public static Eigen Eigen(Matrix x)
        {
            MatrixChecker.IsSquare(x);

            if (x.RowSize < 2)
            {
                throw new ArgumentException("Matrix size is less than 2.");
            }

            Matrix tmp = new Matrix(x);

            Vector _reValues = new Vector();
            Vector _imValues = new Vector();

            double[][] r_vecs = null;
            double[][] i_vecs = null;

            krdlab.law.func.dgeev(tmp._body, tmp._rsize, tmp._csize,
                                    ref _reValues._body, ref _imValues._body, ref r_vecs, ref i_vecs);

            List<Vector> _reVectors = new List<Vector>();
            List<Vector> _imVectors = new List<Vector>();

            foreach (double[] vec in r_vecs)
            {
                _reVectors.Add(new Vector(vec));
            }

            foreach (double[] vec in i_vecs)
            {
                _imVectors.Add(new Vector(vec));
            }
            return new Eigen(_reValues, _imValues, _reVectors, _imVectors);
        }

        /// <summary>
        /// ���`���ʕ���
        /// </summary>
        /// <param name="Xs">
        /// <para>�e�S�� Matrix �I�u�W�F�N�g (�s���f�[�^�C�񂪕ϐ��ɑΉ�����) �̔z��</para>
        /// <para>
        /// 1 �̌Q�f�[�^�� 1 �� Matrix �ŕ\�����D
        /// �{�����ɂ�菑���������邱�Ƃ͂Ȃ� (read-only)�D
        /// </para>
        /// </param>
        public static Lda Lda(Matrix[] Xs)
        {
            int C = Xs.Length;          // �Q��
            int[] ns = new int[C];      // �e�Q�̃f�[�^��
            int p = Xs[0].ColumnSize;   // ������

            // �e�S�̃f�[�^�����擾
            for (int k = 0; k < C; ++k)
            {
                ns[k] = Xs[k].RowSize;
            }

            Vector tAvg = new Vector(new Size(p));    // �S�f�[�^�ɂ�����e�����̕��ϒl
            tAvg.Zero();

            int tN = 0; // �S�f�[�^��

            Vector[] avgs = new Vector[C];  // �e�Q�ɂ�����e�����̕��ϒl
            for (int k = 0; k < C; ++k)
            {
                tN += Xs[k].RowSize;
                avgs[k] = Xs[k].ColumnAverages;
                tAvg.Apply((int i, double val) => val + Xs[k].Columns[i].Sum);
            }
            tAvg /= tN;

            Matrix B = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                IVector dv = avgs[k] - tAvg;
                B += (ns[k] * new ColumnVector(dv) * new RowVector(dv));
            }
            B /= (C - 1);

            Matrix W = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                for (int i = 0; i < ns[k]; ++i)
                {
                    IVector dv = Xs[k].Rows[i] - avgs[k];
                    W += (new ColumnVector(dv) * new RowVector(dv));
                }
            }
            W /= (tN - C);

            Eigen evd = Eigen(Matrix.Inverse(W) * B);
            evd.Sort(KrdLab.Lisys.Eigen.SortOrder.Descending);
            return new Lda(evd.RealEigenvalues, evd.RealEigenvectors, avgs.ToList());
        }

        /// <summary>
        /// ���`�d��A���� (y = X�� + ��)
        /// </summary>
        /// <param name="y">�ϑ��l</param>
        /// <param name="x">���͒l����Ȃ�s��</param>
        public static Lr Lr(IVector y, Matrix x)
        {
            if (y.Size != x.RowSize)
            {
                throw new ArgumentException("sizes of input/output are not equal");
            }

            ColumnVector _y = new ColumnVector(y);
            int p = x.ColumnSize;   // �ϐ��̐�

            // �e�ϐ��̕��ϒl���Z�o
            IVector colAvgs = x.ColumnAverages;
            x.Rows.ForEach((ri, rv) => rv.Apply((int i, double val) => val - colAvgs[i]));

            // ��1��ڂ�"1"��}��
            Matrix tmp = new Matrix(x.RowSize, x.ColumnSize + 1);
            for (int r = 0; r < tmp.RowSize; ++r)
            {
                tmp[r, 0] = 1.0;
                for (int c = 1; c < tmp.ColumnSize; ++c)
                {
                    tmp[r, c] = x[r, c - 1];
                }
            }
            x = tmp;

            // �W���̓���
            Matrix tX = Matrix.Transpose(x);
            Matrix itXX = Matrix.Inverse(tX * x);
            var C = itXX * tX * _y;
            // �萔�������߂�
            for (int i = 1; i < C.Size; ++i)
            {
                C[0] -= C[i] * colAvgs[i - 1];
            }
            var coefficients = C;

            // ���o���b�W
            Matrix H = x * itXX * tX;
            var leverages = new Vector(H.ColumnSize);
            H.Rows.ForEach(delegate(int r, IVector v)
            {
                leverages[r] = v[r];
            });

            // �c���x�N�g��
            ColumnVector e = _y - H * _y;
            var residuals = new Vector(e);

            int phi_t = _y.Size - 1;        // n - 1
            int phi_e = _y.Size - p - 1;    // n - p - 1
            double Se = e * e;

            var dofP = p;
            var dofE = phi_e;

            double y_avg = _y.Average;
            double Syy = 0.0;
            _y.ForEach((int i, double val) =>
                Syy += (val - y_avg) * (val - y_avg));

            // ����W��
            var r2 = 1.0 - Se / Syy;

            // ���R�x�␳�ς݌���W��
            var rc2 = 1.0 - (Se / phi_e) / (Syy / phi_t);

            // �e�W����t�l
            double Ve = Se / phi_e;
            IVector std_coeff = new Vector(coefficients.Size);
            std_coeff.Apply((int i, double val) =>
                Math.Sqrt(Ve * itXX[i, i])); // Sqrt(Ve/Sxx)
            // �܂��C�萔���̕W���΍��͎g���Ȃ��iY�̕��ϒl�̂��̂ɂȂ��Ă���j

            // �萔���̌��蓝�v�ʂ��Z�o
            double var0 = itXX[0, 0];   // 1/n
            for (int i = 1; i < p + 1; ++i)
            {
                for (int j = 1; j < p + 1; ++j)
                {
                    var0 += colAvgs[i - 1] * colAvgs[j - 1] * itXX[i, j];
                }
            }
            std_coeff[0] = Math.Sqrt(var0 * Ve);

            var t = new Vector(std_coeff.Size);
            t.Apply((int i, double val) =>
                val = Math.Abs(coefficients[i]) / std_coeff[i] // �A�������̓�=0
            );
            var ts = t;
            return new Lr(coefficients, leverages, residuals, r2, rc2, ts, dofP, dofE);
        }

        /// <summary>
        /// �听������
        /// </summary>
        /// <param name="data">
        /// [n, d] �̍s��i�f�[�^���Fn�C�������Fd�C�����������邱�Ƃ͂Ȃ��j
        /// </param>
        /// <param name="cor">
        /// ���֍s��ŕ��͂��邩�ǂ��� (�f�t�H���g�� <c>false</c> = ���U�E�����U�s��)
        /// </param>
        public static Pca Pca(Matrix data, bool cor = false)
        {
            Matrix x = new Matrix(data);
            Matrix s;
            if (cor)
            {
                s = Correlate(x, Target.EachColumn);
            }
            else
            {
                // �e�ϐ��̕��ϒl�� 0 �ɂ���
                Vector colAvgs = x.ColumnAverages;
                x.Columns.ForEach((c, vec) => vec.Apply((i, val) => val - colAvgs[c]));
                // ���U�E�����U�s��
                s = Matrix.Transpose(x) * x / (x.RowSize - 1);
            }
            // �ŗL�l����
            Eigen evd = Eigen(s);
            evd.Sort(KrdLab.Lisys.Eigen.SortOrder.Descending);
            // �Ώ̍s��̌ŗL�l�͑S�Ď���
            return new Pca(evd.RealEigenvalues, evd.RealEigenvectors);
        }

        /// <summary>
        /// ���K��
        /// </summary>
        /// <param name="values">���ɂȂ�x�N�g�� (�ύX����Ȃ�)</param>
        /// <param name="average"></param>
        /// <param name="stdev"></param>
        /// <returns>���K����̐V�����C���X�^���X��Ԃ�</returns>
        public static Vector Standardize(IVector values, double average, double stdev)
        {
            return Standardize(values, average, stdev, new Vector(values.Size));
        }

        /// <summary>
        /// ���K��
        /// </summary>
        public static D Standardize<S, D>(S src, double average, double stdev, D dst)
            where S : IRandomAccessible<double>
            where D : IRandomAccessible<double>
        {
            int size = src.Size;
            for (int i = 0; i < size; ++i)
            {
                dst[i] = (src[i] - average) / stdev;
            }
            return dst;
        }

        /// <summary>
        /// �����̑Ώ�
        /// </summary>
        public enum Target
        {
            /// <summary>
            /// �s�P�ʂ�ΏۂƂ���D
            /// </summary>
            EachRow,

            /// <summary>
            /// ��P�ʂ�ΏۂƂ���D
            /// </summary>
            EachColumn,
        }

        /// <summary>
        /// 2�̃x�N�g���̑��ւ����߂�D
        /// </summary>
        /// <param name="vx">�x�N�g��</param>
        /// <param name="vy">�x�N�g��</param>
        /// <returns>����</returns>
        /// <exception cref="System.ArgumentException">
        /// �x�N�g���̃T�C�Y����v���Ȃ��Ƃ���throw�����D
        /// </exception>
        public static double Correlate(IVector vx, IVector vy)
        {
            VectorChecker.SizeEquals(vx, vy);

            double sxy = 0.0;
            double avg_x = vx.Average;
            double avg_y = vy.Average;
            for (int i = 0; i < vx.Size; ++i)
            {
                sxy += ((vx[i] - avg_x) * (vy[i] - avg_y));
            }
            return sxy / Math.Sqrt(vx.Scatter * vy.Scatter);
        }

        /// <summary>
        /// �e�x�N�g���̑��ւ����߂�D
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Matrix Correlate(Matrix X, Matrix Y, Target target)
        {
            MatrixChecker.SizeEquals(X, Y);
            Matrix ret = null;
            if (target == Target.EachRow)
            {
                ret = new Matrix(X.RowSize, X.RowSize);
                for (int ry = 0; ry < ret.RowSize; ++ry)
                {
                    for (int rx = 0; rx < ret.ColumnSize; ++rx)
                    {
                        ret[ry, rx] = Correlate(Y.Rows[ry], X.Rows[rx]);
                    }
                }
            }
            else
            {
                ret = new Matrix(X.ColumnSize, X.ColumnSize);
                for (int cy = 0; cy < ret.RowSize; ++cy)
                {
                    for (int cx = 0; cx < ret.ColumnSize; ++cx)
                    {
                        ret[cy, cx] = Correlate(Y.Columns[cy], X.Columns[cx]);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// �e�x�N�g���̑��ւ����߂�D
        /// </summary>
        /// <param name="X"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Matrix Correlate(Matrix X, Target target)
        {
            return Correlate(X, X, target);
        }

        //// �c�x
        //public static double Skewness()
        //{
        //}

        //// ��x
        //public static double Kurtosis()
        //{
        //}
    }
}
