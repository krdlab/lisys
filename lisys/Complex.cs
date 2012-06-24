using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ���f���iParse�n���\�b�h�͖������j
    /// </summary>
    [Serializable]
    public struct Complex : IEquatable<Complex>
    {
        private double real;
        private double imaginary;

        #region �v���p�e�B

        /// <summary>
        /// ��������ݒ�E�擾����D
        /// </summary>
        public double Real
        {
            set { this.real = value; }
            get { return this.real; }
        }

        /// <summary>
        /// ��������ݒ�E�擾����D
        /// </summary>
        public double Imaginary
        {
            set { this.imaginary = value; }
            get { return this.imaginary; }
        }

        /// <summary>
        /// ���f���ʏ�ł̑傫�����擾����D
        /// </summary>
        public double Size
        {
            get { return Complex.Abs(this); }
        }

        /// <summary>
        /// ���f���ʏ�ł̕Ίp���擾����D
        /// </summary>
        public double Angle
        {
            get { return Complex.Arg(this); }
        }

        #endregion

        /// <summary>
        /// �ʂ̕��f���I�u�W�F�N�g����쐬����D
        /// </summary>
        /// <param name="c"></param>
        public Complex(Complex c)
        {
            this.real = c.real;
            this.imaginary = c.imaginary;
        }

        /// <summary>
        /// �e�v�f���w�肵�č쐬����D
        /// </summary>
        /// <param name="real">����</param>
        /// <param name="imaginary">����</param>
        public Complex(double real, double imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        #region ��{���\�b�h

        /// <summary>
        /// ���̃I�u�W�F�N�g�Ɠ����ł��邩�ۂ��𔻒肷��D
        /// </summary>
        /// <param name="obj">���肵�����I�u�W�F�N�g</param>
        /// <returns>�����ł���ꍇ��true���C����ȊO�̏ꍇ��false��Ԃ��D</returns>
        public override bool Equals(object obj)
        {
            if (obj is Complex)
            {
                return Complex.Equals(this, (Complex)obj);
            }
            return false;
        }

        #region IEquatable<Complex> �����o

        /// <summary>
        /// ���̃I�u�W�F�N�g�Ɠ����ł��邩�ۂ��𔻒肷��D
        /// </summary>
        /// <param name="other">���肵�����I�u�W�F�N�g</param>
        /// <returns>�����ł���ꍇ��true���C����ȊO�̏ꍇ��false��Ԃ��D</returns>
        public bool Equals(Complex other)
        {
            return Complex.Equals(this, other);
        }

        #endregion

        /// <summary>
        /// 2�̃I�u�W�F�N�g�������ł��邩�ۂ��𔻒肷��D
        /// ���e�덷�ɂ́C<see cref="LisysConfig.CalculationLowerLimit"/>���g�p�����D
        /// </summary>
        /// <param name="left"><see cref="Complex"/></param>
        /// <param name="right"><see cref="Complex"/></param>
        /// <returns>�����ł���ꍇ��true���C����ȊO�̏ꍇ��false��Ԃ��D</returns>
        public static bool Equals(Complex left, Complex right)
        {
            return Complex.Equals(left, right, LisysConfig.CalculationLowerLimit);
        }

        /// <summary>
        /// 2�̃I�u�W�F�N�g�������ł��邩�ۂ��𔻒肷��D
        /// </summary>
        /// <param name="left"><see cref="Complex"/></param>
        /// <param name="right"><see cref="Complex"/></param>
        /// <param name="delta">���e�덷�i���̐��l�����̍��ق͖�������j</param>
        /// <returns>�����ł���ꍇ��true���C����ȊO�̏ꍇ��false��Ԃ��D</returns>
        public static bool Equals(Complex left, Complex right, double delta)
        {
            if (Math.Abs(left.real - right.real) < delta
                && Math.Abs(left.imaginary - right.imaginary) < delta)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// �n�b�V���R�[�h���擾����D
        /// �i�����݂̎����́C����̃n�b�V���R�[�h�����s����₷���̂Œ��ӂ���j
        /// </summary>
        /// <returns>�n�b�V���l</returns>
        public override int GetHashCode()
        {
            return (int)Math.Round(this.real + this.imaginary);
        }

        /// <summary>
        /// ���̃I�u�W�F�N�g�̕�����\����Ԃ��D
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public override string ToString()
        {
            return string.Format("{0} + {1}i", this.real, this.imaginary);
        }

        #endregion

        /// <summary>
        /// ��Βl���v�Z����D
        /// </summary>
        /// <param name="c"><see cref="Complex"/></param>
        /// <returns>�����̐�Βl</returns>
        public static double Abs(Complex c)
        {
            return Math.Sqrt(c.real * c.real + c.imaginary * c.imaginary);
        }

        /// <summary>
        /// �Ίp���v�Z����D
        /// </summary>
        /// <param name="c"><see cref="Complex"/></param>
        /// <returns>�����̕Ίp</returns>
        /// <seealso cref="Math.Atan2"/>
        public static double Arg(Complex c)
        {
            return Math.Atan2(c.imaginary, c.real);
        }

        #region ������Z

        private Complex Add(Complex right)
        {
            this.real += right.real;
            this.imaginary += right.imaginary;
            return this;
        }

        private Complex Sub(Complex right)
        {
            this.real -= right.real;
            this.imaginary -= right.imaginary;
            return this;
        }

        private Complex Mul(Complex right)
        {
            double a = this.real;
            double b = this.imaginary;
            
            this.real = a * right.real - b * right.imaginary;
            this.imaginary = a * right.imaginary + b * right.real;
            return this;
        }

        private Complex Div(Complex right)
        {
            double a = this.real;
            double b = this.imaginary;
            double c = right.real;
            double d = right.imaginary;
            double denominator = c * c + d * d;

            this.real = (a * c + b * d) / denominator;
            this.imaginary = (b * c - a * d) / denominator;
            return this;
        }

        #endregion

        #region ���Z�q

        /// <summary>
        /// <see cref="double"/>�^�ɑ΂��閾���I�Ȍ^�ϊ����`����D
        /// </summary>
        /// <param name="value">�ϊ��ΏۂƂȂ�<see cref="double"/>�I�u�W�F�N�g</param>
        /// <returns><see cref="Complex"/></returns>
        public static explicit operator Complex(double value)
        {
            return new Complex(value, 0.0);
        }

        /// <summary>
        /// ���f���ǂ����̉��Z���`����D
        /// </summary>
        /// <param name="left">������<see cref="Complex"/></param>
        /// <param name="right">�E����<see cref="Complex"/></param>
        /// <returns>���Z���ʂ�<see cref="Complex"/></returns>
        public static Complex operator +(Complex left, Complex right)
        {
            return new Complex(left).Add(right);
        }

        /// <summary>
        /// ���f���ǂ����̌��Z���`����D
        /// </summary>
        /// <param name="left">������<see cref="Complex"/></param>
        /// <param name="right">�E����<see cref="Complex"/></param>
        /// <returns>���Z���ʂ�<see cref="Complex"/></returns>
        public static Complex operator -(Complex left, Complex right)
        {
            return new Complex(left).Sub(right);
        }

        /// <summary>
        /// ���f���ǂ����̏�Z���`����D
        /// </summary>
        /// <param name="left">������<see cref="Complex"/></param>
        /// <param name="right">�E����<see cref="Complex"/></param>
        /// <returns>��Z���ʂ�<see cref="Complex"/></returns>
        public static Complex operator *(Complex left, Complex right)
        {
            return new Complex(left).Mul(right);
        }

        /// <summary>
        /// ���f���ǂ����̏��Z���`����D
        /// </summary>
        /// <param name="left">������<see cref="Complex"/></param>
        /// <param name="right">�E����<see cref="Complex"/></param>
        /// <returns>���Z���ʂ�<see cref="Complex"/></returns>
        public static Complex operator /(Complex left, Complex right)
        {
            return new Complex(left).Div(right);
        }

        #endregion
    }
}
