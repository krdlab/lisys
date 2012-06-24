using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// Vector の I/F 定義 (固有プロパティの定義)．
    /// </summary>
    public interface IVector : IRandomAccessible<double>
    {
        /// <summary>
        /// ノルム
        /// </summary>
        double Norm { get; }

        /// <summary>
        /// 合計
        /// </summary>
        double Sum { get; }

        /// <summary>
        /// 二乗和
        /// </summary>
        double SumSq { get; }

        /// <summary>
        /// 平均値
        /// </summary>
        double Average { get; }

        /// <summary>
        /// 散布値
        /// </summary>
        /// <remarks>
        /// <code>
        /// val = 0.0;
        /// avg = this.Average;
        /// foreach(e in this)
        /// {
        ///     val += ((e - avg) * (e - avg));
        /// }
        /// </code>
        /// </remarks>
        double Scatter { get; }

        /// <summary>
        /// 標本分散
        /// </summary>
        double SampleVariance { get; }

        /// <summary>
        /// 不偏分散
        /// </summary>
        double UnbiasedVariance { get; }
    }
}
