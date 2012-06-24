using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    class PrincipalComponentAnalysisTest
    {
        const double Ep = 1e-7;

        [Test]
        public void Test_01()
        {
            // ???
            Matrix data = new Matrix(
                        new RowVector(86, 67),
                        new RowVector(71, 78),
                        new RowVector(42, 39),
                        new RowVector(62, 98),
                        new RowVector(96, 61),
                        new RowVector(39, 45),
                        new RowVector(50, 64),
                        new RowVector(78, 52),
                        new RowVector(51, 76),
                        new RowVector(89, 93));

            Pca result = Func.Pca(data, cor: true);
            Assert.That(result.Ratios[0], Is.EqualTo(0.6880464).Within(Ep));
            Assert.That(result.Ratios[1], Is.EqualTo(0.3119536).Within(Ep));

            var ir2 = Math.Sqrt(2);
            Func<double, double> abs = v => Math.Abs(v);

            // 正負の自由があるため，π の差異は同じとみなす
            Assert.That(abs(result.Eigenvectors[0][0]), Is.EqualTo(1 / ir2).Within(Ep));
            Assert.That(abs(result.Eigenvectors[0][1]), Is.EqualTo(1 / ir2).Within(Ep));
            Assert.True(result.Eigenvectors[0][0] * result.Eigenvectors[0][1] > 0);

            Assert.That(abs(result.Eigenvectors[1][0]), Is.EqualTo(1 / ir2).Within(Ep));
            Assert.That(abs(result.Eigenvectors[1][1]), Is.EqualTo(1 / ir2).Within(Ep));
            Assert.True(result.Eigenvectors[1][0] * result.Eigenvectors[1][1] < 0);
        }

        private const double Ep2 = 1e-3;

        [Test]
        public void Test_02()
        {
            // サイエンス社　ライブラリ新数学大系 E20 「多変量解析法入門」 (初版第 7 刷発行)
            // 永田　靖・棟近雅彦　共著
            // P.132　表 9.1
            var data = new Matrix(
                new[,] {
                    {86.0,79,67,68},
                    {71,75,78,84},
                    {42,43,39,44},
                    {62,58,98,95},
                    {96,97,61,63},
                    {39,33,45,50},
                    {50,53,64,72},
                    {78,66,52,47},
                    {51,44,76,72},
                    {89,92,93,91} });

            Pca res = Func.Pca(data);

            Assert.That(res.Ratios[0], Is.EqualTo(0.6889703).Within(Ep));
            Assert.That(res.Ratios[1], Is.EqualTo(0.2965368).Within(Ep));
            Assert.That(res.Ratios[2], Is.EqualTo(0.01326883).Within(Ep));
            Assert.That(res.Ratios[3], Is.EqualTo(0.001224026).Within(Ep));

            Func<double, double> abs = v => Math.Abs(v);
            /* R の結果 (分散共分散行列)
                     Comp.1 Comp.2 Comp.3 Comp.4
                国語 -0.545  0.440 -0.549  0.456
                英語 -0.592  0.401  0.556 -0.424
                数学 -0.443 -0.576 -0.452 -0.518
                理科 -0.395 -0.561  0.431  0.586
             */
            var vecs = res.Eigenvectors;
            Assert.That(abs(vecs[0][0]), Is.EqualTo(abs(-0.545)).Within(Ep2));
            Assert.That(abs(vecs[0][1]), Is.EqualTo(abs(-0.592)).Within(Ep2));
            Assert.That(abs(vecs[0][2]), Is.EqualTo(abs(-0.443)).Within(Ep2));
            Assert.That(abs(vecs[0][3]), Is.EqualTo(abs(-0.395)).Within(Ep2));
            Assert.True(vecs[0][0] * vecs[0][1] * vecs[0][2] * vecs[0][3] > 0);

            Assert.That(abs(vecs[1][0]), Is.EqualTo(abs(0.440)).Within(Ep2));
            Assert.That(abs(vecs[1][1]), Is.EqualTo(abs(0.401)).Within(Ep2));
            Assert.That(abs(vecs[1][2]), Is.EqualTo(abs(-0.576)).Within(Ep2));
            Assert.That(abs(vecs[1][3]), Is.EqualTo(abs(-0.561)).Within(Ep2));
            Assert.True(Math.Sign(vecs[1][0]) == Math.Sign(vecs[1][1]));
            Assert.True(Math.Sign(vecs[1][2]) == Math.Sign(vecs[1][3]));
            Assert.False(Math.Sign(vecs[1][1]) == Math.Sign(vecs[1][2]));
        }
    }
}
