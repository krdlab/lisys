using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ライブラリに関する情報を提供するクラス
    /// </summary>
    public class LisysConfig
    {
        /// <summary>
        /// 改行コードを取得する．
        /// </summary>
        public static string LineSeparator
        {
            get { return "\r\n"; }
        }

        ///<summary>
        /// 有効な値であると見なす下限値を設定・取得する（デフォルト値：1e-15）．
        ///</summary>
        public static double CalculationLowerLimit
        {
            set { krdlab.law.CalculationChecker.CalculationLowerLimit = value; }
            get { return krdlab.law.CalculationChecker.CalculationLowerLimit; }
        }


        private LisysConfig() { }
    }
}
