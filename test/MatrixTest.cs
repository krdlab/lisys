using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class MatrixTest
    {
        [Test]
        public void Test01()
        {
            Matrix m = new ColumnVector(1, 2, 3) * new RowVector(1, 2, 3);  // 行列生成

            double d;
            int n;
            Vector v;

            v = m.ColumnAverages;   // 各列の平均値
            Assert.IsTrue(Vector.HaveSameValues(new Vector(2, 4, 6), v));

            v = m.ColumnVariances;  // 各列の標本分散
            Assert.IsTrue(Vector.HaveSameValues(new Vector(1, 4, 9), v));

            n = m.ColumnSize;       // 列数

            v = m.RowAverages;      // 各行の平均値
            Assert.IsTrue(Vector.HaveSameValues(new Vector(2, 4, 6), v));

            v = m.RowVariances;     // 各行の標本分散
            Assert.IsTrue(Vector.HaveSameValues(new Vector(1, 4, 9), v));

            n = m.RowSize;          // 列数

            d = m.Trace;            // 行列のトレース
            Assert.AreEqual(14, d, LisysConfig.CalculationLowerLimit);

            d = m.Determinant;      // 行列式
            Assert.AreEqual(0, d, LisysConfig.CalculationLowerLimit);

            m = new Matrix(
                new RowVector(0, 1, -1),
                new RowVector(-1, 1, 0),
                new RowVector(2, 1, 0));

            // 逆行列
            Matrix im;
            im = Matrix.Inverse(m);
            Assert.AreEqual(Matrix.Identity(3), m * im);// Matrix.Identity(3) は，「3×3の単位行列」になる

            im = new Matrix(m);
            im.Inverse();
            Assert.AreEqual(Matrix.Identity(3), m * im);// Matrix.Identity(3) は，「3×3の単位行列」になる

            // 転置
            Matrix tm = Matrix.Transpose(m);
            Assert.AreEqual(new Matrix(
                                new ColumnVector(0, 1, -1), new ColumnVector(-1, 1, 0), new ColumnVector(2, 1, 0)), tm);

            tm = new Matrix(m);
            tm.Transpose();
            Assert.AreEqual(new Matrix(
                                new ColumnVector(0, 1, -1), new ColumnVector(-1, 1, 0), new ColumnVector(2, 1, 0)), tm);

            // 標準化
            m[1, 1] = 2;
            Matrix ms = Function.Standardize(m, Target.EachColumn, VarianceType.DivN);
            foreach (IVector cv in ms.Columns)
            {
                Assert.AreEqual(0, cv.Average, LisysConfig.CalculationLowerLimit);
                Assert.AreEqual(1, cv.Scatter / cv.Size, LisysConfig.CalculationLowerLimit);
            }

            // 各種行列生成法
            m = new Matrix(new ColumnVector(1, 1, 1), new ColumnVector(2, 2, 2), new ColumnVector(3, 3, 3));
            Assert.AreEqual(new Matrix(new double[,] { { 1, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }), m);

            m = new Matrix(new RowVector(1, 1, 1), new RowVector(2, 2, 2), new RowVector(3, 3, 3));
            Assert.AreEqual(new Matrix(new double[,] { { 1, 1, 1 }, { 2, 2, 2 }, { 3, 3, 3 } }), m);

            m = new Matrix(new double[,] { { 1, 1, 1 }, { 2, 2, 2 }, { 3, 3, 3 }, { 4, 4, 4 } });
        }

        [Test]
        public void Test02()
        {
            Matrix m = new Matrix(
                            new RowVector(0, 1, -1),
                            new RowVector(-1, 1, 0),
                            new RowVector(2, 1, 0));

            IVector row0 = m.Rows[0];   // 1行目を取得
            Assert.IsTrue(Vector.HaveSameValues(new Vector(0, 1, -1), row0));

            IVectorCollection vc = m.Rows[0, 2];    // 1行目と3行目のセットを取得
            vc[1][2] = 999;                         // コレクションにおいて，index==1 のベクトル（元の行列でいう3行目）の3番目の要素値を変更
            Assert.AreEqual(999, m[2, 2], LisysConfig.CalculationLowerLimit);

            vc[1][2] = -1;

            Matrix u1 = new Matrix(3, 3).Add(1);    // unit行列を作成
            Assert.AreEqual(new Matrix(new double[,] {
                                { 1, 1, 1 },
                                { 1, 1, 1 },
                                { 1, 1, 1 }
            }), u1);

            m = new Matrix(new double[,]{
                            {1, 2, 3},
                            {4, 5, 6},
                            {7, 8, 9}
            });

            Matrix m1 = m.Rows[0, 2].Swap(0, 1).ToMatrix();
            Assert.AreEqual(new Matrix(new double[,]{
                            {7, 8, 9},
                            {1, 2, 3}
            }), m1);
        }

        [Test]
        public void Test03()
        {
            Matrix X;
            X = new Matrix(new double[,] {
                { 2, 1 },
                { 5, 6 }
            });
            Assert.AreEqual(7, X.Determinant, LisysConfig.CalculationLowerLimit);

            X = new Matrix(new double[,] {
                { 12, 1 },
                { 37, 6 }
            });
            Assert.AreEqual(35, X.Determinant, LisysConfig.CalculationLowerLimit);

            X = new Matrix(new double[,] {
                { 2, 12 },
                { 5, 37 }
            });
            Assert.AreEqual(14, X.Determinant, LisysConfig.CalculationLowerLimit);

            X = new Matrix(new double[,] {
                { 2, 1, 1 },
                { 4, 6, 3 },
                { 8, 8, 9 }
            });
            Assert.AreEqual(32, X.Determinant, LisysConfig.CalculationLowerLimit);

            X = new Matrix(new double[,] {
                { 15, 1, 1 },
                { 41, 6, 3 },
                { 83, 8, 9 }
            });
            Assert.AreEqual(160, X.Determinant, LisysConfig.CalculationLowerLimit);

            X = new Matrix(new double[,] {
                { 2, 1, 15 },
                { 4, 6, 41 },
                { 8, 8, 83 },
            });
            Assert.AreEqual(96, X.Determinant, LisysConfig.CalculationLowerLimit);

            X = new Matrix(new double[,] {
                { 2, 1, 15 },
                { 8, 8, 83 },
                { 4, 6, 41 },
            });
            Assert.AreEqual(-96, X.Determinant, LisysConfig.CalculationLowerLimit);
        }

        [Test]
        public void Test04()
        {
            Matrix A;
            A = new Matrix(new double[,]{
                { 5,  3 },
                { 2, -1 },
                { 4,  2 },
            });

            ColumnVector cv = new ColumnVector(-1, 1);
            ColumnVector Yc = A * cv;
            Assert.AreEqual(new ColumnVector(-2, -3, -2), Yc);

            RowVector rv = new RowVector(-2, 0, 1);
            RowVector Yr = rv * A;
            Assert.AreEqual(new RowVector(-6, -4), Yr);
        }
    }
}
