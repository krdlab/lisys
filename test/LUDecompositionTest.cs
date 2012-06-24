using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class LUDecompositionTest
    {
        const double Ep = 1e-15;

        [Test]
        public void TestRotation()
        {
            const double angle = Math.PI / 6;
            Matrix X = new Matrix(new[,] { { Math.Cos(angle), -Math.Sin(angle), 0 },
                                           { Math.Sin(angle),  Math.Cos(angle), 0 },
                                           {               0,                0, 1 }});
            Assert.That(X.Determinant, Is.EqualTo(1.0).Within(Ep));

            Lud lud = Func.Lud(X);
            Assert.IsTrue(IsLowerTriangular(lud.L, Ep));
            Assert.IsTrue(IsUpperTriangular(lud.U, Ep));
            Assert.IsTrue(Matrix.Equals(X, lud.L * lud.U, Ep));
            Assert.IsTrue(Matrix.Equals(X, lud.P * lud.L * lud.U, Ep));
        }

        [Test]
        public void TestRotation2()
        {
            const double angle = Math.PI / 6;
            Matrix X = new Matrix(new[,] { { Math.Cos(angle), -Math.Sin(angle), 0 },
                                           {               0,                0, 1 },    // çsÇÃì¸ÇÍë÷Ç¶
                                           { Math.Sin(angle),  Math.Cos(angle), 0 }});
            Assert.That(X.Determinant, Is.EqualTo(-1.0).Within(Ep));

            Lud lud = Func.Lud(X);
            Assert.IsTrue(IsLowerTriangular(lud.L, Ep));
            Assert.IsTrue(IsUpperTriangular(lud.U, Ep));
            Assert.IsFalse(Matrix.Equals(X, lud.L * lud.U, Ep));
            Assert.IsTrue(Matrix.Equals(X, lud.P * lud.L * lud.U, Ep));
        }

        [Test]
        public void TestZero()
        {
            Matrix X = new Matrix(3, 3).Zero();
            Lud lud = Func.Lud(X);
            Assert.That(lud.ZeroValueIndexOfU, Is.EqualTo(0));
        }

        [Test]
        public void TestSingular()
        {
            Matrix X = new Matrix(new[,] { { 1.0, 2, 3 },
                                           { 2,   2, 2 },
                                           { 0,   0, 0 } });
            Lud lud = Func.Lud(X);
            Assert.That(lud.ZeroValueIndexOfU, Is.EqualTo(2));  // X.Rows[2]Ç™égÇ¶Ç»Ç¢
        }

        [Test]
        public void TestNotSquare()
        {
            Matrix X = new Matrix(new[,] { { 1.0, 2, 5 },
                                           { 2,   5, 7 } });
            Lud lud = Func.Lud(X);
            Assert.IsTrue(IsLowerTriangular(lud.L, Ep));
            Assert.IsTrue(IsUpperTriangular(lud.U, Ep));
            Assert.IsTrue(Matrix.Equals(X, lud.P * lud.L * lud.U, Ep));
        }

        [Test]
        public void TestNotSquare2()
        {
            Matrix X = new Matrix(
                            new RowVector(1, 2, 5),
                            new RowVector(2, 4, 7),
                            new RowVector(5, 5, 8),
                            new RowVector(3, 4, 7),
                            new RowVector(3, 5, 4));
            Lud lud = Func.Lud(X);
            Assert.IsTrue(IsLowerTriangular(lud.L, Ep));
            Assert.IsTrue(IsUpperTriangular(lud.U, Ep));
            Assert.IsTrue(Matrix.Equals(X, lud.P * lud.L * lud.U, Ep));
        }

        #region Helper

        private bool IsUpperTriangular(Matrix X, double ep)
        {
            for (int r = 0; r < X.RowSize; ++r)
            {
                for (int c = 0; c < r; ++c)
                {
                    if (ep < Math.Abs(X[r, c]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsLowerTriangular(Matrix X, double ep)
        {
            for (int r = 0; r < X.RowSize; ++r)
            {
                for (int c = r + 1; c < X.ColumnSize; ++c)
                {
                    if (ep < Math.Abs(X[r, c]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
