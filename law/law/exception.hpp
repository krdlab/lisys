#pragma once

namespace krdlab {
	namespace law {
		namespace exception {

			/// <summary>
			/// CLAPACK �ɓn���������s���ł��������Ƃ�������O�D
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
				/// �G���[��Ԃ�Ԃ��D��{�I�ɂ� CLAPACK �֐��̖߂�l��Ԃ��D
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
			/// ���ʂ����߂�ߒ��ŃG���[�������������Ƃ�������O�D
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
				/// �G���[��Ԃ�Ԃ��D��{�I�ɂ� CLAPACK �֐��̖߂�l��Ԃ��D
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
