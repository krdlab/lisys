using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class VectorTest
    {
        /// <summary>
        /// 基本メソッド
        /// </summary>
        [Test]
        public void Test00()
        {
            Vector x = new Vector(1, 1, 1);
            Vector y = new Vector(1, 1, 1);
            Vector z = new Vector(1, 1, 1);

            // 一貫性チェック
            Assert.IsTrue(x.Equals(x));
            Assert.IsTrue(x.Equals(x));
            Assert.IsTrue(x.Equals(x));
            Assert.IsTrue(x.Equals(x));

            // 反射律
            Assert.IsTrue(Vector.Equals(x, x));

            // 対称律
            Assert.AreEqual(x.Equals(y), y.Equals(x));
            Assert.AreEqual(Vector.Equals(x, y), Vector.Equals(y, x));

            // 推移律
            Assert.IsTrue(x.Equals(y));
            Assert.IsTrue(y.Equals(z));
            Assert.IsTrue(x.Equals(z));

            // false
            Assert.IsFalse(x.Equals(new Vector(1, 1, 2)));

            // ハッシュコードチェック
            Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
            Assert.AreEqual(y.GetHashCode(), z.GetHashCode());
        }

        /// <summary>
        /// コンストラクション
        /// </summary>
        [Test]
        public void Test01()
        {
            Vector v;

            v = new Vector(new double[] { 1, 1, 1 });
            Assert.AreEqual(new Vector(1, 1, 1), v);

            v = new Vector(3);
            Assert.AreEqual(3, v.Size);
            Assert.AreEqual(new Vector(0, 0, 0), v);

            v = new Vector(new Vector(1, 2, 3));
            Assert.AreEqual(new Vector(1, 2, 3), v);
        }

        /// <summary>
        /// プロパティ
        /// </summary>
        [Test]
        public void Test02()
        {
            Vector v1 = new Vector(2, 3, 4, 5, 6);
            double d;
            int n;

            // プロパティ
            d = v1.Average; // 平均値
            Assert.AreEqual(4, d, LisysConfig.CalculationLowerLimit);

            d = v1.Norm;    // 2-ノルム
            Assert.AreEqual(9.4868329805051379959966806332982, d, LisysConfig.CalculationLowerLimit);

            d = v1.Variance;// 標本分散
            Assert.AreEqual(2.5, d, LisysConfig.CalculationLowerLimit);

            d = v1.Scatter; // 散布値
            Assert.AreEqual(10, d, LisysConfig.CalculationLowerLimit);

            n = v1.Size;    // サイズ
            Assert.AreEqual(5, n);

            d = v1.Sum;     // 合計
            Assert.AreEqual(20, d, LisysConfig.CalculationLowerLimit);

            d = v1.SumSq;   // 2乗和
            Assert.AreEqual(90, d, LisysConfig.CalculationLowerLimit);
        }

        /// <summary>
        /// メソッド
        /// </summary>
        [Test]
        public void Test03()
        {
            Vector v1 = new Vector(1, 1, 1, 1, 1);

            // 各要素に対するアクションの適用
            v1.ForEach(delegate(double v) { ++v; });
            Assert.AreEqual(new Vector(1, 1, 1, 1, 1), v1);

            v1.ForEach(delegate(ref double v) { ++v; });       // 各要素を+1する
            Assert.AreEqual(new Vector(2, 2, 2, 2, 2), v1);

            v1.ForEach(delegate(int i, double v) { v += i; });
            Assert.AreEqual(new Vector(2, 2, 2, 2, 2), v1);

            v1.ForEach(delegate(int i, ref double v) { v += i; });// 各要素を 要素値 + index に更新する 
            Assert.AreEqual(new Vector(2, 3, 4, 5, 6), v1);

            // 出力
            double[] a = v1.ToArray();  // 配列として出力
            Assert.AreEqual(new double[] { 2, 3, 4, 5, 6 }, a);

            string s = v1.ToString();

            string csv = v1.ToCsv();

            // リサイズ
            v1.Resize(10);      // リサイズ
            Assert.AreEqual(10, v1.Size);
            Assert.AreEqual(new Vector(0, 0, 0, 0, 0, 0, 0, 0, 0, 0), v1);

            v1.Resize(10, 1);   // リサイズ（要素値の指定）
            Assert.AreEqual(10, v1.Size);
            Assert.AreEqual(new Vector(1, 1, 1, 1, 1, 1, 1, 1, 1, 1), v1);

            Vector v2 = new Vector(1, 2, 3, 4, 5);

            v1.CopyFrom(v2);    // コピー
            Assert.AreEqual(v2, v1);

            v1.Flip();          // 符号反転
            v1.Zero();          // 全ての要素を0
            Assert.AreEqual(new Vector(5).Zero(), v1);

            v1 = new Vector(v2);    // クローンを作成

            Assert.AreEqual(v1, v2);

            v1.Clear();         // クリア
            Assert.AreEqual(0, v1.Size);
            Assert.AreNotEqual(v1, v2);


            // 部分ベクトルを取得（コピーではなく参照）
            IVector vv = v2[0, 2, 4];
            vv[1] = 999;
            Assert.AreEqual(new Vector(1, 2, 999, 4, 5), v2);

            vv.Zero();
            Assert.AreEqual(new Vector(0, 2, 0, 4, 0), v2);

            IVector vvv = new Vector(v2);

            vv = vvv.Subvector(2, 3);
            vv[0] = vv[1] = vv[2] = 11;
            Assert.AreEqual(new Vector(0, 2, 11, 11, 11), vvv);

            // 演算例
            v1 = new RowVector(1, 1, 1, 1, 1);
            Vector t = v1 + v2;
            Assert.AreEqual(new Vector(1, 3, 1, 5, 1), t);

            t = v1 - v2;
            Assert.AreEqual(new Vector(1, -1, 1, -3, 1), t);

            double d = v1 * v2;
            Assert.AreEqual(6, d, LisysConfig.CalculationLowerLimit);

            v1 *= 2;
            Assert.AreEqual(new Vector(2, 2, 2, 2, 2), v1);

            t = v1 / 2;
            Assert.AreEqual(new Vector(1, 1, 1, 1, 1), t);


            // 特殊関数
            v2 = Function.Standardize(new Vector(2, 3, 4, 6, 4), VarianceType.Sample);
            Assert.AreEqual(0, v2.Average, LisysConfig.CalculationLowerLimit);
            Assert.AreEqual(1, v2.Variance, LisysConfig.CalculationLowerLimit);
        }

        /// <summary>
        /// 演算子
        /// </summary>
        [Test]
        public void Test04()
        {
            IVector iv = new Vector(1, 2, 3);
            Vector vv = new Vector(1, 2, 3);
            RowVector rv = new RowVector(1, 2, 3);

            // IVector, Vector, RowVector 27パターン

            // IVector <-
            // iv = iv + iv;
            iv = Vector.Add(iv, iv);

            iv = vv + vv;
            iv = rv + rv;

            iv = vv + rv;
            iv = rv + vv;

            iv = iv + vv;
            iv = vv + iv;
            iv += vv;

            iv = iv + rv;
            iv = rv + iv;
            iv += rv;


            // Vector <-
            //vv = iv + iv;   // 不可（メソッドにしないとできない）
            vv = Vector.Add(iv, iv);

            vv = vv + vv;
            vv += vv;

            vv = rv + rv;

            vv = rv + vv;
            vv = vv + rv;
            vv += rv;

            vv = iv + vv;
            vv = vv + iv;
            vv += iv;

            // RowVector <-
            //rv = iv + iv;   // 不可（メソッドにしないとできない）
            rv = RowVector.Add(iv, iv);

            //rv = vv + vv;   // 不可（メソッドにしないとできない）
            rv = RowVector.Add(vv, vv);
            rv = rv + rv;
            rv += rv;

            rv = rv + vv;
            rv = vv + rv;
            rv += vv;

            //rv = iv + vv;   // 不可（メソッドにしないとできない）
            //rv = vv + iv;   // 不可（メソッドにしないとできない）

            rv = iv + rv;
            rv = rv + iv;
            rv += iv;
        }

        /// <summary>
        /// 複合
        /// </summary>
        [Test]
        public void Test09()
        {
            RowVector rv = new RowVector(1, 2, 3);
            ColumnVector cv = new ColumnVector(1, 2, 3);

            double val = rv * cv;
            Assert.AreEqual(14, val, LisysConfig.CalculationLowerLimit);

            Matrix m = cv * rv;
            Assert.AreEqual(new Matrix(new double[,] { { 1, 2, 3 }, { 2, 4, 6 }, { 3, 6, 9 } }), m);
        }
    }
}
