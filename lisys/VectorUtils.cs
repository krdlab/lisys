using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KrdLab.Lisys
{
    /// <summary>
    /// �N���X���f�I�ɑ��݂���`�F�b�N�������`����D
    /// </summary>
    internal static class VectorChecker
    {
        /// <summary>
        /// �����Ɏw�肳�ꂽ�x�N�g���̃T�C�Y��0�łȂ����Ƃ𒲂ׂ�D
        /// </summary>
        /// <param name="v"></param>
        /// <exception cref="Exception.ZeroSizeException">
        /// �x�N�g���T�C�Y��0�̏ꍇ��throw�����D
        /// </exception>
        public static void ZeroSize(IVector v)
        {
            if (v.Size == 0)
            {
                throw new Exception.ZeroSizeException();
            }
        }

        /// <summary>
        /// �����Ɏw�肳�ꂽ�x�N�g���̃T�C�Y����v���邱�Ƃ𒲂ׂ�D
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <exception cref="Exception.MismatchSizeException">
        /// �x�N�g���̃T�C�Y����v���Ȃ��Ƃ���throw�����D
        /// </exception>
        public static void MismatchSize(IVector v1, IVector v2)
        {
            if (v1.Size != v2.Size)
            {
                throw new Exception.MismatchSizeException();
            }
        }

        public static void ValueIsLessThanLimit(double value)
        {
            if (krdlab.law.CalculationChecker.IsLessThanLimit(value))
            {
                throw new Exception.ValueIsLessThanLimitException();
            }
        }
    }

}
