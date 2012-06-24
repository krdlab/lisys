#pragma once

namespace krdlab {
	namespace law {
		namespace exception {

			/// <summary>
			/// CLAPACK に渡す引数が不正であったことを示す例外．
			/// </summary>
            public ref class LapackArgumentException : public System::ArgumentException
			{
			private:
				const int info;

			public:
				LapackArgumentException(const int code)
					: info(code)
				{}
				LapackArgumentException(const int code, System::String^ message)
					: System::ArgumentException(message), info(code)
				{}
				LapackArgumentException(const int code, System::String^ message, System::Exception^ cause)
					: System::ArgumentException(message, cause), info(code)
				{}

				/// <summary>
				/// エラー状態を返す．基本的には CLAPACK 関数の戻り値を返す．
				/// </summary>
				property const int Info
				{
					const int get()
					{
						return this->info;
					}
				}
			};

			/// <summary>
			/// 結果を求める過程でエラーが発生したことを示す例外．
			/// </summary>
			public ref class LapackResultException : public System::Exception
			{
			private:
				const int info;

			public:
				LapackResultException(const int code)
					: info(code)
				{}
				LapackResultException(const int code, System::String^ message)
					: System::Exception(message), info(code)
				{}
				LapackResultException(const int code, System::String^ message, System::Exception^ cause)
					: System::Exception(message, cause), info(code)
				{}

				/// <summary>
				/// エラー状態を返す．基本的には CLAPACK 関数の戻り値を返す．
				/// </summary>
				property const int Info
				{
					const int get()
					{
						return this->info;
					}
				}
			};

		}// end namespace exception
	}// end namespace claw
}// end namespace krdlab
