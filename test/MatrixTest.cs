using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class MatrixTest
    {
        const double Ep = 1e-15;

        [Test]
        public void Test_Sizes()
        {
            Matrix sized = new Matrix(3, 5);
            Assert.That(sized.RowSize, Is.EqualTo(new Size(3)));
            Assert.That(sized.ColumnSize, Is.EqualTo(new Size(5)));
            Assert.That(sized.Sizes, Is.EqualTo(new Sizes(3, 5)));
        }

        [Test]
        public void Test_Static()
        {
            {// 単位行列
                Matrix identity = Matrix.Identity(3);
                Assert.True(Matrix.Equals(
                    identity,
                    new Matrix(new[,] { { 1.0, 0, 0 },
                                        { 0, 1.0, 0 },
                                        { 0, 0, 1.0 } }),
                    Ep));
            }
            {// 対角行列
                Matrix diag = Matrix.Diagonal(1, 2, 3);
                Assert.True(Matrix.Equals(
                    diag,
                    new Matrix(new[,] { { 1.0, 0, 0 },
                                        { 0, 2.0, 0 },
                                        { 0, 0, 3.0 } }),
                    Ep));
                Assert.That(
                    diag.ToArray(),
                    Is.EqualTo(new[,] { { 1.0, 0, 0 },
                                        { 0, 2.0, 0 },
                                        { 0, 0, 3.0 } }));
            }
            {// 対角行列 (from Vector)
                Matrix diag = Matrix.Diagonal(new Vector(3, 2, 1));
                Assert.True(Matrix.Equals(
                    diag,
                    new Matrix(new[,] { { 3.0, 0, 0 },
                                        { 0, 2.0, 0 },
                                        { 0, 0, 1.0 } }),
                    Ep));
            }
        }

        [Test]
        public void Test_Properties()
        {
            Matrix m = new ColumnVector(1, 2, 3) * new RowVector(1, 2, 3);
            /*
             * 1 2 3
             * 2 4 6
             * 3 6 9
             */

            Assert.That(m.ColumnSize, Is.EqualTo(new Size(3)));
            Assert.That(m.ColumnAverages, Is.EqualTo(new Vector(2, 4, 6)).Within(Ep));
            Assert.That(m.ColumnUnbiasedVariances, Is.EqualTo(new Vector(1, 4, 9)).Within(Ep));

            Assert.That(m.RowSize, Is.EqualTo(new Size(3)));
            Assert.That(m.RowAverages, Is.EqualTo(new Vector(2, 4, 6)).Within(Ep));
            Assert.That(m.RowUnbiasedVariances, Is.EqualTo(new Vector(1, 4, 9)).Within(Ep));

            Assert.That(m.Trace, Is.EqualTo(14).Within(Ep));
            Assert.That(m.Determinant, Is.EqualTo(0).Within(Ep));
        }

        [Test]
        public void Test_Methods()
        {
            Matrix m = new Matrix(
                new RowVector(0, 1, -1),
                new RowVector(-1, 1, 0),
                new RowVector(2, 1, 0));

            // 逆行列
            Matrix im;
            im = Matrix.Inverse(m);
            Assert.That(m * im, Is.EqualTo(Matrix.I(3)).Within(Ep));

            im = new Matrix(m);
            im.Inverse();
            Assert.That(m * im, Is.EqualTo(Matrix.I(3)).Within(Ep));

            // 転置
            Matrix tm = Matrix.Transpose(m);
            Assert.That(tm, Is.EqualTo(new Matrix(
                                new ColumnVector(0, 1, -1),
                                new ColumnVector(-1, 1, 0),
                                new ColumnVector(2, 1, 0))).Within(Ep));

            tm = new Matrix(m);
            tm.Transpose();
            Assert.That(tm, Is.EqualTo(new Matrix(
                                new ColumnVector(0, 1, -1),
                                new ColumnVector(-1, 1, 0),
                                new ColumnVector(2, 1, 0))).Within(Ep));
        }

        [Test]
        public void Test_Create()
        {
            Matrix m;

            // 各種行列生成法
            m = new Matrix(new ColumnVector(1, 1, 1), new ColumnVector(2, 2, 2), new ColumnVector(3, 3, 3));
            Assert.That(m, Is.EqualTo(new Matrix(new[,] { { 1.0, 2, 3 },
                                                          { 1  , 2, 3 },
                                                          { 1  , 2, 3 } })).Within(Ep));

            m = new Matrix(new RowVector(1, 1, 1),
                           new RowVector(2, 2, 2),
                           new RowVector(3, 3, 3));
            Assert.That(m, Is.EqualTo(new Matrix(new[,] { { 1.0, 1, 1 },
                                                          { 2  , 2, 2 },
                                                          { 3  , 3, 3 } })).Within(Ep));

            m = new Matrix(new[,] { { 1.0, 1, 1 },
                                    { 2,   2, 2 },
                                    { 3,   3, 3 },
                                    { 4,   4, 4 } });
            Assert.That(m, Is.EqualTo(new[] { 1.0, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4 }).Within(Ep));
        }

        [Test]
        public void Test_VectorCollection()
        {
            Matrix m = new Matrix(
                            new RowVector(0, 1, -1),
                            new RowVector(-1, 1, 0),
                            new RowVector(2, 1, 0));

            IVector row0 = m.Rows[0];   // 1行目を取得
            Assert.IsTrue(Vectors.Equals(new Vector(0, 1, -1), row0, Ep));

            var vc = m.Rows[0, 2];      // 1行目と3行目のセットを取得
            vc[1][2] = 999;             // コレクションにおいて，index==1 のベクトル（元の行列でいう3行目）の3番目の要素値を変更
            Assert.That(m[2, 2], Is.EqualTo(999));
            vc[1][2] = -1;
            Assert.That(m[2, 2], Is.EqualTo(-1));

            m = new Matrix(new double[,]{
                            {1, 2, 3},
                            {4, 5, 6},
                            {7, 8, 9}
            });
            Matrix m1 = m.Rows[0, 2].Swap(0, 1).ToMatrix();
            Assert.That(m1, Is.EqualTo(new Matrix(new[,] {
                                                    { 7.0, 8, 9 },
                                                    { 1  , 2, 3 }
            })));
        }

        [Test]
        public void Test_DirectOperator()
        {
            Matrix u1 = new Matrix(3, 3).Add(1);
            Assert.AreEqual(new Matrix(new double[,] {
                                { 1, 1, 1 },
                                { 1, 1, 1 },
                                { 1, 1, 1 }
            }), u1);
        }

        [Test]
        public void Test_Normalize()
        {
            Matrix m = new Matrix(
                new RowVector(0, 1, -1),
                new RowVector(-1, 1, 0),
                new RowVector(2, 1, 0));

            // 正規化
            m[1, 1] = 2;
            Matrix ms = new Matrix(m);
            ms.Columns.ForEach((ci, cv) =>
            {
                double avg = cv.Average;
                double std = Math.Sqrt(cv.UnbiasedVariance);
                cv.Apply((i, val) => (val - avg) / std);
            });
            foreach (IVector cv in ms.Columns)
            {
                Assert.That(cv.Average, Is.EqualTo(0).Within(Ep));
                Assert.That(cv.UnbiasedVariance, Is.EqualTo(1).Within(Ep));
            }
        }

        [Test]
        public void Test_Determinant()
        {
            Matrix X;
            X = new Matrix(new double[,] {
                { 2, 1 },
                { 5, 6 }
            });
            Assert.AreEqual(7, X.Determinant, Ep);

            X = new Matrix(new double[,] {
                { 12, 1 },
                { 37, 6 }
            });
            Assert.AreEqual(35, X.Determinant, Ep);

            X = new Matrix(new double[,] {
                { 2, 12 },
                { 5, 37 }
            });
            Assert.AreEqual(14, X.Determinant, Ep);

            X = new Matrix(new double[,] {
                { 2, 1, 1 },
                { 4, 6, 3 },
                { 8, 8, 9 }
            });
            Assert.AreEqual(32, X.Determinant, Ep);

            X = new Matrix(new double[,] {
                { 15, 1, 1 },
                { 41, 6, 3 },
                { 83, 8, 9 }
            });
            Assert.AreEqual(160, X.Determinant, Ep);

            X = new Matrix(new double[,] {
                { 2, 1, 15 },
                { 4, 6, 41 },
                { 8, 8, 83 },
            });
            Assert.AreEqual(96, X.Determinant, Ep);

            X = new Matrix(new double[,] {
                { 2, 1, 15 },
                { 8, 8, 83 },
                { 4, 6, 41 },
            });
            Assert.AreEqual(-96, X.Determinant, Ep);
        }

        [Test]
        public void Test_VectorOperator()
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
