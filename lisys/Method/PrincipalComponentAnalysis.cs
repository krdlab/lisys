using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// �听������
    /// </summary>
    public class PrincipalComponentAnalysis
    {
        private Vector eigenvalues = null;
        private Vector[] eigenvectors = null;

        /// <summary>
        /// �s�� X �Ɏ听�����͂�K�p����D
        /// </summary>
        /// <param name="X">
        /// <para>[n, d]�̍s��i�����������邱�Ƃ͂Ȃ��j</para>
        /// <para>�f�[�^���Fn�C�������Fd�Ƃ����Ƃ��C�s�� X �̌`���� n�~d �łȂ���΂Ȃ�Ȃ��D</para>
        /// </param>
        /// <param name="varType">���U�̎��</param>
        public PrincipalComponentAnalysis(Matrix X, VarianceType varType)
        {
            Matrix _X = new Matrix(X);

            // �e�ϐ��̕��ϒl��0�ɂ���
            Vector colAvg = _X.ColumnAverages;
            _X.Columns.ForEach(delegate(int i, IVector v)
            {
                v.ForEach(delegate(ref double val)
                {
                    val -= colAvg[i];
                });
            });

            // ���U�E�����U�s��
            Matrix S = Matrix.Transpose(_X) * _X;
            if (varType == VarianceType.DivN)
            {
                S /= _X.RowSize;
            }
            else
            {
                S /= (_X.RowSize - 1);
            }

            // �ŗL�l����
            EigenvalueDecomposition evd = new EigenvalueDecomposition(S);
            evd.Sort(EigenvalueDecomposition.SortOrder.Descending);

            this.eigenvalues = evd.RealEigenvalues;
            this.eigenvectors = evd.RealEigenvectors;
        }

        /// <summary>
        /// �ŗL�l���擾����D
        /// ��1�C��2�C�c�Ƃ��������ŕ���ł���D
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// �ŗL�x�N�g�����擾����D
        /// �ŗL�l�̕��тɑΉ����Ă���D
        /// </summary>
        public Vector[] Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// �e�听�����̊�^�����擾����D
        /// ��1�C��2�C�c�Ƃ��������ŕ���ł���D
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
        /// �ŗL�x�N�g������\�������s����擾����i�听�����_�W���s��Ƃ��Ă΂��j�D
        /// </summary>
        /// <remarks>
        /// <para>
        /// ���̃v���p�e�B����擾�ł���s��́C
        /// �听�����_�s���P�CPCA�̑ΏۂƂȂ����s���X�C�听�����_�W���s���C�Ƃ����Ƃ��C
        /// </para>
        /// <para><c>P = XC</c></para>
        /// <para>
        /// �̊֌W�ƂȂ�s��<c>C</c>�ł���D
        /// ���я��́C�ŗL�l�̕��я��i��1�听���C��2�听���C�D�D�D�j�ɑΉ����Ă���D
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
}
