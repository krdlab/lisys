using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;
using System.IO;

namespace LisysTest
{
    [TestFixture]
    class LinearRegressionTest
    {
        const double Ep = 1e-5;

        [Test]
        public void Test_01()
        {
            // airquality (R x64 2.14.1)
            var y = File.ReadAllText("data/airquality/data_y.csv", Encoding.UTF8)
                .Split(',')
                .Select(s => Double.Parse(s))
                .ToVector();
            var xs = File.ReadLines("data/airquality/data_xs.csv", Encoding.UTF8)
                .Select(line =>
                    line.Split(',').Select(s => Double.Parse(s)).ToRow())
                .ToMatrix();

            Assert.That(y.Size, Is.EqualTo(xs.RowSize));

            Lr result = Func.Lr(y, xs);

            // (Intercept)      Solar.R         Wind         Temp
            var expect = new[] { -64.34208, 0.05982, -3.33359, 1.65209 };

            Func<double, double> abs = v => Math.Abs(v);
            Action<double, double, double> AssertEqual = (ac, ex, ep) => Assert.That(ac, Is.EqualTo(ex).Within(ep));

            var a = result.CoefficientVector;
            AssertEqual(a[0], expect[0], Ep);
            AssertEqual(a[1], expect[1], Ep);
            AssertEqual(a[2], expect[2], Ep);
            AssertEqual(a[3], expect[3], Ep);

            Assert.That(result.R2, Is.EqualTo(0.6059).Within(1e-4));
            Assert.That(result.Rc2, Is.EqualTo(0.5948).Within(1e-4));
            Assert.That(result.DofP, Is.EqualTo(3));
            Assert.That(result.DofE, Is.EqualTo(107));
        }
    }
}
