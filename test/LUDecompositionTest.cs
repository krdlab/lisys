using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;
using KrdLab.Lisys.Method;

namespace LisysTest
{
    [TestFixture]
    public class LUDecompositionTest
    {
        [Test]
        public void Test01()
        {
            const double angle = Math.PI / 6;
            Matrix X = new Matrix(new RowVector(Math.Cos(angle), -Math.Sin(angle), 0),
                                    new RowVector(Math.Sin(angle), Math.Cos(angle), 0),
                                    new RowVector(0, 0, 1));

            LUDecomposition lud = new LUDecomposition(X);

            Assert.IsTrue(IsLowerTriangular(lud.L));
            Assert.IsTrue(IsUpperTriangular(lud.U));

            Assert.AreEqual(X, lud.P * lud.L * lud.U);

            Assert.AreEqual(1.0, X.Determinant, LisysConfig.CalculationLowerLimit);
        }

        [Test]
        public void Test02()
        {
            const double angle = Math.PI / 6;
            Matrix X = new Matrix(new RowVector(Math.Cos(angle), -Math.Sin(angle), 0),
                                    new RowVector(0, 0, 1),                             // 行を入れ替えた
                                    new RowVector(Math.Sin(angle), Math.Cos(angle), 0));

            LUDecomposition lud = new LUDecomposition(X);

            Assert.IsTrue(IsLowerTriangular(lud.L));
            Assert.IsTrue(IsUpperTriangular(lud.U));

            Assert.AreNotEqual(X, lud.L * lud.U);

            Assert.AreEqual(X, lud.P * lud.L * lud.U);

            Assert.AreEqual(-1.0, X.Determinant, LisysConfig.CalculationLowerLimit);
        }

        [Test]
        public void Test03()
        {
            Matrix X = new Matrix(3, 3).Zero();
            LUDecomposition lud = new LUDecomposition(X);

            Assert.AreEqual(0, lud.ZeroValueIndexOfU);   // いきなり 0 要素
        }

        [Test]
        public void Test04()
        {
            Matrix X = new Matrix(new double[,] { { 1, 2, 3 }, { 2, 2, 2 }, { 0, 0, 0 } });
            LUDecomposition lud = new LUDecomposition(X);

            Assert.AreEqual(2, lud.ZeroValueIndexOfU);   // X.Rows[2]が使えない
        }

        [Test]
        public void Test05()
        {
            Matrix X = new Matrix(
                            new RowVector(1, 2, 5),
                            new RowVector(2, 5, 7));

            LUDecomposition lud = new LUDecomposition(X);

            Assert.IsTrue(IsLowerTriangular(lud.L));
            Assert.IsTrue(IsUpperTriangular(lud.U));

            Assert.AreEqual(X, lud.P * lud.L * lud.U);
        }

        [Test]
        public void Test06()
        {
            Matrix X = new Matrix(
                            new RowVector(1, 2, 5),
                            new RowVector(2, 4, 7),
                            new RowVector(5, 5, 8),
                            new RowVector(3, 4, 7),
                            new RowVector(3, 5, 4));
            
            LUDecomposition lud = new LUDecomposition(X);

            Assert.IsTrue(IsLowerTriangular(lud.L));
            Assert.IsTrue(IsUpperTriangular(lud.U));

            Assert.AreEqual(X, lud.P * lud.L * lud.U);
        }

        //public void Test02()
        //{
        //    Matrix X = new Matrix(  new RowVector(1, 1, 1),
        //                            new RowVector(3, 1, 3),
        //                            new RowVector(1, 2, -5),
        //                            new RowVector(1, 4, 3));

        //    LUDecomposition lud = new LUDecomposition(X);

        //    Assert.IsTrue(IsLowerTriangular(lud.L));
        //    Assert.IsTrue(IsUpperTriangular(lud.U));

        //    Assert.AreEqual(X, lud.P * lud.L * lud.U);// 非正方行列に対する，Pの構築に問題あり
        //}


        private bool IsUpperTriangular(Matrix X)
        {
            for (int r = 0; r < X.RowSize; ++r)
            {
                for (int c = 0; c < r; ++c)
                {
                    if (LisysConfig.CalculationLowerLimit <= Math.Abs(X[r, c]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsLowerTriangular(Matrix X)
        {
            for (int r = 0; r < X.RowSize; ++r)
            {
                for (int c = r + 1; c < X.ColumnSize; ++c)
                {
                    if (LisysConfig.CalculationLowerLimit <= Math.Abs(X[r, c]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
