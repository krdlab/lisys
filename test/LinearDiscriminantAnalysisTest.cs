using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    class LinearDiscriminantAnalysisTest
    {
        const double Ep = 1e-7;

        [Test]
        public void Test_01()
        {
            // iris データ
            // http://en.wikipedia.org/wiki/Iris_flower_data_set

            const int C = 3;
            var data = new Matrix[C];
            for (int i = 0; i < C; ++i)
            {
                data[i] =
                    File.ReadAllLines(String.Format("data/iris/data_{0}.csv", i), Encoding.UTF8)
                        .Select(line =>
                            new RowVector(line.Split(',').Select(s => Double.Parse(s))))
                        .ToMatrix();
            }

            Lda result = Func.Lda(data);

            // Coefficients of linear discriminants
            var ldc1 = result.Eigenvectors[0];
            var ldc2 = result.Eigenvectors[1];

            Func<double, double> abs = v => Math.Abs(v);
            Action<double, double, double> AssertAbsEqual = (ac, ex, ep) => Assert.That(abs(ac), Is.EqualTo(abs(ex)).Within(ep));

            {
                var expect = new Vector(0.8293776, 1.5344731, -2.2012117, -2.8104603);
                expect /= expect.Norm;
                AssertAbsEqual(ldc1[0], expect[0], Ep);
                AssertAbsEqual(ldc1[1], expect[1], Ep);
                AssertAbsEqual(ldc1[2], expect[2], Ep);
                AssertAbsEqual(ldc1[3], expect[3], Ep);
                Assert.That(ldc1[0] * ldc1[1], Is.GreaterThan(0));
                Assert.That(ldc1[2] * ldc1[3], Is.GreaterThan(0));
                Assert.That(ldc1[0] * ldc1[2], Is.LessThan(0));
                Assert.That(ldc1[0] * ldc1[3], Is.LessThan(0));
            }
            {
                var expect = new Vector(0.02410215, 2.16452123, -0.93192121, 2.83918785);
                expect /= expect.Norm;
                AssertAbsEqual(ldc2[0], expect[0], Ep);
                AssertAbsEqual(ldc2[1], expect[1], Ep);
                AssertAbsEqual(ldc2[2], expect[2], Ep);
                AssertAbsEqual(ldc2[3], expect[3], Ep);
                Assert.That(ldc2[0] * ldc2[1], Is.GreaterThan(0));
                Assert.That(ldc2[1] * ldc2[3], Is.GreaterThan(0));
                Assert.That(ldc2[0] * ldc2[2], Is.LessThan(0));
                Assert.That(ldc2[1] * ldc2[2], Is.LessThan(0));
                Assert.That(ldc2[3] * ldc2[2], Is.LessThan(0));
            }

            Assert.That(result.Ratios[0], Is.EqualTo(0.9912).Within(1e-4));
            Assert.That(result.Ratios[1], Is.EqualTo(0.0088).Within(1e-4));
        }
    }
}
