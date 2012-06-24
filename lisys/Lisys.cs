using System;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// ���C�u�����Ɋւ������񋟂���N���X
    /// </summary>
    public class LisysConfig
    {
        /// <summary>
        /// ���s�R�[�h���擾����D
        /// </summary>
        public static string LineSeparator
        {
            get { return "\r\n"; }
        }

        ///<summary>
        /// �L���Ȓl�ł���ƌ��Ȃ������l��ݒ�E�擾����i�f�t�H���g�l�F1e-15�j�D
        ///</summary>
        public static double CalculationLowerLimit
        {
            set { krdlab.law.CalculationChecker.CalculationLowerLimit = value; }
            get { return krdlab.law.CalculationChecker.CalculationLowerLimit; }
        }


        private LisysConfig() { }
    }
}
