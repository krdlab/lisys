using System;
using System.Collections.Generic;
using System.Text;
using KrdLab.Lisys.Method;

namespace KrdLab.Lisys
{
    /// <summary>
    /// 統計処理関数を定義したクラス
    /// </summary>
    public static class Function
    {
        /// <summary>
        /// 標準化された<see cref="RowVector"/>を作成する．
        /// </summary>
        /// <param name="vector">標準化される<see cref="RowVector"/>（実際は，この引数のコピーが標準化される）</param>
        /// <param name="varType">標準化に使用する分散の種類</param>
        /// <returns>標準化された<see cref="RowVector"/></returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// 標準偏差値が設定されている精度の下限値未満の場合にthrowされる．
        /// </exception>
        public static RowVector Standardize(RowVector vector, VarianceType varType)
        {
            return _Standardize_T(new RowVector(vector), varType);
        }

        /// <summary>
        /// 標準化された<see cref="ColumnVector"/>を作成する．
        /// </summary>
        /// <param name="vector">標準化される<see cref="ColumnVector"/>（実際は，この引数のコピーが標準化される）</param>
        /// <param name="varType">標準化に使用する分散の種類</param>
        /// <returns>標準化された<see cref="ColumnVector"/></returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// 標準偏差値が設定されている精度の下限値未満の場合にthrowされる．
        /// </exception>
        public static ColumnVector Standardize(ColumnVector vector, VarianceType varType)
        {
            return _Standardize_T(new ColumnVector(vector), varType);
        }

        /// <summary>
        /// 標準化された<see cref="Vector"/>を作成する．
        /// </summary>
        /// <param name="vector">標準化される<see cref="Vector"/>（実際は，この引数のコピーが標準化される）</param>
        /// <param name="varType">標準化に使用する分散の種類</param>
        /// <returns>標準化された<see cref="Vector"/></returns>
        /// <seealso cref="IResizableVector"/>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// 標準偏差値が設定されている精度の下限値未満の場合にthrowされる．
        /// </exception>
        public static Vector Standardize(Vector vector, VarianceType varType)
        {
            return _Standardize_T(new Vector(vector), varType);
        }

        /// <summary>
        /// 標準化された<see cref="IVector"/>インタフェース実装オブジェクトを作成する．
        /// このメソッドは，インタフェースで受け取り，インタフェースで返す．
        /// </summary>
        /// <param name="vector">
        /// 標準化される<see cref="IVector"/>インタフェース実装オブジェクト（実際は，この引数のコピーが標準化される）
        /// </param>
        /// <param name="varType">標準化に使用される分散の種類</param>
        /// <returns>標準化された<see cref="IVector"/>インタフェース実装オブジェクト</returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// 標準偏差値が設定されている精度の下限値未満の場合にthrowされる．
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
        /// 指定された行列を標準化する．
        /// </summary>
        /// <param name="X">標準化する行列（直接変更されない）</param>
        /// <param name="target">行ごとの標準化なのか，列ごとの標準化なのかを指定する</param>
        /// <param name="varType">標準化に使用される分散のタイプ</param>
        /// <returns>Xのコピーを標準化した行列</returns>
        /// 
        /// <exception cref="Exception.ValueIsLessThanLimitException">
        /// 標準偏差値が設定されている精度の下限値未満の場合にthrowされる．
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
        /// 2つのベクトルの相関を求める．
        /// </summary>
        /// <param name="vx">ベクトル</param>
        /// <param name="vy">ベクトル</param>
        /// <returns>相関</returns>
        /// <exception cref="Exception.MismatchSizeException">
        /// ベクトルのサイズが一致しないときにthrowされる．
        /// </exception>
        public static double Correl(IVector vx, IVector vy)
        {
            return CorrelationAnalysis.Correl(vx, vy);
        }

        /// <summary>
        /// <see cref="IVector"/>の値を対角要素に持つ対角行列を新しく作成する．
        /// </summary>
        /// <param name="v">対角行列の対角要素を格納している<see cref="IVector"/></param>
        /// <returns>対角行列</returns>
        /// <exception cref="Exception.ZeroSizeException">
        /// ベクトルサイズが0の場合にthrowされる．
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

        //// 歪度
        //public static double Skewness()
        //{
        //}

        //// 尖度
        //public static double Kurtosis()
        //{
        //}
    }
}
