using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;
using KrdLab.Lisys.Method;

namespace LisysTest
{
    [TestFixture]
    public class SingularValueDecompositionTest
    {
        [Test]
        public void Test01()
        {
            Matrix m = new Matrix(
                            new RowVector(0, 1, -1),
                            new RowVector(-1, 1, 0),
                            new RowVector(2, 1, 0));

            SingularValueDecomposition svd = new SingularValueDecomposition(m);
            Assert.AreEqual(m, svd.U * svd.S * Matrix.Transpose(svd.V));
        }

        [Test]
        public void Test02()
        {
            double prev = LisysConfig.CalculationLowerLimit;
            LisysConfig.CalculationLowerLimit = 1e-13;

            Matrix m = new Matrix(
                            new RowVector(1, 2, 5),
                            new RowVector(2, 4, 7),
                            new RowVector(5, 5, 8),
                            new RowVector(3, 4, 7),
                            new RowVector(3, 5, 4));

            SingularValueDecomposition svd = new SingularValueDecomposition(m);
            Assert.AreEqual(m, svd.U * svd.S * Matrix.Transpose(svd.V));

            LisysConfig.CalculationLowerLimit = prev;
        }

        [Test]
        public void Test03()
        {
            Matrix m = new Matrix(
                            new RowVector(1, 2, 5),
                            new RowVector(2, 5, 7));

            SingularValueDecomposition svd = new SingularValueDecomposition(m);
            Assert.AreEqual(m, svd.U * svd.S * Matrix.Transpose(svd.V));
        }
    }
}
