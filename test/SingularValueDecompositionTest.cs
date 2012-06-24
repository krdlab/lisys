using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class SingularValueDecompositionTest
    {
        const double Ep = 1e-13;

        [Test]
        public void Test01()
        {
            Matrix m = new Matrix(
                            new RowVector(0, 1, -1),
                            new RowVector(-1, 1, 0),
                            new RowVector(2, 1, 0));
            Svd svd = Func.Svd(m);
            Assert.That(svd.U * svd.S * Matrix.T(svd.V), Is.EqualTo(m).Within(Ep));
        }

        [Test]
        public void Test02()
        {
            Matrix m = new Matrix(
                            new RowVector(1, 2, 5),
                            new RowVector(2, 4, 7),
                            new RowVector(5, 5, 8),
                            new RowVector(3, 4, 7),
                            new RowVector(3, 5, 4));
            Svd svd = Func.Svd(m);
            Assert.That(svd.U * svd.S * Matrix.T(svd.V), Is.EqualTo(m).Within(Ep));
        }

        [Test]
        public void Test03()
        {
            Matrix m = new Matrix(
                            new RowVector(1, 2, 5),
                            new RowVector(2, 5, 7));
            Svd svd = Func.Svd(m);
            Assert.That(svd.U * svd.S * Matrix.T(svd.V), Is.EqualTo(m).Within(Ep));
        }
    }
}
