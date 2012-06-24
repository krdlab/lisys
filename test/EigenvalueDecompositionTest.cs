using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;
using KrdLab.Lisys.Method;

namespace LisysTest
{
    [TestFixture]
    public class EigenvalueDecompositionTest
    {
        [Test]
        public void Test01()
        {
            Matrix X = new Matrix(  new RowVector(1, 0, 2),
                                    new RowVector(0, 1, 0),
                                    new RowVector(2, 0, 1));
            
            Assert.IsTrue(X.IsSymmetric);

            EigenvalueDecomposition evd = new EigenvalueDecomposition(X);
            evd.Sort(EigenvalueDecomposition.SortOrder.Descending);

            Assert.AreEqual( 3, evd.RealEigenvalues[0], LisysConfig.CalculationLowerLimit);
            Assert.AreEqual( 1, evd.RealEigenvalues[1], LisysConfig.CalculationLowerLimit);
            Assert.AreEqual(-1, evd.RealEigenvalues[2], LisysConfig.CalculationLowerLimit);

            Assert.AreEqual(new Vector(1 / Math.Sqrt(2), 0, 1 / Math.Sqrt(2)), evd.RealEigenvectors[0]);
            Assert.AreEqual(new Vector(0, 1, 0), evd.RealEigenvectors[1]);
            Assert.AreEqual(new Vector(-1 / Math.Sqrt(2), 0, 1 / Math.Sqrt(2)), evd.RealEigenvectors[2]);
        }

        [Test]
        public void Test02()
        {
            const double angle = Math.PI / 6;
            Matrix X = new Matrix(new RowVector(Math.Cos(angle), -Math.Sin(angle), 0),
                                    new RowVector(Math.Sin(angle), Math.Cos(angle), 0),
                                    new RowVector(0, 0, 1));

            EigenvalueDecomposition evd = new EigenvalueDecomposition(X);
            evd.Sort(EigenvalueDecomposition.SortOrder.Descending);

            Assert.IsTrue(evd.HasComplexValue);

            List<Complex> cv = new List<Complex>();
            int count = evd.RealEigenvalues.Size;
            for (int i = 0; i < count; ++i)
            {
                cv.Add(new Complex(evd.RealEigenvalues[i], evd.ImaginaryEigenvalues[i]));
            }

            Assert.AreEqual(1, (cv[0] * cv[1] * cv[2]).Size, LisysConfig.CalculationLowerLimit);

            Assert.AreEqual(new Complex(1, 0), cv[0]);
            Assert.AreEqual(new Complex(Math.Cos(angle), -Math.Sin(angle)), cv[1]);
            Assert.AreEqual(new Complex(Math.Cos(angle),  Math.Sin(angle)), cv[2]);
        }
    }
}
