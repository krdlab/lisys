using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// Vector �� I/F ��` (�ŗL�v���p�e�B�̒�`)�D
    /// </summary>
    public interface IVector : IRandomAccessible<double>
    {
        /// <summary>
        /// �m����
        /// </summary>
        double Norm { get; }

        /// <summary>
        /// ���v
        /// </summary>
        double Sum { get; }

        /// <summary>
        /// ���a
        /// </summary>
        double SumSq { get; }

        /// <summary>
        /// ���ϒl
        /// </summary>
        double Average { get; }

        /// <summary>
        /// �U�z�l
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
        /// �W�{���U
        /// </summary>
        double SampleVariance { get; }

        /// <summary>
        /// �s�Ε��U
        /// </summary>
        double UnbiasedVariance { get; }
    }
}
