using System;
using System.Collections.Generic;
using System.Text;
using KrdLab.Lisys.Method;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ���v�����֐����`�����N���X
    /// </summary>
    public static class Function
    {
        /// <summary>
        /// �W�������ꂽ<see cref="RowVector"/>���쐬����D
        /// </summary>
        /// <param name="vector">�W���������<see cref="RowVector"/>�i���ۂ́C���̈����̃R�s�[���W���������j</param>
        /// <param name="varType">�W�����Ɏg�p���镪�U�̎��</param>
        /// <returns>�W�������ꂽ<see cref="RowVector"/></returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// �W���΍��l���ݒ肳��Ă��鐸�x�̉����l�����̏ꍇ��throw�����D
        /// </exception>
        public static RowVector Standardize(RowVector vector, VarianceType varType)
        {
            return _Standardize_T(new RowVector(vector), varType);
        }

        /// <summary>
        /// �W�������ꂽ<see cref="ColumnVector"/>���쐬����D
        /// </summary>
        /// <param name="vector">�W���������<see cref="ColumnVector"/>�i���ۂ́C���̈����̃R�s�[���W���������j</param>
        /// <param name="varType">�W�����Ɏg�p���镪�U�̎��</param>
        /// <returns>�W�������ꂽ<see cref="ColumnVector"/></returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// �W���΍��l���ݒ肳��Ă��鐸�x�̉����l�����̏ꍇ��throw�����D
        /// </exception>
        public static ColumnVector Standardize(ColumnVector vector, VarianceType varType)
        {
            return _Standardize_T(new ColumnVector(vector), varType);
        }

        /// <summary>
        /// �W�������ꂽ<see cref="Vector"/>���쐬����D
        /// </summary>
        /// <param name="vector">�W���������<see cref="Vector"/>�i���ۂ́C���̈����̃R�s�[���W���������j</param>
        /// <param name="varType">�W�����Ɏg�p���镪�U�̎��</param>
        /// <returns>�W�������ꂽ<see cref="Vector"/></returns>
        /// <seealso cref="IResizableVector"/>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// �W���΍��l���ݒ肳��Ă��鐸�x�̉����l�����̏ꍇ��throw�����D
        /// </exception>
        public static Vector Standardize(Vector vector, VarianceType varType)
        {
            return _Standardize_T(new Vector(vector), varType);
        }

        /// <summary>
        /// �W�������ꂽ<see cref="IVector"/>�C���^�t�F�[�X�����I�u�W�F�N�g���쐬����D
        /// ���̃��\�b�h�́C�C���^�t�F�[�X�Ŏ󂯎��C�C���^�t�F�[�X�ŕԂ��D
        /// </summary>
        /// <param name="vector">
        /// �W���������<see cref="IVector"/>�C���^�t�F�[�X�����I�u�W�F�N�g�i���ۂ́C���̈����̃R�s�[���W���������j
        /// </param>
        /// <param name="varType">�W�����Ɏg�p����镪�U�̎��</param>
        /// <returns>�W�������ꂽ<see cref="IVector"/>�C���^�t�F�[�X�����I�u�W�F�N�g</returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// �W���΍��l���ݒ肳��Ă��鐸�x�̉����l�����̏ꍇ��throw�����D
        /// </exception>
        public static IVector Standardize(IVector vector, VarianceType varType)
        {
            return _Standardize_T(new Vector(vector), varType);
        }

        private static T _Standardize_T<T>(T vector, VarianceType varType) where T : IVector
        {
            VectorChecker.ZeroSize(vector);

            double avg = vector.Average;
            double std = Math.Sqrt(varType == VarianceType.DivN ? vector.Scatter / vector.Size : vector.Variance);

            if (krdlab.law.CalculationChecker.IsLessThanLimit(std))
            {
                throw new Exception.ValueIsLessThanLimitException(
                    "The standard variation is less than \"Calculation Limit\".\n"
                    + "Values = " + vector.ToString());
            }

            for (int i = 0; i < vector.Size; ++i)
            {
                vector[i] = (vector[i] - avg) / std;
            }
            return vector;
        }

        /// <summary>
        /// �w�肳�ꂽ�s���W��������D
        /// </summary>
        /// <param name="X">�W��������s��i���ڕύX����Ȃ��j</param>
        /// <param name="target">�s���Ƃ̕W�����Ȃ̂��C�񂲂Ƃ̕W�����Ȃ̂����w�肷��</param>
        /// <param name="varType">�W�����Ɏg�p����镪�U�̃^�C�v</param>
        /// <returns>X�̃R�s�[��W���������s��</returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// �W���΍��l���ݒ肳��Ă��鐸�x�̉����l�����̏ꍇ��throw�����D
        /// </exception>
        public static Matrix Standardize(Matrix X, Target target, VarianceType varType)
        {
            Matrix ret = new Matrix(X);
            if (target == Target.EachRow)
            {
                for (int r = 0; r < ret.RowSize; ++r)
                {
                    ret.Rows[r] = Standardize(ret.Rows[r], varType);
                }
            }
            else
            {
                for (int c = 0; c < ret.ColumnSize; ++c)
                {
                    ret.Columns[c] = Standardize(ret.Columns[c], varType);
                }
            }
            return ret;
        }


        /// <summary>
        /// 2�̃x�N�g���̑��ւ����߂�D
        /// </summary>
        /// <param name="vx">�x�N�g��</param>
        /// <param name="vy">�x�N�g��</param>
        /// <returns>����</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// �x�N�g���̃T�C�Y����v���Ȃ��Ƃ���throw�����D
        /// </exception>
        public static double Correl(IVector vx, IVector vy)
        {
            return CorrelationAnalysis.Correl(vx, vy);
        }

        /// <summary>
        /// <see cref="IVector"/>�̒l��Ίp�v�f�Ɏ��Ίp�s���V�����쐬����D
        /// </summary>
        /// <param name="v">�Ίp�s��̑Ίp�v�f���i�[���Ă���<see cref="IVector"/></param>
        /// <returns>�Ίp�s��</returns>
        /// <exception cref="Exception.ZeroSizeException">
        /// �x�N�g���T�C�Y��0�̏ꍇ��throw�����D
        /// </exception>
        public static Matrix CreateDiagonalMatrix(IVector v)
        {
            VectorChecker.ZeroSize(v);
            Matrix m = new Matrix(v.Size, v.Size);
            m.Zero();
            v.ForEach(delegate(int i, double e)
            {
                m[i, i] = e;
            });
            return m;
        }

        //// �c�x
        //public static double Skewness()
        //{
        //}

        //// ��x
        //public static double Kurtosis()
        //{
        //}
    }
}
