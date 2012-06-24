// law.hpp
#pragma once
using namespace System;

namespace krdlab {
	namespace law {
		using namespace exception;

		/// <summary>
		/// CLAPACK �� CLR ��ŗ��p���邽�߂̃��b�p�[�N���X
		/// </summary>
		public ref class func
		{
		public:
			/// <summary>
			/// �x�N�g���v�f�̑��a���v�Z����D
			/// </summary>
			/// <param name="arr">�z�� (�ύX����Ȃ�)</param>
			/// <returns>���a</returns>
            static doublereal dasum(array<doublereal>^ arr)
            {
                integer N = arr->Length;
                pin_ptr<doublereal> pX = &arr[0];
                integer incX = 1;
                return dasum_(&N, pX, &incX);
            }

			/// <summary>
			/// �x�N�g���̃m�������v�Z����D
			/// </summary>
			/// <param name="arr">�m������m�肽���z�� (�ύX����Ȃ�)</param>
			/// <returns>�m����</returns>
			static double dnrm2(array<doublereal>^ arr)
			{
				integer N = arr->Length;
				pin_ptr<doublereal> pX = &arr[0];
				integer incX = 1;
				return dnrm2_(&N, pX, &incX);
			}


			/// <summary>
			/// �z��̊e�v�f���X�J���{����D
			/// </summary>
			/// <param name="ret">���ʔz�� (�����œK�؂ȃT�C�Y�����蓖�Ă���)</param>
			/// <param name="arr">�X�J���{�����z��</param>
			/// <param name="d">�X�J��</param>
			/// <returns>CLAPACK/BLAS/SRC/dscal.c �ɂ��΁C��� 0 ���Ԃ��Ă���D</returns>
			static int dscal_r(array<doublereal>^% ret, array<doublereal>^ arr, double d)
			{
				integer S = arr->Length;
                
                ret = gcnew array<doublereal>(S);
                System::Array::Copy(arr, ret, S);

				doublereal N = d;

				pin_ptr<doublereal> pX = &ret[0]; // arr �̏㏑����h���Cret �Ɍ��ʂ����邽��
				integer incX = 1;

				return dscal_(&S, &N, pX, &incX);
			}

            /// <summary>
            /// calculate: d*x (d: scala, x: vector)
            /// </summary>
			static int dscal(array<doublereal>^ x, double d)
			{
				integer N = x->Length;
				doublereal a = d;
				pin_ptr<doublereal> pX = &x[0];
				integer incX = 1;
				return dscal_(&N, &a, pX, &incX);
			}

			/// <summary>
			/// z := ax + y ���v�Z����D
			/// </summary>
			/// <param name="z">���ʔz�� (�����œK�؂ȃT�C�Y�����蓖�Ă���)</param>
			/// <param name="a">�X�J��</param>
			/// <param name="x">�z�� (�ύX����Ȃ�)</param>
			/// <param name="y">�z�� (�ύX����Ȃ�)</param>
			/// <returns>CLAPACK/BLAS/SRC/daxpy.c �ɂ��΁C��� 0 ���Ԃ��Ă���D</returns>
			static int daxpy_r(array<doublereal>^% z, doublereal a, array<doublereal>^ x, array<doublereal>^ y)
			{				
				z = gcnew array<double>(y->Length);
				System::Array::Copy(y, z, y->Length);

				integer N = x->Length;

				pin_ptr<doublereal> pX = &x[0];
				integer incx = 1;
				
				pin_ptr<doublereal> pY = &z[0];  // y �̏㏑����h���Cz �Ɍ��ʂ����邽��
				integer incy = 1;
				
				return daxpy_(&N, &a, pX, &incx, pY, &incy);
			}

            /// <summary>
            /// calculate: a*x + y -> y (a: scala, x: vector, y: vector)
            /// </summary>
			static int daxpy(doublereal a, array<doublereal>^ x, array<doublereal>^ y)
			{
				integer N = x->Length;
				pin_ptr<doublereal> pX = &x[0];
				integer incx = 1;
				pin_ptr<doublereal> pY = &y[0];
				integer incy = 1;
				return daxpy_(&N, &a, pX, &incx, pY, &incy);
			}

			/// <summary>
			/// �x�N�g���̓��ς��Ƃ�D
			/// </summary>
			/// <param name="v1">�z�� (�ύX����Ȃ�)</param>
			/// <param name="v2">�z�� (�ύX����Ȃ�)</param>
			/// <returns><paramref name="v1"/> �� <paramref name="v2"/> �̓���</returns>
			static doublereal ddot(array<doublereal>^ v1, array<doublereal>^ v2)
			{
				integer N = v1->Length;
				pin_ptr<doublereal> pX = &v1[0];
				integer incX = 1;
				pin_ptr<doublereal> pY = &v2[0];
				integer incY = 1;
				return ddot_(&N, pX, &incX, pY, &incY);
			}


			/// <summary>
			/// �s��ǂ����̏�Z���s���D
			/// </summary>
			/// <param name="C">���ʂ̍s�� (�����œK�؂ȃT�C�Y�̃����������蓖�Ă���)</param>
			/// <param name="A">�s�� (�ύX����Ȃ�)</param>
			/// <param name="B">�s�� (�ύX����Ȃ�)</param>
			/// <returns>��� 0 ���Ԃ��Ă���D</returns>
			/// <exception cref="ArgumentException"><paramref name="A"/> �̗񐔂� <paramref name="B"/> �̍s���Ƃ���v���Ȃ��ꍇ</exception>
			/// <remarks>
			/// <para>�Ή�����CLAPACK�֐� (CLAPACK/BLAS/SRC/dgemm.c)</para>
			/// <code>
			///	int dgemm_(char* transA, char* transB, integer* M, integer* N, integer* K,
			///	            doublereal* alpha,
			///	            doublereal* A, integer* lda,
			///	            doublereal* B, integer* ldb,
			///	            doublereal* beta,
			///	            doublereal* C, integer* ldc);
			/// </code>
			/// </remarks>
			static int dgemm(array<doublereal>^% C, int% c_row, int% c_col,
							 array<doublereal>^  A, int  a_row, int  a_col,
							 array<doublereal>^  B, int  b_row, int  b_col)
			{
				if(a_col != b_row)
				{
					throw gcnew ArgumentException("a_col != b_row");
				}

				// ���� Matrix �̃T�C�Y
				c_row = a_row;
				c_col = b_col;

				C = gcnew array<doublereal>(c_row*c_col);
				
				char transA = 'N';
				char transB = 'N';
				integer M = a_row;
				integer N = b_col;
				integer K = a_col; // or b_row
				
				doublereal alpha = 1.0;
				
				pin_ptr<doublereal> pA = &A[0];
				integer lda = std::max(1, M); // transA == 'N' then
				
				pin_ptr<doublereal> pB = &B[0];
				integer ldb = std::max(1, K); // transB == 'N' then
				
				doublereal beta = 0.0;
				
				pin_ptr<doublereal> pC = &C[0];
				integer ldc = std::max(1, M);
				
                // C := alpha * A * B + beta * C
				return dgemm_( &transA, &transB, &M, &N, &K,
								&alpha,
								pA, &lda,
								pB, &ldb,
								&beta,
								pC, &ldc );
			}


			/// <summary>
			/// Y = AX ���v�Z����D
			/// </summary>
			/// <param name="Y">���ʂ̗�x�N�g�� (�����œK�؂ȃT�C�Y�̃����������蓖�Ă���)</param>
			/// <param name="A">�s�� (�ύX����Ȃ�)</param>
			/// <param name="X">��x�N�g�� (�ύX����Ȃ�)</param>
			/// <returns>��� 0 ���Ԃ��Ă���D</returns>
			/// <exception cref="ArgumentException"><paramref name="A"/> �̗񐔂� <paramref name="X"/> �̃T�C�Y�Ƃ���v���Ȃ��ꍇ</exception>
			/// <remarks>
			/// <para>�Ή�����CLAPACK�֐� (CLAPACK/BLAS/SRC/dgemv.c)</para>
			/// <code>
			/// int
			/// dgemv_(char* trans, integer* M, integer* N,
			///	       doublereal* alpha,
			///	       doublereal* A, integer* lda,
			///	       doublereal* X, integer* incX,
			///	       doublereal* beta,
			///	       doublereal* Y, integer* incY);
			/// </code>
			/// </remarks>
			static int dgemv(array<doublereal>^% Y,
							 array<doublereal>^ A, int a_row, int a_col,
							 array<doublereal>^ X)
			{
				if(a_col != X->Length)
				{
					throw gcnew ArgumentException("a_col != X->Length");
				}

				Y = gcnew array<doublereal>(a_row);
				
				char trans = 'N';	// A �͓]�u���Ȃ�

				integer M = a_row;
				integer N = a_col;
				
				doublereal alpha = 1.0;
				
				pin_ptr<doublereal> pA = &A[0];
				integer lda = a_row;  // := max( 1, m )
				
				pin_ptr<doublereal> pX = &X[0];
				integer incX = 1;
				
				doublereal beta = 0.0;
				
				pin_ptr<doublereal> pY = &Y[0];
				integer incY = 1;

				// Y := alpha * A + beta * Y
				return dgemv_(&trans,
                              &M, &N,
                              &alpha, pA, &lda,
                                      pX, &incX,
                              &beta , pY, &incY);
			}

			/// <summary>
			/// Y = XA ���v�Z����D
			/// </summary>
			/// <param name="Y">���ʂ̍s�x�N�g�� (�����œK�؂ȃT�C�Y�̃����������蓖�Ă���)</param>
			/// <param name="X">�s�x�N�g�� (�ύX����Ȃ�)</param>
			/// <param name="A">�s��i�ύX����Ȃ��j</param>
			/// <returns>��� 0 ���Ԃ��Ă���D</returns>
			/// <exception cref="ArgumentException"><paramref name="X"/> �̃T�C�Y�� <paramref name="A"/> �̍s���Ƃ���v���Ȃ��ꍇ</exception>
			/// <remarks>
			/// <para>�Ή�����CLAPACK�֐� (CLAPACK/BLAS/SRC/dgemv.c)</para>
			/// <code>
			/// int
			/// dgemv_(char* trans, integer* M, integer* N,
			///	       doublereal* alpha,
			///	       doublereal* A, integer* lda,
			///	       doublereal* X, integer* incX,
			///	       doublereal* beta,
			///	       doublereal* Y, integer* incY);
			/// </code>
			/// </remarks>
			static int dgemv(array<doublereal>^% Y,
							 array<doublereal>^  X,
							 array<doublereal>^  A, int a_row, int a_col)
			{
				if(X->Length != a_row)
				{
					throw gcnew ArgumentException("X->Length != a_row");
				}

				Y = gcnew array<doublereal>(a_col);
				
				char trans = 'T';
				integer M = a_row;
				integer N = a_col;
				
				doublereal alpha = 1.0;
				
				pin_ptr<doublereal> pA   = &A[0];
				integer     lda = a_row;
				
				pin_ptr<doublereal> pX    = &X[0];
				integer     incX = 1;
				
				doublereal beta = 0.0;
				
				pin_ptr<doublereal> pY    = &Y[0];
				integer     incY = 1;

				// transpose(A) * X = Y => transpose(Y) == X * A (Y�͔z��Ȃ̂ŁC���ʂ�Ԃ��ۂɓ]�u����K�v�͂Ȃ�)
				return dgemv_( &trans, &M, &N, &alpha, pA, &lda,
										pX, &incX, &beta, pY, &incY );
			}


			/// <summary>
			/// <para>A * X = B ������ (X ����)�D</para>
			/// <para>A �� n�~n �̍s��CX �� B �� n�~nrhs �̍s��ł���D</para>
			/// </summary>
			/// <param name="X"><c>A * X = B</c> �̉��ł��� X ���i�[����� (���ۂɂ� B �Ɠ����I�u�W�F�N�g���w��)</param>
			/// <param name="x_row">�s�� X �̍s�����i�[����� (<c>== <paramref name="b_row"/></c>)</param>
			/// <param name="x_col">�s�� X �̗񐔂��i�[����� (<c>== <paramref name="b_col"/></c>)</param>
			/// <param name="A">�W���s�� (LU�����̌��ʂł��� P*L*U �ɏ�����������DP*L*U�ɂ��Ă�<see cref="dgetrf"/>���Q��)</param>
			/// <param name="a_row">�s��A�̍s��</param>
			/// <param name="a_col">�s��A�̗�</param>
			/// <param name="B">�s�� B (������CLAPACK�֐��ɂ�� X �̒l���i�[�����)</param>
			/// <param name="b_row">�s��B�̍s��</param>
			/// <param name="b_col">�s��B�̗�</param>
			/// <returns>��� 0 ���Ԃ��Ă���D</returns>
			/// <exception cref="LapackArgumentException">������ dgesv_�֐��ɓn���ꂽ�����ɖ�肪�������ꍇ</exception>
			/// <exception cref="LapackResultException">�s�� A �� LU �����ɂ����āCU[i, i] �� 0 �ƂȂ��Ă��܂����ꍇ (�������߂邱�Ƃ��ł��Ȃ�)</exception>
			/// <remarks>
			/// <para>�Ή�����CLAPACK�֐� (CLAPACK/BLAS/SRC/dgesv.c)</para>
			/// <code>
			/// int
			/// dgesv_(integer *n, integer *nrhs,
			///        doublereal *a, integer *lda, integer *ipiv,
			///        doublereal *b, integer *ldb, integer *info);
			/// </code>
			/// <para>dgesv_ �֐��̓����ł� LU �������g�p����Ă���D</para>
			/// </remarks>
			static int dgesv(array<doublereal>^% X, int% x_row, int% x_col,
							 array<doublereal>^  A, int  a_row, int  a_col,
							 array<doublereal>^  B, int  b_row, int  b_col)
			{
				integer n    = a_row;	// input: �A���ꎟ�������̎����D�����s��[A]�̎���(n��0)�D 
				integer nrhs = b_col;	// input: �s��B��Column��
				
				pin_ptr<doublereal> a = &A[0];
				// �z��`���� a[lda�~n]
				//   input : n�sn��̌W���s��[A]
				//   output: LU������̍s��[L]�ƍs��[U]�D�������C�s��[L]�̒P�ʑΊp�v�f�͊i�[����Ȃ��D
				//           A=[P]*[L]*[U]�ł���C[P]�͍s�Ɨ�����ւ��鑀��ɑΉ�����u���s��ƌĂ΂�C0��1���i�[�����D
				
				integer lda = n;
				// input: �s��A�̑�ꎟ��(�̃������i�[��)�Dlda��max(1,n)�ł���C�ʏ�� lda==n �ŗǂ��D

				integer* ipiv = new integer[n];
				// output: �傫��n�̔z��D�u���s��[P]���`���鎲�I��p�Y�����D

				pin_ptr<doublereal> b = &B[0];
				// input/output: �z��`����b[ldb�~nrhs]�D�ʏ��nrhs��1�Ȃ̂ŁC�z��`����b[ldb]�ƂȂ�D
				// input : b[ldb�~nrhs]�̔z��`���������E�Ӎs��{B}�D
				// output: info==0 �̏ꍇ�ɁCb[ldb�~nrhs]�`���̉��s��{X}���i�[�����D

				integer ldb = b_row;
				// input: �z��b�̑�ꎟ��(�̃������i�[��)�Dldb��max(1,n)�ł���C�ʏ�� ldb==n �ŗǂ��D

				integer info = 1;
				// output:
				// info==0: ����I��
				// info < 0: info==-i �Ȃ�΁Ci�Ԗڂ̈����̒l���Ԉ���Ă��邱�Ƃ������D
				// 0 < info <N-1: �ŗL�x�N�g���͌v�Z����Ă��Ȃ����Ƃ������D
				// info > N: LAPACK���Ŗ�肪���������Ƃ������D

				int ret;
				try
				{
					ret = dgesv_(&n, &nrhs, a, &lda, ipiv, b, &ldb, &info);
					
					if(info == 0)
					{
						X = B;
						x_row = b_row;
						x_col = b_col;
					}
					else
					{
						X = nullptr;
						x_row = 0;
						x_col = 0;
						
						if(info < 0)
						{
                            throw gcnew LapackArgumentException(info, "dgesv_ argument");
						}
						else
						{
                            throw gcnew LapackResultException(info, "dgesv_ result");
						}
					}
				}
				finally
				{
					delete[] ipiv; ipiv = nullptr;
				}
				return ret;
			}


			/// <summary>
			/// ���ْl����
			/// </summary>
			/// <param name="X">���ْl���������s�� (���e�͔j�󂳂��)</param>
			/// <param name="x_row">�s�� <paramref name="X"/> �̍s��</param>
			/// <param name="x_col">�s�� <paramref name="X"/> �̗�</param>
			/// <param name="U">�e���ْl�ɑΉ����鍶���كx�N�g�����C�񂲂Ƃɓ����Ă���s��</param>
			/// <param name="u_row">�s�� <paramref name="U"/> �̍s��</param>
			/// <param name="u_col">�s�� <paramref name="U"/> �̗�</param>
			/// <param name="S">�Ίp�v�f�����ْl�ł���s�� (���ْl�͍~���Ɋi�[����Ă��� S[i, i] >= S[i+1, i+1])</param>
			/// <param name="s_row">�s�� <paramref name="S"/> �̍s��</param>
			/// <param name="s_col">�s�� <paramref name="S"/> �̗�</param>
			/// <param name="V">�e���ْl�ɑΉ�����E���كx�N�g�����C�񂲂Ƃɓ����Ă���s��</param>
			/// <param name="v_row">�s�� <paramref name="V"/> �̍s��</param>
			/// <param name="v_col">�s�� <paramref name="V"/> �̗�</param>
			/// <returns>��� 0 ���Ԃ��Ă���D</returns>
			/// <exception cref="LapackArgumentException">������ dgesvd_�֐��ɓn���ꂽ�����ɖ�肪�������ꍇ</exception>
			/// <exception cref="LapackResultException">�v�Z���������Ȃ������ꍇ</exception>
			/// <remarks>
			/// <para>�Ή�����CLAPACK�֐� (CLAPACK/BLAS/SRC/dgesvd.c)</para>
			/// <code>
			/// int dgesvd_(char *jobu, char *jobvt, integer *m, integer *n, 
			///	            doublereal *a, integer *lda,
			///             doublereal *s, doublereal *u, integer *ldu, doublereal *vt, integer *ldvt,
			///             doublereal *work, integer *lwork, 
			///	            integer *info);
			/// </code>
			/// </remarks>
			static int dgesvd(  array<doublereal>^  X, int  x_row, int  x_col,
								array<doublereal>^% U, int% u_row, int% u_col,
								array<doublereal>^% S, int% s_row, int% s_col,
								array<doublereal>^% V, int% v_row, int% v_col)
			{
				char jobu = 'S';
				// ���s�� U �̗�i�����ْl�x�N�g���j���ǂ��܂Łi�u�S�āv or �u�����v�j�v�Z����̂����w�肷��D
				// 
				// �I�����͈ȉ��̒ʂ�ł���D
				//   = 'A': all M columns of U�i�s�� U �� M�~M �ɂȂ�j
				//   = 'S': the first min(m, n) columns of U�i�s�� U �ɂ͍����كx�N�g�����i�[�����j
				//   = 'O': 'S' �w�莞�̍s�� U �̓��e�ōs�� X ���㏑�������
				//   = 'N': �����كx�N�g�����v�Z���Ȃ�

				char jobvt = 'S';
				// ���s�� V'�i�]�u���ꂽ V�j�̍s�x�N�g���i�s�� V �̗�x�N�g���F�E���كx�N�g���j��
				//   �ǂ��܂Łi�u�S�āvor�u�����v�j�v�Z����̂����w�肷��D
				//
				// �I�����͈ȉ��̒ʂ�ł���D
				//   ='A': all N rows of V'�i�s�� V �� N�~N �ɂȂ�j
				//   ='S': the first min(m, n) rows of V'�i�s�� V' �ɂ͉E���كx�N�g�����i�[�����j
				//   ='O': 'S' �w�莞�̍s�� V' �̓��e�ōs�� X ���㏑�������
				//   ='N': �E���كx�N�g�����v�Z���Ȃ�
				
				integer m = x_row;	// M: the number of rows of matrix X
				integer n = x_col;	// N: the number of columns of matrix X
				
				pin_ptr<doublereal> a = &X[0];
				// ��
				// input : [m(=lda), n]�̍s�� X
				// output:
				//   if jobu == 'O' then
				//     the first min(m, n) columns of U�i�z�� a �͍����كx�N�g���ŏ㏑�������j
				//
				//   if jobvt == 'O' then 
				//     the first min(m, n) rows of V'�i�z�� a �͉E���كx�N�g���ŏ㏑�������j
				//
				//   if jobu != 'O' and jobvt != 'O' then�i".ne." �Ƃ��邯�� "not equal" �̈Ӗ�����ȁH�j
				//     �z�� a �̓��e�͔j�󂳂��
				

				integer lda = m;	// the leading dimension of the array X.�i���H�j�C�Ƃ肠���� max(1, M) <= lda
				
				const integer min_mn = Math::Min(m, n);
				
				// S�̗̈���m�ہC���̎��_�ł̓x�N�g���C��őΊp�s�񉻂���D
				S = gcnew array<doublereal>(min_mn);
				pin_ptr<doublereal> s = &S[0];	// the singular values of X (sorted: s[i] >= s[i+1])
				
				
				integer ldu = Math::Max(1, m);
				// �K�� 1 <= ldu �𖞂����K�v������D
				// if jobu == ('A' or 'S') then M <= ldu;
				
				u_row = ldu;
				u_col = min_mn;	// jobu == 'S' �Ȃ̂�
				U = gcnew array<doublereal>(u_row * u_col);
				// [ldu, m]         if jobu == 'A' (U contains the M-by-M orthogonal matrix U)
				// [ldu, min(m, n)] if jobu == 'S' (U contains the first min(m, n) columns of U)
				// U is not referenced, if jobu == ('N' or 'O')
				pin_ptr<doublereal> u = &U[0];
				
				
				integer ldvt = Math::Max(1, min_mn);	// jobvt == 'S' �Ȃ̂�
				// �K�� 1 <= ldvt �𖞂����K�v������D
				// if jobvt == 'A' then n <= ldvt
				// if jobvt == 'S' then min(m, n) <= ldvt
								
				// ��[ldvt, n] �� V' �̃T�C�Y�ł��邽�߁CV �̃T�C�Y�� [n, ldvt] �ƂȂ�D
				v_row = n;
				v_col = ldvt;
				array<doublereal>^ VT = gcnew array<doublereal>(v_col * v_row);
				// [ldvt, n] if jobvt == ('A' or 'S')
				// U is not referenced, if jobvt == ('N' or 'O')
				pin_ptr<doublereal> vt = &VT[0];
				
				
				integer lwork = Math::Max(3 * min_mn + Math::Max(m, n), 5 * min_mn);
				// �K�� 1 <= lwork �𖞂����K�v������D
				// MAX(3*MIN(M,N)+MAX(M,N),5*MIN(M,N)) <= LWORK
				// For good performance, LWORK should generally be larger.
				doublereal* work = new doublereal[lwork];
				// if info == 0 then work[0] returns the optimal LWORK
				// if info >  0 then work[2:min(m, n)] contains the unconverged superdiagonal elements of an upper bidiagonal matrix B whose diagonal is in S (not necessarily sorted).
				//                   B satisfies A = U * B * VT, so it has the same singular values as A, and singular vectors related by U and VT.
				// �i�Ӗ�j
				// work[2:min(m, n)] �́C�������Ȃ����� ���d�Ίp�s�� B �� 1��Ίp�v�f ���܂ށi�s��B�̑Ίp�v�f�� S �ɂ���j�D
				// �s�� B �́CA = U * B * VT �𖞂����D�����āCA �Ɠ������ْl������, and singular vectors related by U and VT.
				
				integer info = 0;
				// if info == 0: ����I��
				// if info <  0: -info �Ԗڂ̈����́C�Ԉ�����l�����Ă���D
				// if info >  0: �v�Z�͎������Ȃ������iwork���Q�Ɓj�D
				
				int ret;
				try
				{
					// CLAPACK�̓��ْl�������[�`��
					ret = dgesvd_(&jobu, &jobvt, &m, &n, a, &lda, s, u, &ldu, vt, &ldvt, work, &lwork, &info);
					
					if(info == 0)
					{
						// �i�[����
						array<doublereal>^ dS = gcnew array<doublereal>(S->Length * S->Length);
						for(int i=0, j=0; j < S->Length; i += (S->Length + 1), ++j)
						{
							dS[i] = S[j];
						}
						s_row = S->Length;
						s_col = S->Length;
						S = dS;

						// U�͂��̂܂�
						// VT��]�u����V��
						V = gcnew array<doublereal>(v_row * v_col);
						for(int r=0; r<v_row; ++r)
						{
							for(int c=0; c<v_col; ++c)
							{
								V[r + c*v_row] = VT[c + r*v_col];
							}
						}
					}
					else
					{
						U = S = V = nullptr;
						u_row = u_col = s_row = s_col = v_row = v_col = 0;
						if(info < 0)
						{
							throw gcnew LapackArgumentException(info, "dgesvd_ argument");
						}
						else
						{
							throw gcnew LapackResultException(info, "dgesvd_ result");
						}
					}
				}
				finally
				{
					delete[] work; work = nullptr;
				}
				return ret;
			}


			/// <summary>
			/// <para>�ŗL�l����</para>
			/// <para>�v�Z���ꂽ�ŗL�x�N�g���́C�傫���i���[�N���b�h�m�����j�� 1 �ɋK�i������Ă���D</para>
			/// </summary>
			/// <param name="X">�ŗL�l���������s��i�v�Z�̉ߒ��ŏ㏑�������j</param>
			/// <param name="x_row">�s�� <paramref name="X"/> �̍s��</param>
			/// <param name="x_col">�s�� <paramref name="X"/> �̗�</param>
			/// <param name="r_evals">�ŗL�l�̎�����</param>
			/// <param name="i_evals">�ŗL�l�̋�����</param>
			/// <param name="r_evecs">�ŗL�x�N�g���̎�����</param>
			/// <param name="i_evecs">�ŗL�x�N�g���̋�����</param>
			/// <returns>��� 0 ���Ԃ��Ă���D</returns>
			/// <remarks>
			/// <para>�Ή�����CLAPACK�֐��iCLAPACK/BLAS/SRC/dgeev.c�j</para>
			/// <code>
			/// int dgeev_(char *jobvl, char *jobvr, integer *n,
			///            doublereal *a, integer *lda,
			///            doublereal *wr, doublereal *wi,
			///            doublereal *vl, integer *ldvl, doublereal *vr, integer *ldvr,
			///            doublereal *work, integer *lwork, integer *info);
			/// </code>
			/// </remarks>
			static int dgeev(array<doublereal>^ X, int x_row, int x_col,
							 array<doublereal>^% r_evals,
							 array<doublereal>^% i_evals,
							 array< array<doublereal>^ >^% r_evecs,
							 array< array<doublereal>^ >^% i_evecs )
			{
				char jobvl = 'N';
				// ���ŗL�x�N�g����
				//   if jobvl == 'V' then �v�Z����
				//   if jobvl == 'N' then �v�Z���Ȃ�

				char jobvr = 'V';
				// �E�ŗL�x�N�g����
				//   if jobvr == 'V' then �v�Z����
				//   if jobvr == 'N' then �v�Z���Ȃ�

				integer n = x_col;
				// �s�� X �̑傫���iN�~N�Ȃ̂ŁC�Е������ł悢�j
				
				integer lda = n;
				// the leading dimension of the array A. lda >= max(1, N).

				pin_ptr<doublereal> a = &X[0];
				// [lda, n] N�~N �̍s�� X
				// �z�� a �i�s�� X�j�́C�v�Z�̉ߒ��ŏ㏑�������D			
				
				doublereal* wr = new doublereal[n];
				// �v�Z���ꂽ�ŗL�l�̎���������D
				
				doublereal* wi = new doublereal[n];
				// �v�Z���ꂽ�ŗL�l�̋���������D
				// ���f����΂̏ꍇ�́Cwi[j]=(���l)�Cwi[j+1]=(���l) �̏��ɓ���D
				

				/*
				 * �����ŗL�x�N�g���͌v�Z���Ȃ�
				 */
				

				integer ldvl = 1;
				// �K�� 1 <= ldvl �𖞂����K�v������D
				// if jobvl == 'V' then N <= ldvl

				doublereal* vl = nullptr;
				// vl is not referenced, because jobvl == 'N'.
				
				/*
				 * ���E�ŗL�x�N�g���͌v�Z����
				 */

				integer ldvr = n;
				// �K�� 1 <= ldvr �𖞂����K�v������D
				// if jobvr == 'V' then N <= ldvr

				doublereal* vr = new doublereal[ldvr * n];
				// if jobvr == 'V' then �E�ŗL�x�N�g���� vr �̊e��ɁC�ŗL�l�Ɠ��������Ŋi�[�����D
				// if jobvr == 'N' then vr is not referenced.
				//
				// wi[i] �� 0 �łȂ��Ƃ��i�ŗL�l�����f����΂̂Ƃ��j
				//     ����          ����
				// vr[   0, i]  vr[   0, i+1]
				// vr[   1, i]  vr[   1, i+1]
				//            ...
				// vr[ldvr, i]  vr[ldvr, i+1]
				// �̂悤�Ɋi�[�����D
				// �i���\�[�X�̃R�����g���j
				// If the j-th eigenvalue is real, then v(j) = VR(:,j), the j-th column of VR.
				// If the j-th and (j+1)-st eigenvalues form a complex conjugate pair, then v(j) = VR(:,j) + i*VR(:,j+1) and v(j+1) = VR(:,j) - i*VR(:,j+1).
				
				
				//
				// ���̑�

				integer lwork = 4*n;
				// max(1, 3*N) <= lwork
				// if jobvl == 'V' or jobvr == 'V' then 4*N <= lwork
				// �ǂ��p�t�H�[�}���X�𓾂邽�߂ɁC���̏ꍇ lwork �͑傫�����ׂ����D
				doublereal* work = new doublereal[lwork];
				// if info == 0 then work[0] returns the optimal lwork.
				
				integer info = 0;
				// if info == 0 then ����I��
				// if info <  0 then -info �Ԗڂ̈����̒l���Ԉ���Ă���D
				// if info >  0 then QR�A���S���Y���́C�S�Ă̌ŗL�l���v�Z�ł��Ȃ������D
				//                   �ŗL�x�N�g���͌v�Z����Ă��Ȃ��D
				//                   wr[info+1:N] �� wl[info+1:N] �ɂ́C���������ŗL�l���܂܂�Ă���D
				
				
				int ret;
				try
				{
					// CLAPACK���[�`��
					ret = dgeev_(&jobvl, &jobvr, &n, a, &lda, wr, wi, vl, &ldvl, vr, &ldvr, work, &lwork, &info);

					if(info == 0)
					{
						// �ŗL�l���i�[
						r_evals = gcnew array<doublereal>(n);
						i_evals = gcnew array<doublereal>(n);
						for(int i=0; i<n; ++i)
						{
							r_evals[i] = wr[i];
							i_evals[i] = wi[i];
						}
						
						// �ŗL�x�N�g�����i�[
						r_evecs = gcnew array< array<doublereal>^ >(n);
						i_evecs = gcnew array< array<doublereal>^ >(n);						
						for(int i=0; i<n; ++i)
						{
							if(wi[i] <= 0.0)
							{
								// �ʏ�̊i�[����
								r_evecs[i] = gcnew array<doublereal>(ldvr);
								i_evecs[i] = gcnew array<doublereal>(ldvr);

								for(int j=0; j<ldvr; ++j)
								{
									r_evecs[i][j] = vr[i*ldvr + j];
									i_evecs[i][j] = 0.0;
								}
							}
							else
							{
                                // ����������ꍇ
							    array<doublereal>^ realvec1 = gcnew array<doublereal>(ldvr);
							    array<doublereal>^ realvec2 = gcnew array<doublereal>(ldvr);
							    array<doublereal>^ imgyvec1 = gcnew array<doublereal>(ldvr);
							    array<doublereal>^ imgyvec2 = gcnew array<doublereal>(ldvr);
							    for(int j=0; j<ldvr; ++j)
							    {
								    realvec1[j] =  vr[ i   *ldvr + j];
								    realvec2[j] =  vr[ i   *ldvr + j];
								    imgyvec1[j] =  vr[(i+1)*ldvr + j];
								    imgyvec2[j] = -vr[(i+1)*ldvr + j];
							    }
							    r_evecs[i  ] = realvec1;
							    r_evecs[i+1] = realvec2;
							    i_evecs[i  ] = imgyvec1;
							    i_evecs[i+1] = imgyvec2;
							    ++i;	// 2�񂸂Q�Ƃ���̂ŁC�����ŃJ�E���g�A�b�v
                            }
						}// end for i
					}// end if info == 0
					else
					{
						if(info < 0)
						{
							throw gcnew LapackArgumentException(info, "dgeev_ argument");
						}
						else
						{
							throw gcnew LapackResultException(info, "dgeev_ result");
						}
					}
				}
				finally
				{
					// unmanaged code �̌�n��
					delete[] wr; wr = nullptr;
					delete[] wi; wi = nullptr;
					delete[] vr; vr = nullptr;
					delete[] work; work = nullptr;
				}
				return ret;
			}


			/// <summary>
			/// �ŗL�l�����i�ŗL�l�̂݁j
			/// <para>
			/// �ڍׂ� �ŗL�l�E�ŗL�x�N�g���̗������v�Z���� dgeev ���\�b�h ���Q�Ƃ���D
			/// </para>
			/// </summary>
			/// <param name="X">�ŗL�l���������s��i�v�Z�ߒ��ŏ㏑�������j</param>
			/// <param name="x_row">�s�� <paramref name="X"/> �̍s��</param>
			/// <param name="x_col">�s�� <paramref name="X"/> �̗�</param>
			/// <param name="r_evals">�ŗL�l�̎�����</param>
			/// <param name="i_evals">�ŗL�l�̋�����</param>
			/// <returns>��� 0 ���Ԃ��Ă���D</returns>
			static int dgeev(array<doublereal>^ X, int x_row, int x_col,
							 array<doublereal>^% r_evals,
							 array<doublereal>^% i_evals )
			{
				// �ŗL�x�N�g���͌v�Z���Ȃ�
				char jobvl = 'N';
				char jobvr = 'N';

				integer n = x_col;
				
				integer lda = n;
				pin_ptr<doublereal> a = &X[0];
				
				doublereal* wr = new doublereal[n];
				doublereal* wi = new doublereal[n];
				
				integer ldvl = 1;
				doublereal* vl = nullptr;

				integer ldvr = 1;
				doublereal* vr = nullptr;
				
				integer lwork = 3*n;
				doublereal* work = new doublereal[lwork];
				
				integer info = 0;
				
				int ret;
				try
				{
					ret = dgeev_(&jobvl, &jobvr, &n, a, &lda, wr, wi, vl, &ldvl, vr, &ldvr, work, &lwork, &info);
					
					if(info == 0)
					{
						// �ŗL�l���i�[
						r_evals = gcnew array<doublereal>(n);
						i_evals = gcnew array<doublereal>(n);
						
						for(int i=0; i<n; ++i)
						{
							r_evals[i] = wr[i];
							i_evals[i] = wi[i];
						}
					}
					else
					{
						if(info < 0)
						{
							throw gcnew LapackArgumentException(info, "dgeev_ argument");
						}
						else
						{
							throw gcnew LapackResultException(info, "dgeev_ result");
						}
					}
				}
				finally
				{
					// unmanaged code �̌�n��
					delete[] wr; wr = nullptr;
					delete[] wi; wi = nullptr;
					delete[] work; work = nullptr;
				}

				return ret;
			}
			
			
			/// <summary>
			/// LU ����
			/// </summary>
			/// <param name="X">LU �����̑ΏۂƂȂ�s�� (�����̌��ʂ��i�[�����)</param>
			/// <param name="x_row">�s�� <paramref name="X" /> �̍s��</param>
			/// <param name="x_col">�s�� <paramref name="X" /> �̗�</param>
			/// <param name="p">�u����� (�s�� X �� i �s (i �� 0 �n�܂�) �́Cp[i] �s�ƌ������ꂽ���Ƃ�����)</param>
			/// <returns>
			/// ����I���̎��� 0 ���Ԃ��Ă���D
			/// LU ������̍s�� U �̑Ίp�v�f�� 0 �v�f���܂܂��ꍇ�́C
			/// ���̈ʒu +1 (0 �͐���I������������) ��Ԃ�(U[i-1, i-1] == 0.0)�D
			/// </returns>
			/// <remarks>
			/// <para>LU �������� (�s���F<paramref name="x_row" />�C�񐔁F<paramref name="x_col" />) �ɂ� L �� U ���i�[����Ă���D
            /// L �̑Ίp�v�f (�S�� 1) �͊i�[����Ă��Ȃ��D</para>
			/// <para>�Ή�����CLAPACK�֐� (CLAPACK/BLAS/SRC/dgetrf.c)</para>
			/// <code>
			/// int
			/// dgetrf_(integer *m, integer *n, doublereal *a,
			///         integer *lda, integer *ipiv, integer *info)
			/// </code>
			/// </remarks>
			static int dgetrf(array<doublereal>^ X, int x_row, int x_col, array<int>^% p)
			{
				// the size of matrix X is M-by-N.
				integer m = x_row;	// the number of rows of the matrix X
				integer n = x_col;	// the number of columns of the matrix X
				
				pin_ptr<doublereal> a = &X[0];
				// M�~N �̍s�� X
				// ���s��́CA = P*L*U �� L �� U ���i�[�����D
				// �������CL �̑Ίp�v�f�i�e�v�f�̒l�F1�j�͊i�[����Ȃ��D
				
				integer lda = m;	// max(1, M) <= lda
				
				integer* piv = new integer[Math::Min(m, n)];
				// the pivot indices [1, min(M, N)]
				// �s�� X �� i �Ԗڂ̍s�́Cpiv[i] �Ԗڂ̍s�ƌ������ꂽ���Ƃ������D
				
				integer info;
				// if info == 0 then ����I��
				// if info <  0 then -info �Ԗڂ̈����̒l���Ԉ���Ă���
				// if info >  0 then info = i, U[i, i] �̓[���ł��邱�Ƃ������D
				//                   �������C�������̂͊������Ă���D
				//                   U �͓��قł���C�A���������������ߒ��Ŏg�p����� 0�� ����������D

				int ret;
				try
				{
					ret = dgetrf_(&m, &n, a, &lda, piv, &info);
					
					if(info == 0)
					{
						// ����I��
						p = gcnew array<int>(Math::Min(m, n));
						for(int i=0; i< p->Length; ++i)
						{
							p[i] = piv[i] - 1;	// 1�n�܂� �� 0�n�܂� �ɂ���
						}
					}
					else
					{
						if(info < 0)
						{
							throw gcnew LapackArgumentException(info, "dgetrf_ argument");
						}
						else
						{
							ret = info;	// 0�v�f�̈ʒu+1��Ԃ�
						}
					}
				}
				finally
				{
					delete[] piv; piv = nullptr;
				}
				return ret;
			}
		};
	}// end namespace claw
}// end namespace krdlab

