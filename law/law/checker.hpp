#pragma once

namespace krdlab {
	namespace law {

		/// <summary>
		/// 計算上必要になるチェックルーチンを定義する．
		/// </summary>
		public ref class CalculationChecker
		{
		public:
			///<summary>
			/// 有効な値であると見なす下限値（デフォルト値：1e-15）
			///</summary>
			static double CalculationLowerLimit = 1e-15;	// Excelの精度くらいかな？

			/// <summary>
			/// 設定された精度未満の値を丸め込む．
			/// </summary>
			static void Round(array<doublereal>^ arr, double delta)
			{
				for(int i=0; i < arr->Length; ++i) {
					arr[i] = (Math::Abs(arr[i]) < delta) ? (0.0) : (arr[i]);
				}
			}

			/// <summary>
			/// 精度が下限値より下であるかどうかを調べる．
			/// </summary>
			/// <param name="value">調べたい値</param>
			/// <returns>下限値を下回る場合はtrue，その他はfalseを返す．</returns>
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
