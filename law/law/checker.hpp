#pragma once

namespace krdlab {
	namespace law {

		/// <summary>
		/// �v�Z��K�v�ɂȂ�`�F�b�N���[�`�����`����D
		/// </summary>
		public ref class CalculationChecker
		{
		public:
			///<summary>
			/// �L���Ȓl�ł���ƌ��Ȃ������l�i�f�t�H���g�l�F1e-15�j
			///</summary>
			static double CalculationLowerLimit = 1e-15;	// Excel�̐��x���炢���ȁH

			/// <summary>
			/// �ݒ肳�ꂽ���x�����̒l���ۂߍ��ށD
			/// </summary>
			static void Round(array<doublereal>^ arr, double delta)
			{
				for(int i=0; i < arr->Length; ++i) {
					arr[i] = (Math::Abs(arr[i]) < delta) ? (0.0) : (arr[i]);
				}
			}

			/// <summary>
			/// ���x�������l��艺�ł��邩�ǂ����𒲂ׂ�D
			/// </summary>
			/// <param name="value">���ׂ����l</param>
			/// <returns>�����l�������ꍇ��true�C���̑���false��Ԃ��D</returns>
			static bool IsLessThanLimit(double value)
			{
				if(value < CalculationLowerLimit)
				{
					return true;
				}
				return false;
			}
		};
	}
}
