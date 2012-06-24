using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys.Method
{
    /// <summary>
    /// 固有値分解
    /// </summary>
    public class EigenvalueDecomposition
    {
        private Vector _reValues;
        private Vector _imValues;
        private Vector[] _reVectors;
        private Vector[] _imVectors;
        private bool hasComplex;

        #region プロパティの定義

        /// <summary>
        /// 複素数の固有値を持つかどうかを示す．
        /// </summary>
        public bool HasComplexValue
        {
            get { return this.hasComplex; }
        }

        /// <summary>
        /// 固有値の実数成分を取得する．
        /// </summary>
        public Vector RealEigenvalues
        {
            get { return this._reValues; }
        }

        /// <summary>
        /// 固有値の虚数成分を取得する．
        /// </summary>
        public Vector ImaginaryEigenvalues
        {
            get { return this._imValues; }
        }

        /// <summary>
        /// 固有ベクトルの実部を取得する．
        /// </summary>
        public Vector[] RealEigenvectors
        {
            get { return this._reVectors; }
        }

        /// <summary>
        /// 固有ベクトルの虚部を取得する．
        /// </summary>
        public Vector[] ImaginaryEigenvectors
        {
            get { return this._imVectors; }
        }

        #endregion


        /// <summary>
        /// 固有値分解
        /// </summary>
        /// <param name="X">固有値分解される行列（書き換えられることはない）</param>
        /// <exception cref="Exception.IllegalArgumentException">
        /// 行列のサイズが2未満の時にthrowされる．
        /// </exception>
        /// <exception cref="Exception.NotSquareMatrixException">
        /// 行列が正方行列でないときにthrowされる．
        /// </exception>
        /// <remarks>
        /// 内部では，<paramref name="X"/>がコピーされてから<see cref="krdlab.law.func.dgeev(double[], int, int, ref double[], ref double[], ref double[][], ref double[][])"/>に渡される．
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
        /// ソートオーダ
        /// </summary>
        public enum SortOrder
        {
            /// <summary>
            /// 昇順（小→大）
            /// </summary>
            Ascending,

            /// <summary>
            /// 降順（大→小）
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
            /// ベクトル長
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
        /// 固有値を基準に，固有値と固有ベクトルのペアをソートする．
        /// </summary>
        /// <param name="order">SortOrder列挙子で昇順・降順を指定する．</param>
        public void Sort(SortOrder order)
        {
            // ソート用の構造に詰める
            List<SortItem> items = new List<SortItem>();
            int num = this._reValues.Size;
            for (int i = 0; i < num; ++i)
            {
                items.Add(new SortItem(this._reValues[i], this._imValues[i], this._reVectors[i], this._imVectors[i]));
            }

            // ソーティング
            if (order == SortOrder.Ascending)
            {
                items.Sort(delegate(SortItem left, SortItem right)
                {
                    if (left == null)
                    {
                        if (right == null)
                        {
                            return 0;   // 等しい
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
                            // left, rightがともにnullでないとき，固有値を比較する
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

            // itemsを詰め直す
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
