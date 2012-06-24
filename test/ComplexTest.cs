using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class ComplexTest
    {
        [Test]
        public void Test01()
        {
            Complex c = new Complex(1, 2);
            Assert.AreEqual(1, c.Real, LisysConfig.CalculationLowerLimit);
            Assert.AreEqual(2, c.Imaginary, LisysConfig.CalculationLowerLimit);

            Assert.AreEqual(new Complex(1, 2), c);
            Assert.AreEqual(new Complex(c), c);

            Complex c1 = new Complex(1, 1);

            Assert.AreEqual(Math.Sqrt(2), c1.Size, LisysConfig.CalculationLowerLimit);
            Assert.AreEqual(Math.PI / 4, c1.Angle, LisysConfig.CalculationLowerLimit);
        }

        [Test]
        public void Test02()
        {
            Assert.AreEqual(new Complex( 2,  3), new Complex(1, 1) + new Complex(1, 2));
            Assert.AreEqual(new Complex( 0, -1), new Complex(1, 1) - new Complex(1, 2));
            Assert.AreEqual(new Complex(-1,  3), new Complex(1, 1) * new Complex(1, 2));
            Assert.AreEqual(new Complex(0.2, 1.6), new Complex(-3, 2) / new Complex(1, 2));
        }
    }
}
