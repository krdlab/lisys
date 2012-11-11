using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using KrdLab.Lisys;

namespace LisysTest
{
    [TestFixture]
    public class VectorTest
    {
        const double Ep = 1e-15;

        [Test]
        public void Test_Create()
        {
            Vector src = new Vector(1, 2, 3);
            Assert.That(src[0], Is.EqualTo(1));
            Assert.That(src[1], Is.EqualTo(2));
            Assert.That(src[2], Is.EqualTo(3));
            Assert.AreEqual(3, src.Size);

            Assert.True(Vector.Equals(src, new Vector(1, 2, 3), Ep));

            Vector one = new Vector(7);
            Assert.That(one[0], Is.EqualTo(7));
            Assert.AreEqual(1, one.Size);

            Vector copy = new Vector(src);
            Assert.True(Vector.Equals(copy, src, Ep));

            Vector zero = Vector.Zero(3);
            Assert.True(Vector.Equals(zero, new Vector(0, 0, 0), Ep));
        }

        [Test]
        public void Test_Equals_BasicRules()
        {
            Vector x = new Vector(1, 1.1, 1.2);
            Vector y = new Vector(1, 1.1, 1.2);
            Vector z = new Vector(1, 1.1, 1.2);

            // ��ѐ��`�F�b�N
            Assert.IsTrue(Vector.Equals(x, x, Ep));
            Assert.IsTrue(Vector.Equals(x, x, Ep));

            // ���˗�
            Assert.IsTrue(Vector.Equals(x, x, Ep));

            // �Ώ̗�
            Assert.AreEqual(Vector.Equals(x, y, Ep), Vector.Equals(y, x, Ep));
            Assert.AreEqual(Vector.Equals(x, y, Ep), Vector.Equals(y, x, Ep));

            // ���ڗ�
            Assert.IsTrue(Vector.Equals(x, y, Ep));
            Assert.IsTrue(Vector.Equals(y, z, Ep));
            Assert.IsTrue(Vector.Equals(x, z, Ep));

            // false
            Assert.IsFalse(Vector.Equals(x, new Vector(1, 1, 2), Ep));
            Assert.IsFalse(Vector.Equals(new Vector(1, 1, 2), x, Ep));
            Assert.IsFalse(Vector.Equals(x, null, Ep));
        }

        [Test]
        public void Test_BasicOperations()
        {
            Vector src = new Vector(1, 2, 3);
            Vector copied = new Vector(src);

            // ����
            double innerProduct = src * copied;
            Assert.That(innerProduct, Is.EqualTo(14).Within(Ep));

            // �t��
            Vector ie = -src;  // copy ������
            Assert.False(Object.ReferenceEquals(ie, src));
            Assert.True(Vector.Equals(ie, new Vector(-1, -2, -3), Ep));

            // View
            Vector large = new Vector(0.To(9).Select(i => (double) i));
            Vector view = new Vector(large[0.To(2)]);
            Assert.True(Vector.Equals(view, new Vector(0, 1, 2), Ep));
        }

        /// <summary>
        /// �v���p�e�B
        /// </summary>
        [Test]
        public void Test_IVector_Properties()
        {
            Vector v1 = new Vector(2, 3, 4, 5, 6);
            double d;
            int n;

            // �v���p�e�B
            d = v1.Average; // ���ϒl
            Assert.AreEqual(4, d, Ep);

            d = v1.Norm;    // 2-�m����
            Assert.AreEqual(9.4868329805051379959966806332982, d, Ep);

            // ���U
            Assert.That(v1.SampleVariance, Is.EqualTo(2.0).Within(Ep));
            Assert.That(v1.UnbiasedVariance, Is.EqualTo(2.5).Within(Ep));

            d = v1.Scatter; // �U�z�l
            Assert.AreEqual(10, d, Ep);

            n = v1.Size;    // �T�C�Y
            Assert.AreEqual(5, n);

            d = v1.Sum;     // ���v
            Assert.AreEqual(20, d, Ep);

            d = v1.SumSq;   // 2��a
            Assert.AreEqual(90, d, Ep);
        }

        [Test]
        public void Test_Vector_Operator()
        {
            Vector l = new Vector(1, 1, 1);
            Vector r = new Vector(1, 1, 1);

            Vector add = l + r;
            Assert.True(Vector.Equals(add, new Vector(2, 2, 2), Ep));

            Vector sub = l - r;
            Assert.True(Vector.Equals(sub, new Vector(0, 0, 0), Ep));

            Vector mul = l * 2;
            Assert.True(Vector.Equals(mul, new Vector(2, 2, 2), Ep));

            Vector mur = 2 * r;
            Assert.True(Vector.Equals(mur, new Vector(2, 2, 2), Ep));

            Vector div = l / 2;
            Assert.True(Vector.Equals(div, new Vector(0.5, 0.5, 0.5), Ep));
        }

        [Test]
        public void Test_Equals()
        {
            Vector v = new Vector(1, 2, 3);
            RowVector rv = new RowVector(1, 2, 3);
            ColumnVector cv = new ColumnVector(1, 2, 3);

            // Vector.Equals �̓N���X�̋�ʂ����Ȃ�
            Assert.True(Vector.Equals(v, v, Ep));
            Assert.True(Vector.Equals(v, rv, Ep));
            Assert.True(Vector.Equals(v, cv, Ep));

            //// RowVector.Equals �� RowVector ���m�̔�r�̂�
            //Assert.False(RowVector.Equals(rv, v, Ep));
            //Assert.True(RowVector.Equals(rv, rv, Ep));
            //Assert.False(RowVector.Equals(rv, cv, Ep));

            //// ColumnVector.Equals �� ColumnVector ���m�̔�r�̂�
            //Assert.False(ColumnVector.Equals(cv, v, Ep));
            //Assert.False(ColumnVector.Equals(cv, rv, Ep));
            //Assert.True(ColumnVector.Equals(cv, cv, Ep));
        }

        [Test]
        public void Test_RowVector_Operator()
        {
            RowVector l = new RowVector(1, 1, 1);
            RowVector r = new RowVector(1, 1, 1);

            RowVector add = l + r;
            Assert.True(RowVector.Equals(add, new RowVector(2, 2, 2), Ep));

            RowVector sub = l - r;
            Assert.True(RowVector.Equals(sub, new RowVector(0, 0, 0), Ep));

            RowVector mul = l * 2;
            Assert.True(RowVector.Equals(mul, new RowVector(2, 2, 2), Ep));

            RowVector mur = 2 * r;
            Assert.True(RowVector.Equals(mur, new RowVector(2, 2, 2), Ep));

            RowVector div = l / 2;
            Assert.True(RowVector.Equals(div, new RowVector(0.5, 0.5, 0.5), Ep));
        }

        [Test]
        public void Test_ColumnVector_Operator()
        {
            ColumnVector l = new ColumnVector(1, 1, 1);
            ColumnVector r = new ColumnVector(1, 1, 1);

            ColumnVector add = l + r;
            Assert.True(ColumnVector.Equals(add, new ColumnVector(2, 2, 2), Ep), add.ToString());

            ColumnVector sub = l - r;
            Assert.True(ColumnVector.Equals(sub, new ColumnVector(0, 0, 0), Ep));

            ColumnVector mul = l * 2;
            Assert.True(ColumnVector.Equals(mul, new ColumnVector(2, 2, 2), Ep));

            ColumnVector mur = 2 * r;
            Assert.True(ColumnVector.Equals(mur, new ColumnVector(2, 2, 2), Ep));

            ColumnVector div = l / 2;
            Assert.True(ColumnVector.Equals(div, new ColumnVector(0.5, 0.5, 0.5), Ep));
        }

        [Test]
        public void Test_Vector_RowVector_Operator()
        {
            // Vector ��������ƁC�S�� Vector �ɂȂ� (���ۂɂȂ�)
            Vector l = new Vector(1, 1, 1);
            Vector r = new Vector(1, 1, 1);
            RowVector rv = new RowVector(1, 1, 1);

            Vector add = l + rv;
            Assert.That(add, Is.Not.InstanceOf(typeof(RowVector)));
            Assert.True(Vector.Equals(add, new Vector(2, 2, 2), Ep));
            add = rv + r;
            Assert.That(add, Is.Not.InstanceOf(typeof(RowVector)));
            Assert.True(Vector.Equals(add, new Vector(2, 2, 2), Ep));

            Vector sub = l - rv;
            Assert.That(sub, Is.Not.InstanceOf(typeof(RowVector)));
            Assert.True(Vector.Equals(sub, new Vector(0, 0, 0), Ep));
            sub = rv - r;
            Assert.That(sub, Is.Not.InstanceOf(typeof(RowVector)));
            Assert.True(Vector.Equals(sub, new Vector(0, 0, 0), Ep));
        }

        [Test]
        public void Test_Vector_ColumnVector_Operator()
        {
            // Vector ��������ƁC�S�� Vector �ɂȂ� (���ۂɂȂ�)
            Vector l = new Vector(1, 1, 1);
            Vector r = new Vector(1, 1, 1);
            ColumnVector cv = new ColumnVector(1, 1, 1);

            Vector add = l + cv;
            Assert.That(add, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vector.Equals(add, new Vector(2, 2, 2), Ep));
            add = cv + r;
            Assert.That(add, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vector.Equals(add, new Vector(2, 2, 2), Ep));

            Vector sub = l - cv;
            Assert.That(sub, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vector.Equals(sub, new Vector(0, 0, 0), Ep));
            sub = cv - r;
            Assert.That(sub, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vector.Equals(sub, new Vector(0, 0, 0), Ep));
        }

        [Test]
        public void Test_IVector_ColumnVector_Operator()
        {
            // Vector ��������ƁC�S�� Vector �ɂȂ� (���ۂɂȂ�)
            IVector l = new Vector(1, 1, 1);
            IVector r = new Vector(1, 1, 1);
            ColumnVector cv = new ColumnVector(1, 1, 1);

            IVector add = l + cv;
            Assert.That(add, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vectors.Equals(add, new Vector(2, 2, 2), Ep));
            add = cv + r;
            Assert.That(add, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vectors.Equals(add, new Vector(2, 2, 2), Ep));

            IVector sub = l - cv;
            Assert.That(sub, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vectors.Equals(sub, new Vector(0, 0, 0), Ep));
            sub = cv - r;
            Assert.That(sub, Is.Not.InstanceOf(typeof(ColumnVector)));
            Assert.True(Vectors.Equals(sub, new Vector(0, 0, 0), Ep));
        }

        [Test]
        public void Test_Action()
        {
            Vector vec = new Vector(1, 1, 1, 1, 1);
            vec.ForEach((i, v) => ++v);
            Assert.True(Vector.Equals(vec, new Vector(1, 1, 1, 1, 1), Ep));
            vec.Apply((i, v) => v + i);
            Assert.True(Vector.Equals(vec, new Vector(1, 2, 3, 4, 5), Ep));
        }

        [Test]
        public void Test_Others() // TODO
        {
            Vector v1 = new Vector(1, 2, 3, 4, 5);
            // �o��
            Assert.That(v1.ToArray(), Is.EqualTo(new[] { 1.0, 2, 3, 4, 5 }));
            string s = v1.ToString();
            string csv = Vectors.ToCsv(v1);

            v1 = Vector.Fill(10, 1);
            Assert.AreEqual(10, v1.Size);
            Assert.AreEqual(new Vector(1, 1, 1, 1, 1, 1, 1, 1, 1, 1), v1);

            v1.Flip();          // �������]
            v1.Zero();          // �S�Ă̗v�f��0
            Assert.AreEqual(new Vector(new Size(10)).Zero(), v1);
        }

        [Test]
        public void Test_View()
        {
            Vector v2 = new Vector(1, 2, 3, 4, 5);
            // �����x�N�g�����擾�i�R�s�[�ł͂Ȃ��Q�Ɓj
            IVector vv = v2[0, 2, 4];
            vv[1] = 999;
            Assert.True(Vectors.Equals(v2, new Vector(1, 2, 999, 4, 5), Ep));
        }

        [Test]
        public void Test_Functions()
        {
            Vector src = new Vector(2, 3, 4, 6, 4);
            Vector v = Func.Standardize(src, src.Average, Math.Sqrt(src.UnbiasedVariance));
            Assert.AreEqual(0, v.Average, Ep);
            Assert.AreEqual(1, v.UnbiasedVariance, Ep);
        }

        [Test]
        public void Test_IVector_Vector_Operator()
        {
            IVector iv = new Vector(1, 2, 3);
            Vector vv = new Vector(1, 2, 3);
            // iv = iv + iv :: IVector
            iv = vv + vv;
            iv = iv + vv;
            iv = vv + iv;
            iv += vv;
        }

        [Test]
        public void Test_Combination()
        {
            RowVector rv = new RowVector(1, 2, 3);
            ColumnVector cv = new ColumnVector(1, 2, 3);

            Assert.That(rv * cv, Is.EqualTo(14).Within(Ep));
            Assert.That(cv * rv, Is.EqualTo(
                new Matrix(new [,] {
                    { 1.0, 2, 3 },
                    { 2, 4, 6 },
                    { 3, 6, 9 }
                })
                ).Within(Ep)
            );
        }

        [Test]
        public void Test_Extends()
        {
            var v = new Vector(1, 2, 3, 4, 5);
            Assert.That(v.ToRow(), Is.InstanceOf(typeof(RowVector)));
            Assert.That(v.ToRow(), Is.EqualTo(v));
            Assert.That(v.ToColumn(), Is.InstanceOf(typeof(ColumnVector)));
            Assert.That(v.ToColumn(), Is.EqualTo(v));

            var iv = v[0, 2, 4];
            Assert.That(iv.ToVector(), Is.InstanceOf(typeof(Vector)));
            Assert.That(iv.ToVector(), Is.EqualTo(new Vector(1, 3, 5)));
        }
    }
}
