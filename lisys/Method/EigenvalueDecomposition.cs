using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// �ŗL�l����
    /// </summary>
    public class EigenvalueDecomposition
    {
        private Vector _reValues;
        private Vector _imValues;
        private Vector[] _reVectors;
        private Vector[] _imVectors;
        private bool hasComplex;

        #region �v���p�e�B�̒�`

        /// <summary>
        /// ���f���̌ŗL�l�������ǂ����������D
        /// </summary>
        public bool HasComplexValue
        {
            get { return this.hasComplex; }
        }

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
        public Vector[] RealEigenvectors
        {
            get { return this._reVectors; }
        }

        /// <summary>
        /// �ŗL�x�N�g���̋������擾����D
        /// </summary>
        public Vector[] ImaginaryEigenvectors
        {
            get { return this._imVectors; }
        }

        #endregion


        /// <summary>
        /// �ŗL�l����
        /// </summary>
        /// <param name="X">�ŗL�l���������s��i�����������邱�Ƃ͂Ȃ��j</param>
        /// <exception cref="Exception.IllegalArgumentException">
        /// �s��̃T�C�Y��2�����̎���throw�����D
        /// </exception>
        /// <exception cref="Exception.NotSquareMatrixException">
        /// �s�񂪐����s��łȂ��Ƃ���throw�����D
        /// </exception>
        /// <remarks>
        /// �����ł́C<paramref name="X"/>���R�s�[����Ă���<see cref="krdlab.law.func.dgeev(double[], int, int, ref double[], ref double[], ref double[][], ref double[][])"/>�ɓn�����D
        /// </remarks>
        public EigenvalueDecomposition(Matrix X)
        {
            MatrixChecker.IsSquare(X);

            if (X.RowSize < 2)
            {
                throw new Exception.IllegalArgumentException("Matrix size is less than 2.");
            }

            Matrix tmp = new Matrix(X);

            this._reValues = new Vector();
            this._imValues = new Vector();

            double[][] r_vecs = null;
            double[][] i_vecs = null;

            krdlab.law.func.dgeev(tmp._body, tmp._rsize, tmp._csize,
                                    ref this._reValues._body, ref this._imValues._body, ref r_vecs, ref i_vecs);

            List<Vector> reVectors = new List<Vector>();
            List<Vector> imVectors = new List<Vector>();

            foreach (double[] vec in r_vecs)
            {
                reVectors.Add(new Vector(vec));
            }

            foreach (double[] vec in i_vecs)
            {
                imVectors.Add(new Vector(vec));
            }

            this._reVectors = reVectors.ToArray();
            this._imVectors = imVectors.ToArray();

            // has complex eigenvalue
            this.hasComplex = false;
            foreach (double elem in this._imValues._body)
            {
                if (!krdlab.law.CalculationChecker.IsLessThanLimit(elem))
                {
                    this.hasComplex = true;
                    break;
                }
            }
        }

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
            public double ReValue;
            public double ImValue;
            public Vector ReVector;
            public Vector ImVector;

            /// <summary>
            /// �x�N�g����
            /// </summary>
            public double Length
            {
                get { return Math.Sqrt(this.ReValue * this.ReValue + this.ImValue * this.ImValue); }
            }

            public SortItem(double reVal, double imVal, Vector reVec, Vector imVec)
            {
                this.ReValue = reVal;
                this.ImValue = imVal;
                this.ReVector = reVec;
                this.ImVector = imVec;
            }
        }

        /// <summary>
        /// �ŗL�l����ɁC�ŗL�l�ƌŗL�x�N�g���̃y�A���\�[�g����D
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

            // �\�[�e�B���O
            if (order == SortOrder.Ascending)
            {
                items.Sort(delegate(SortItem left, SortItem right)
                {
                    if (left == null)
                    {
                        if (right == null)
                        {
                            return 0;   // ������
                        }
                        else
                        {
                            return -1;  // left < right
                        }
                    }
                    else
                    {
                        if (right == null)
                        {
                            return 1;   // left > right
                        }
                        else
                        {
                            // left, right���Ƃ���null�łȂ��Ƃ��C�ŗL�l���r����
                            if (left.Length > right.Length)
                            {
                                return 1;
                            }
                            else if (left.Length < right.Length)
                            {
                                return -1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                });
            }
            else
            {
                items.Sort(delegate(SortItem left, SortItem right)
                {
                    if (left == null)
                    {
                        if (right == null)
                        {
                            return 0;   // equal
                        }
                        else
                        {
                            return 1;   // right is greater
                        }
                    }
                    else
                    {
                        if (right == null)
                        {
                            return -1;  // left is greater
                        }
                        else
                        {
                            // both are not null, compare the eigenvalue
                            if (left.Length > right.Length)
                            {
                                return -1;
                            }
                            else if (left.Length < right.Length)
                            {
                                return 1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                });
            }

            // items���l�ߒ���
            for (int i = 0; i < items.Count; ++i)
            {
                SortItem item = items[i];
                this._reValues[i] = item.ReValue;
                this._imValues[i] = item.ImValue;
                this._reVectors[i] = item.ReVector;
                this._imVectors[i] = item.ImVector;
            }
        }
    }
}
