using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// �d���ʕ��́i�������ʕ���: Canonical Discriminant Analysis�j
    /// </summary>
    public class MultipleDiscriminantAnalysis
    {
        private Vector eigenvalues = null;
        private Vector[] eigenvectors = null;

        /// <summary>
        /// ���ʊ֐��i�W���x�N�g���j�����߂�D
        /// </summary>
        /// <param name="Xs">
        /// <para>�e�S��Matrix�I�u�W�F�N�g�i�e�s�̓f�[�^�ɁC�e��͊e�ϐ��ɑΉ�����j�̔z��</para>
        /// <para>
        /// 1�̌Q�f�[�^�́C1��<see cref="Matrix"/>�I�u�W�F�N�g�ł���킳��Ă���C
        /// �{�����ɂ�菑���������邱�Ƃ͂Ȃ��iread-only�j�D
        /// </para>
        /// </param>
        public MultipleDiscriminantAnalysis(Matrix[] Xs)
        {
            int C = Xs.Length;          // �Q��
            int[] ns = new int[C];      // �e�Q�̃f�[�^��
            int p = Xs[0].ColumnSize;   // ������

            // �e�S�̃f�[�^�����擾
            for (int k = 0; k < C; ++k)
            {
                ns[k] = Xs[k].RowSize;
            }

            Vector tAvg = new Vector(p);    // �S�f�[�^�ɂ�����e�����̕��ϒl
            tAvg.Zero();

            int tN = 0; // �S�f�[�^��

            Vector[] avgs = new Vector[C];  // �e�Q�ɂ�����e�����̕��ϒl
            for (int k = 0; k < C; ++k)
            {
                tN += Xs[k].RowSize;
                avgs[k] = Xs[k].ColumnAverages;

                tAvg.ForEach(delegate(int i, ref double val)
                {
                    val += Xs[k].Columns[i].Sum;
                });
            }
            tAvg /= tN;

            Matrix B = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                IVector dv = avgs[k] - tAvg;
                B += (ns[k] * new ColumnVector(dv) * new RowVector(dv));
            }

            Matrix W = new Matrix(p, p);
            for (int k = 0; k < C; ++k)
            {
                for (int i = 0; i < ns[k]; ++i)
                {
                    IVector dv = Xs[k].Rows[i] - avgs[k];
                    W += (new ColumnVector(dv) * new RowVector(dv));
                }
            }

            EigenvalueDecomposition evd = new EigenvalueDecomposition(Matrix.Inverse(W) * B);
            evd.Sort(EigenvalueDecomposition.SortOrder.Descending);

            this.eigenvalues = evd.RealEigenvalues;
            this.eigenvectors = evd.RealEigenvectors;
        }

        /// <summary>
        /// �ŗL�l���擾����D�~���i�偨���j�Ƀ\�[�g����Ă���D
        /// </summary>
        public Vector Eigenvalues
        {
            get { return this.eigenvalues; }
        }

        /// <summary>
        /// �ŗL�x�N�g���i�W���x�N�g���j���擾����D�ŗL�l�̏����ɑΉ����Ă���D
        /// </summary>
        public Vector[] Eigenvectors
        {
            get { return this.eigenvectors; }
        }

        /// <summary>
        /// ���ʊ֐��̊�^�����擾����D
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
    }
}
