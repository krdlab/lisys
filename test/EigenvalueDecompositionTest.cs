using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class EigenvalueDecompositionTest
    {
        const double Ep = 1e-15;

        [Test]
        public void TestSymmetric()
        {
            Matrix X = new Matrix(  new RowVector(1, 0, 2),
                                    new RowVector(0, 1, 0),
                                    new RowVector(2, 0, 1));
            
            Eigen evd = Func.Eigen(X);
            evd.Sort(Eigen.SortOrder.Descending);

            Assert.That(evd.RealEigenvalues[0], Is.EqualTo(3).Within(Ep));
            Assert.That(evd.RealEigenvalues[1], Is.EqualTo(1).Within(Ep));
            Assert.That(evd.RealEigenvalues[2], Is.EqualTo(-1).Within(Ep));

            Assert.IsTrue(Vector.Equals(
                new Vector(1 / Math.Sqrt(2), 0, 1 / Math.Sqrt(2)),
                evd.RealEigenvectors[0],
                Ep));
            Assert.IsTrue(Vector.Equals(
                new Vector(0, 1, 0),
                evd.RealEigenvectors[1],
                Ep));
            Assert.IsTrue(Vector.Equals(
                new Vector(-1 / Math.Sqrt(2), 0, 1 / Math.Sqrt(2)),
                evd.RealEigenvectors[2],
                Ep));
        }

        [Test]
        public void TestRotation()
        {
            const double angle = Math.PI / 6;
            Matrix X = new Matrix(new RowVector(Math.Cos(angle), -Math.Sin(angle), 0),
                                  new RowVector(Math.Sin(angle), Math.Cos(angle), 0),
                                  new RowVector(0, 0, 1));
            Eigen evd = Func.Eigen(X);
            evd.Sort(Eigen.SortOrder.Descending);

            List<Complex> cv = new List<Complex>();
            int count = evd.RealEigenvalues.Size;
            for (int i = 0; i < count; ++i)
            {
                cv.Add(new Complex(evd.RealEigenvalues[i], evd.ImaginaryEigenvalues[i]));
            }

            Assert.That((cv[0] * cv[1] * cv[2]).Magnitude, Is.EqualTo(1.0).Within(Ep));

            Assert.AreEqual(new Complex(1, 0), cv[0]);
            Assert.That(cv[1].Real, Is.EqualTo(Math.Cos(angle)).Within(Ep));
            Assert.That(cv[1].Imaginary, Is.EqualTo(-Math.Sin(angle)).Within(Ep));
            Assert.That(cv[2].Real, Is.EqualTo(Math.Cos(angle)).Within(Ep));
            Assert.That(cv[2].Imaginary, Is.EqualTo(Math.Sin(angle)).Within(Ep));
        }

        [Test]
        public void TestSortAscending()
        {
            Eigen evd = new Eigen(
                new Vector(1, 3, 2),
                new Vector(0, 0, 0),
                new List<Vector>(new[] { new Vector(), new Vector(), new Vector() }),
                new List<Vector>(new[] { new Vector(), new Vector(), new Vector() }));
            evd.Sort(Eigen.SortOrder.Ascending);
            Assert.That(evd.RealEigenvalues[0], Is.EqualTo(1));
            Assert.That(evd.RealEigenvalues[1], Is.EqualTo(2));
            Assert.That(evd.RealEigenvalues[2], Is.EqualTo(3));
        }

        [Test]
        public void TestSortDescending()
        {
            Eigen evd = new Eigen(
                new Vector(1, 3, 2),
                new Vector(0, 0, 0),
                new List<Vector>(new[] { new Vector(), new Vector(), new Vector() }),
                new List<Vector>(new[] { new Vector(), new Vector(), new Vector() }));
            evd.Sort(Eigen.SortOrder.Descending);
            Assert.That(evd.RealEigenvalues[0], Is.EqualTo(3));
            Assert.That(evd.RealEigenvalues[1], Is.EqualTo(2));
            Assert.That(evd.RealEigenvalues[2], Is.EqualTo(1));
        }
    }
}
