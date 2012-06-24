using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// �d��A����
    /// </summary>
    public class MultipleLinearRegressionAnalysis
    {
        private IVector coefficients = null;
        private IVector leverages = null;
        private IVector residuals = null;
        private double r2 = 0;
        private double rc2 = 0;
        private IVector ts = null;
        private int dofP = -1;  // ��A���f���̎��R�x
        private int dofE = -1;  // �c���̎��R�x


        /// <summary>
        /// �W���x�N�g�����擾����D
        /// </summary>
        /// <remarks>
        /// <para>���C�W���x�N�g����<c>c</c>�Ɋi�[����Ă���Ƃ���D</para>
        /// <para>���̂Ƃ��C</para>
        /// <para><c>c[0]</c>�ɂ́C�萔��</para>
        /// <para><c>c[1], c[2], ...</c>�ɂ́C�e�ϐ��ɑ΂���W��</para>
        /// ���C���ꂼ��i�[����Ă���D
        /// </remarks>
        public IVector CoefficientVector
        {
            get { return this.coefficients; }
        }

        /// <summary>
        /// ����W�����擾����D
        /// </summary>
        /// <remarks>
        /// <para>�T���v�����O�f�[�^<c>y</c>�Ɨ\���l<c>y'</c>�Ƃ̑���<c>R</c>���C�d���֌W���ƌĂԁD</para>
        /// <para>
        /// ����W���Ƃ́C���̏d���֌W����2��l�̂��Ƃł���D
        /// ���̒l�́C�T���v�����O�f�[�^<c>y</c>�������U�̂����C�\���l<c>y'</c>�̕��U����߂銄����\���Ă���D
        /// ���̒l���Ⴂ�ꍇ�C�덷�̑傫�ȁi���܂��t�B�b�g���Ă��Ȃ��j���f���ł���Ƃ�����D
        /// </para>
        /// </remarks>
        public double R2
        {
            get { return this.r2; }
        }

        /// <summary>
        /// ���R�x�����ς݌���W�����擾����D
        /// </summary>
        /// <remarks>
        /// �ʏ�C���f���̃p�����[�^���𑝂₷���ƂŁi���Ƃ��Ӗ��̂Ȃ��p�����[�^�ł������Ƃ��Ă��j
        /// ����W���̒l��傫�����邱�Ƃ��ł���D
        /// �������C����͈Ӗ��̂��郂�f�����\�z�����ōD�܂����Ȃ��D
        /// �����ŁC����W���ɑ΂��Ď��R�x�ɂ�钲�����s�������̂��C���̎��R�x�����ς݌���W���ł���D
        /// </remarks>
        public double Rc2
        {
            get { return this.rc2; }
        }

        /// <summary>
        /// �e�R����擾����D
        /// </summary>
        public IVector LeverageVector
        {
            get { return this.leverages; }
        }

        /// <summary>
        /// �c���x�N�g�����擾����D
        /// </summary>
        public IVector ResidualVector
        {
            get { return this.residuals; }
        }

        /// <summary>
        /// ���肵���e�W���̐�Βl�ɑ΂��錟�蓝�v�ʁit�l�j���擾����D
        /// </summary>
        public IVector TValueVector
        {
            get { return this.ts; }
        }

        /// <summary>
        /// ��A���f���̎��R�x���擾����D
        /// </summary>
        /// <remarks><c>phi_p = p</c></remarks>
        public int DofP
        {
            get { return this.dofP; }
        }

        /// <summary>
        /// �c���̎��R�x���擾����D
        /// </summary>
        /// <remarks><c>phi_e = n - p - 1</c></remarks>
        public int DofE
        {
            get { return this.dofE; }
        }

        /// <summary>
        /// �d��A���͂�K�p����D
        /// </summary>
        /// <param name="y">�o�͒l�̃x�N�g��</param>
        /// <param name="xs">���̓x�N�g���̃Z�b�g</param>
        public MultipleLinearRegressionAnalysis(IVector y, IVector[] xs)
        {
            if (y.Size != xs.Length)
            {
                throw new Exception.MismatchSizeException();
            }

            ColumnVector Y = new ColumnVector(y);
            Matrix X = new Matrix(VectorType.RowVector, xs);
            int p = X.ColumnSize;   // �ϐ��̐�

            // �e�ϐ��̕��ϒl���Z�o
            IVector colAvgs = X.ColumnAverages;
            X.Rows.ForEach(delegate(IVector rv)
            {
                rv.ForEach(delegate(int i, ref double val)
                {
                    val -= colAvgs[i];
                });
            });

            // ��1��ڂ�"1"��}��
            Matrix tmp = new Matrix(X.RowSize, X.ColumnSize + 1);
            for (int r = 0; r < tmp.RowSize; ++r)
            {
                tmp[r, 0] = 1.0;
                for (int c = 1; c < tmp.ColumnSize; ++c)
                {
                    tmp[r, c] = X[r, c - 1];
                }
            }
            X = tmp;

            // �W���̓���
            Matrix tX = Matrix.Transpose(X);
            Matrix itXX = Matrix.Inverse(tX * X);
            IVector C = itXX * tX * Y;

            // �萔�������߂�
            for (int i = 1; i < C.Size; ++i)
            {
                C[0] -= C[i] * colAvgs[i - 1];
            }

            this.coefficients = C;

            // �e�R��
            Matrix H = X * itXX * tX;
            this.leverages = new Vector(H.ColumnSize);
            H.Rows.ForEach(delegate(int r, IVector v)
            {
                this.leverages[r] = v[r];
            });

            // �c���x�N�g��
            ColumnVector e = Y - H * Y;
            this.residuals = new Vector(e);

            int phi_t = Y.Size - 1;        // n - 1
            int phi_e = Y.Size - p - 1;    // n - p - 1
            double Se = e * e;

            this.dofP = p;
            this.dofE = phi_e;

            double y_avg = Y.Average;
            double Syy = 0.0;
            Y.ForEach(delegate(double val)
            {
                Syy += (val - y_avg) * (val - y_avg);
            });

            // ����W��
            this.r2 = 1.0 - Se / Syy;

            // ���R�x�␳�ς݌���W��
            this.rc2 = 1.0 - (Se / phi_e) / (Syy / phi_t);

            // �e�W����t�l
            double Ve = Se / phi_e;
            IVector std_coeff = new Vector(this.coefficients.Size);
            std_coeff.ForEach(delegate(int i, ref double val)
            {
                val = Math.Sqrt(Ve * itXX[i, i]);
            }); // Sqrt(Ve/Sxx)
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

            IVector t = new Vector(std_coeff.Size);
            t.ForEach(delegate(int i, ref double val)
            {
                val = Math.Abs(this.coefficients[i]) / std_coeff[i]; // �A�������̓�=0
            });
            this.ts = t;
        }
    }
}
