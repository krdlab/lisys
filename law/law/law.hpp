// law.hpp
#pragma once
using namespace System;

namespace krdlab {
	namespace law {
		using namespace exception;

		/// <summary>
		/// CLAPACK を CLR 上で利用するためのラッパークラス
		/// </summary>
		public ref class func
		{
		public:
			/// <summary>
			/// ベクトル要素の絶対値の総和を計算する．
			/// </summary>
			/// <param name="arr">配列 (変更されない)</param>
			/// <returns>絶対値の総和</returns>
            static double dasum(array<double>^ arr)
            {
                int N = arr->Length;
                pin_ptr<double> pX = &arr[0];
                int incX = 1;
                return dasum_(&N, pX, &incX);
            }

			/// <summary>
			/// ベクトルのノルムを計算する．
			/// </summary>
			/// <param name="arr">ノルムを知りたい配列 (変更されない)</param>
			/// <returns>ノルム</returns>
			static double dnrm2(array<double>^ arr)
			{
				int N = arr->Length;
				pin_ptr<double> pX = &arr[0];
				int incX = 1;
				return dnrm2_(&N, pX, &incX);
			}


			/// <summary>
			/// 配列の各要素をスカラ倍する．
			/// </summary>
			/// <param name="ret">結果配列 (内部で適切なサイズが割り当てられる)</param>
			/// <param name="arr">スカラ倍される配列</param>
			/// <param name="d">スカラ</param>
			/// <returns>CLAPACK/BLAS/SRC/dscal.c によれば，常に 0 が返ってくる．</returns>
			static int dscal_r(array<double>^% ret, array<double>^ arr, double d)
			{
				int S = arr->Length;
                
                ret = gcnew array<double>(S);
                System::Array::Copy(arr, ret, S);

				double N = d;

				pin_ptr<double> pX = &ret[0]; // arr の上書きを防ぎ，ret に結果を入れるため
				int incX = 1;

				return dscal_(&S, &N, pX, &incX);
			}

            /// <summary>
            /// calculate: d*x (d: scala, x: vector)
            /// </summary>
			static int dscal(array<double>^ x, double d)
			{
				int N = x->Length;
				double a = d;
				pin_ptr<double> pX = &x[0];
				int incX = 1;
				return dscal_(&N, &a, pX, &incX);
			}

			/// <summary>
			/// z := ax + y を計算する．
			/// </summary>
			/// <param name="z">結果配列 (内部で適切なサイズが割り当てられる)</param>
			/// <param name="a">スカラ</param>
			/// <param name="x">配列 (変更されない)</param>
			/// <param name="y">配列 (変更されない)</param>
			/// <returns>CLAPACK/BLAS/SRC/daxpy.c によれば，常に 0 が返ってくる．</returns>
			static int daxpy_r(array<double>^% z, double a, array<double>^ x, array<double>^ y)
			{				
				z = gcnew array<double>(y->Length);
				System::Array::Copy(y, z, y->Length);

				int N = x->Length;

				pin_ptr<double> pX = &x[0];
				int incx = 1;
				
				pin_ptr<double> pY = &z[0];  // y の上書きを防ぎ，z に結果を入れるため
				int incy = 1;
				
				return daxpy_(&N, &a, pX, &incx, pY, &incy);
			}

            /// <summary>
            /// calculate: a*x + y -> y (a: scala, x: vector, y: vector)
            /// </summary>
			static int daxpy(double a, array<double>^ x, array<double>^ y)
			{
				int N = x->Length;
				pin_ptr<double> pX = &x[0];
				int incx = 1;
				pin_ptr<double> pY = &y[0];
				int incy = 1;
				return daxpy_(&N, &a, pX, &incx, pY, &incy);
			}

			/// <summary>
			/// ベクトルの内積をとる．
			/// </summary>
			/// <param name="v1">配列 (変更されない)</param>
			/// <param name="v2">配列 (変更されない)</param>
			/// <returns><paramref name="v1"/> と <paramref name="v2"/> の内積</returns>
			static double ddot(array<double>^ v1, array<double>^ v2)
			{
				int N = v1->Length;
				pin_ptr<double> pX = &v1[0];
				int incX = 1;
				pin_ptr<double> pY = &v2[0];
				int incY = 1;
				return ddot_(&N, pX, &incX, pY, &incY);
			}


			/// <summary>
			/// 行列どうしの乗算を行う．
			/// </summary>
			/// <param name="C">結果の行列 (内部で適切なサイズのメモリが割り当てられる)</param>
			/// <param name="A">行列 (変更されない)</param>
			/// <param name="B">行列 (変更されない)</param>
			/// <returns>常に 0 が返ってくる．</returns>
			/// <exception cref="ArgumentException"><paramref name="A"/> の列数と <paramref name="B"/> の行数とが一致しない場合</exception>
			/// <remarks>
			/// <para>対応するCLAPACK関数 (CLAPACK/BLAS/SRC/dgemm.c)</para>
			/// <code>
			///	int dgemm_(char* transA, char* transB, int* M, int* N, int* K,
			///	            double* alpha,
			///	            double* A, int* lda,
			///	            double* B, int* ldb,
			///	            double* beta,
			///	            double* C, int* ldc);
			/// </code>
			/// </remarks>
			static int dgemm(array<double>^% C, int% c_row, int% c_col,
							 array<double>^  A, int  a_row, int  a_col,
							 array<double>^  B, int  b_row, int  b_col)
			{
				if(a_col != b_row)
				{
					throw gcnew ArgumentException("a_col != b_row");
				}

				// 結果 Matrix のサイズ
				c_row = a_row;
				c_col = b_col;

				C = gcnew array<double>(c_row*c_col);
				
				char transA = 'N';
				char transB = 'N';
				int M = a_row;
				int N = b_col;
				int K = a_col; // or b_row
				
				double alpha = 1.0;
				
				pin_ptr<double> pA = &A[0];
				int lda = std::max(1, M); // transA == 'N' then
				
				pin_ptr<double> pB = &B[0];
				int ldb = std::max(1, K); // transB == 'N' then
				
				double beta = 0.0;
				
				pin_ptr<double> pC = &C[0];
				int ldc = std::max(1, M);
				
                // C := alpha * A * B + beta * C
				return dgemm_( &transA, &transB, &M, &N, &K,
								&alpha,
								pA, &lda,
								pB, &ldb,
								&beta,
								pC, &ldc );
			}


			/// <summary>
			/// Y = AX を計算する．
			/// </summary>
			/// <param name="Y">結果の列ベクトル (内部で適切なサイズのメモリが割り当てられる)</param>
			/// <param name="A">行列 (変更されない)</param>
			/// <param name="X">列ベクトル (変更されない)</param>
			/// <returns>常に 0 が返ってくる．</returns>
			/// <exception cref="ArgumentException"><paramref name="A"/> の列数と <paramref name="X"/> のサイズとが一致しない場合</exception>
			/// <remarks>
			/// <para>対応するCLAPACK関数 (CLAPACK/BLAS/SRC/dgemv.c)</para>
			/// <code>
			/// int
			/// dgemv_(char* trans, int* M, int* N,
			///	       double* alpha,
			///	       double* A, int* lda,
			///	       double* X, int* incX,
			///	       double* beta,
			///	       double* Y, int* incY);
			/// </code>
			/// </remarks>
			static int dgemv(array<double>^% Y,
							 array<double>^ A, int a_row, int a_col,
							 array<double>^ X)
			{
				if(a_col != X->Length)
				{
					throw gcnew ArgumentException("a_col != X->Length");
				}

				Y = gcnew array<double>(a_row);
				
				char trans = 'N';	// A は転置しない

				int M = a_row;
				int N = a_col;
				
				double alpha = 1.0;
				
				pin_ptr<double> pA = &A[0];
				int lda = a_row;  // := max( 1, m )
				
				pin_ptr<double> pX = &X[0];
				int incX = 1;
				
				double beta = 0.0;
				
				pin_ptr<double> pY = &Y[0];
				int incY = 1;

				// Y := alpha * A + beta * Y
				return dgemv_(&trans,
                              &M, &N,
                              &alpha, pA, &lda,
                                      pX, &incX,
                              &beta , pY, &incY);
			}

			/// <summary>
			/// Y = XA を計算する．
			/// </summary>
			/// <param name="Y">結果の行ベクトル (内部で適切なサイズのメモリが割り当てられる)</param>
			/// <param name="X">行ベクトル (変更されない)</param>
			/// <param name="A">行列（変更されない）</param>
			/// <returns>常に 0 が返ってくる．</returns>
			/// <exception cref="ArgumentException"><paramref name="X"/> のサイズと <paramref name="A"/> の行数とが一致しない場合</exception>
			/// <remarks>
			/// <para>対応するCLAPACK関数 (CLAPACK/BLAS/SRC/dgemv.c)</para>
			/// <code>
			/// int
			/// dgemv_(char* trans, int* M, int* N,
			///	       double* alpha,
			///	       double* A, int* lda,
			///	       double* X, int* incX,
			///	       double* beta,
			///	       double* Y, int* incY);
			/// </code>
			/// </remarks>
			static int dgemv(array<double>^% Y,
							 array<double>^  X,
							 array<double>^  A, int a_row, int a_col)
			{
				if(X->Length != a_row)
				{
					throw gcnew ArgumentException("X->Length != a_row");
				}

				Y = gcnew array<double>(a_col);
				
				char trans = 'T';
				int M = a_row;
				int N = a_col;
				
				double alpha = 1.0;
				
				pin_ptr<double> pA   = &A[0];
				int     lda = a_row;
				
				pin_ptr<double> pX    = &X[0];
				int     incX = 1;
				
				double beta = 0.0;
				
				pin_ptr<double> pY    = &Y[0];
				int     incY = 1;

				// transpose(A) * X = Y => transpose(Y) == X * A (Yは配列なので，結果を返す際に転置する必要はない)
				return dgemv_( &trans, &M, &N, &alpha, pA, &lda,
										pX, &incX, &beta, pY, &incY );
			}


			/// <summary>
			/// <para>A * X = B を解く (X が解)．</para>
			/// <para>A は n×n の行列，X と B は n×nrhs の行列である．</para>
			/// </summary>
			/// <param name="X"><c>A * X = B</c> の解である X が格納される (実際には B と同じオブジェクトを指す)</param>
			/// <param name="x_row">行列 X の行数が格納される (<c>== <paramref name="b_row"/></c>)</param>
			/// <param name="x_col">行列 X の列数が格納される (<c>== <paramref name="b_col"/></c>)</param>
			/// <param name="A">係数行列 (LU分解の結果である P*L*U に書き換えられる．P*L*Uについては<see cref="dgetrf"/>を参照)</param>
			/// <param name="a_row">行列Aの行数</param>
			/// <param name="a_col">行列Aの列数</param>
			/// <param name="B">行列 B (内部のCLAPACK関数により X の値が格納される)</param>
			/// <param name="b_row">行列Bの行数</param>
			/// <param name="b_col">行列Bの列数</param>
			/// <returns>常に 0 が返ってくる．</returns>
			/// <exception cref="LapackArgumentException">内部で dgesv_関数に渡された引数に問題があった場合</exception>
			/// <exception cref="LapackResultException">行列 A の LU 分解において，U[i, i] が 0 となってしまった場合 (解を求めることができない)</exception>
			/// <remarks>
			/// <para>対応するCLAPACK関数 (CLAPACK/BLAS/SRC/dgesv.c)</para>
			/// <code>
			/// int
			/// dgesv_(int *n, int *nrhs,
			///        double *a, int *lda, int *ipiv,
			///        double *b, int *ldb, int *info);
			/// </code>
			/// <para>dgesv_ 関数の内部では LU 分解が使用されている．</para>
			/// </remarks>
			static int dgesv(array<double>^% X, int% x_row, int% x_col,
							 array<double>^  A, int  a_row, int  a_col,
							 array<double>^  B, int  b_row, int  b_col)
			{
				int n    = a_row;	// input: 連立一次方程式の式数．正方行列[A]の次数(n≧0)． 
				int nrhs = b_col;	// input: 行列BのColumn数
				
				pin_ptr<double> a = &A[0];
				// 配列形式は a[lda×n]
				//   input : n行n列の係数行列[A]
				//   output: LU分解後の行列[L]と行列[U]．ただし，行列[L]の単位対角要素は格納されない．
				//           A=[P]*[L]*[U]であり，[P]は行と列を入れ替える操作に対応する置換行列と呼ばれ，0か1が格納される．
				
				int lda = n;
				// input: 行列Aの第一次元(のメモリ格納数)．lda≧max(1,n)であり，通常は lda==n で良い．

				int* ipiv = new int[n];
				// output: 大きさnの配列．置換行列[P]を定義する軸選択用添え字．

				pin_ptr<double> b = &B[0];
				// input/output: 配列形式はb[ldb×nrhs]．通常はnrhsが1なので，配列形式がb[ldb]となる．
				// input : b[ldb×nrhs]の配列形式をした右辺行列{B}．
				// output: info==0 の場合に，b[ldb×nrhs]形式の解行列{X}が格納される．

				int ldb = b_row;
				// input: 配列bの第一次元(のメモリ格納数)．ldb≧max(1,n)であり，通常は ldb==n で良い．

				int info = 1;
				// output:
				// info==0: 正常終了
				// info < 0: info==-i ならば，i番目の引数の値が間違っていることを示す．
				// 0 < info <N-1: 固有ベクトルは計算されていないことを示す．
				// info > N: LAPACK内で問題が生じたことを示す．

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
			/// 特異値分解
			/// </summary>
			/// <param name="X">特異値分解される行列 (内容は破壊される)</param>
			/// <param name="x_row">行列 <paramref name="X"/> の行数</param>
			/// <param name="x_col">行列 <paramref name="X"/> の列数</param>
			/// <param name="U">各特異値に対応する左特異ベクトルが，列ごとに入っている行列</param>
			/// <param name="u_row">行列 <paramref name="U"/> の行数</param>
			/// <param name="u_col">行列 <paramref name="U"/> の列数</param>
			/// <param name="S">対角要素が特異値である行列 (特異値は降順に格納されている S[i, i] >= S[i+1, i+1])</param>
			/// <param name="s_row">行列 <paramref name="S"/> の行数</param>
			/// <param name="s_col">行列 <paramref name="S"/> の列数</param>
			/// <param name="V">各特異値に対応する右特異ベクトルが，列ごとに入っている行列</param>
			/// <param name="v_row">行列 <paramref name="V"/> の行数</param>
			/// <param name="v_col">行列 <paramref name="V"/> の列数</param>
			/// <returns>常に 0 が返ってくる．</returns>
			/// <exception cref="LapackArgumentException">内部で dgesvd_関数に渡された引数に問題があった場合</exception>
			/// <exception cref="LapackResultException">計算が収束しなかった場合</exception>
			/// <remarks>
			/// <para>対応するCLAPACK関数 (CLAPACK/BLAS/SRC/dgesvd.c)</para>
			/// <code>
			/// int dgesvd_(char *jobu, char *jobvt, int *m, int *n, 
			///	            double *a, int *lda,
			///             double *s, double *u, int *ldu, double *vt, int *ldvt,
			///             double *work, int *lwork, 
			///	            int *info);
			/// </code>
			/// </remarks>
			static int dgesvd(  array<double>^  X, int  x_row, int  x_col,
								array<double>^% U, int% u_row, int% u_col,
								array<double>^% S, int% s_row, int% s_col,
								array<double>^% V, int% v_row, int% v_col)
			{
				char jobu = 'S';
				// ↑行列 U の列（左特異値ベクトル）をどこまで（「全て」 or 「部分」）計算するのかを指定する．
				// 
				// 選択肢は以下の通りである．
				//   = 'A': all M columns of U（行列 U は M×M になる）
				//   = 'S': the first min(m, n) columns of U（行列 U には左特異ベクトルが格納される）
				//   = 'O': 'S' 指定時の行列 U の内容で行列 X が上書きされる
				//   = 'N': 左特異ベクトルを計算しない

				char jobvt = 'S';
				// ↑行列 V'（転置された V）の行ベクトル（行列 V の列ベクトル：右特異ベクトル）を
				//   どこまで（「全て」or「部分」）計算するのかを指定する．
				//
				// 選択肢は以下の通りである．
				//   ='A': all N rows of V'（行列 V は N×N になる）
				//   ='S': the first min(m, n) rows of V'（行列 V' には右特異ベクトルが格納される）
				//   ='O': 'S' 指定時の行列 V' の内容で行列 X が上書きされる
				//   ='N': 右特異ベクトルを計算しない
				
				int m = x_row;	// M: the number of rows of matrix X
				int n = x_col;	// N: the number of columns of matrix X
				
				pin_ptr<double> a = &X[0];
				// ↑
				// input : [m(=lda), n]の行列 X
				// output:
				//   if jobu == 'O' then
				//     the first min(m, n) columns of U（配列 a は左特異ベクトルで上書きされる）
				//
				//   if jobvt == 'O' then 
				//     the first min(m, n) rows of V'（配列 a は右特異ベクトルで上書きされる）
				//
				//   if jobu != 'O' and jobvt != 'O' then（".ne." とあるけど "not equal" の意味だよな？）
				//     配列 a の内容は破壊される
				

				int lda = m;	// the leading dimension of the array X.（←？），とりあえず max(1, M) <= lda
				
				const int min_mn = Math::Min(m, n);
				
				// Sの領域を確保，この時点ではベクトル，後で対角行列化する．
				S = gcnew array<double>(min_mn);
				pin_ptr<double> s = &S[0];	// the singular values of X (sorted: s[i] >= s[i+1])
				
				
				int ldu = Math::Max(1, m);
				// 必ず 1 <= ldu を満たす必要がある．
				// if jobu == ('A' or 'S') then M <= ldu;
				
				u_row = ldu;
				u_col = min_mn;	// jobu == 'S' なので
				U = gcnew array<double>(u_row * u_col);
				// [ldu, m]         if jobu == 'A' (U contains the M-by-M orthogonal matrix U)
				// [ldu, min(m, n)] if jobu == 'S' (U contains the first min(m, n) columns of U)
				// U is not referenced, if jobu == ('N' or 'O')
				pin_ptr<double> u = &U[0];
				
				
				int ldvt = Math::Max(1, min_mn);	// jobvt == 'S' なので
				// 必ず 1 <= ldvt を満たす必要がある．
				// if jobvt == 'A' then n <= ldvt
				// if jobvt == 'S' then min(m, n) <= ldvt
								
				// ↓[ldvt, n] は V' のサイズであるため，V のサイズは [n, ldvt] となる．
				v_row = n;
				v_col = ldvt;
				array<double>^ VT = gcnew array<double>(v_col * v_row);
				// [ldvt, n] if jobvt == ('A' or 'S')
				// U is not referenced, if jobvt == ('N' or 'O')
				pin_ptr<double> vt = &VT[0];
				
				
				int lwork = Math::Max(3 * min_mn + Math::Max(m, n), 5 * min_mn);
				// 必ず 1 <= lwork を満たす必要がある．
				// MAX(3*MIN(M,N)+MAX(M,N),5*MIN(M,N)) <= LWORK
				// For good performance, LWORK should generally be larger.
				double* work = new double[lwork];
				// if info == 0 then work[0] returns the optimal LWORK
				// if info >  0 then work[2:min(m, n)] contains the unconverged superdiagonal elements of an upper bidiagonal matrix B whose diagonal is in S (not necessarily sorted).
				//                   B satisfies A = U * B * VT, so it has the same singular values as A, and singular vectors related by U and VT.
				// （意訳）
				// work[2:min(m, n)] は，収束しなかった 上二重対角行列 B の 1つ上対角要素 を含む（行列Bの対角要素は S にある）．
				// 行列 B は，A = U * B * VT を満たす．そして，A と同じ特異値を持ち, and singular vectors related by U and VT.
				
				int info = 0;
				// if info == 0: 正常終了
				// if info <  0: -info 番目の引数は，間違った値持っている．
				// if info >  0: 計算は収束しなかった（workを参照）．
				
				int ret;
				try
				{
					// CLAPACKの特異値分解ルーチン
					ret = dgesvd_(&jobu, &jobvt, &m, &n, a, &lda, s, u, &ldu, vt, &ldvt, work, &lwork, &info);
					
					if(info == 0)
					{
						// 格納する
						array<double>^ dS = gcnew array<double>(S->Length * S->Length);
						for(int i=0, j=0; j < S->Length; i += (S->Length + 1), ++j)
						{
							dS[i] = S[j];
						}
						s_row = S->Length;
						s_col = S->Length;
						S = dS;

						// Uはそのまま
						// VTを転置してVに
						V = gcnew array<double>(v_row * v_col);
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
			/// <para>固有値分解</para>
			/// <para>計算された固有ベクトルは，大きさ（ユークリッドノルム）が 1 に規格化されている．</para>
			/// </summary>
			/// <param name="X">固有値分解される行列（計算の過程で上書きされる）</param>
			/// <param name="x_row">行列 <paramref name="X"/> の行数</param>
			/// <param name="x_col">行列 <paramref name="X"/> の列数</param>
			/// <param name="r_evals">固有値の実数部</param>
			/// <param name="i_evals">固有値の虚数部</param>
			/// <param name="r_evecs">固有ベクトルの実数部</param>
			/// <param name="i_evecs">固有ベクトルの虚数部</param>
			/// <returns>常に 0 が返ってくる．</returns>
			/// <remarks>
			/// <para>対応するCLAPACK関数（CLAPACK/BLAS/SRC/dgeev.c）</para>
			/// <code>
			/// int dgeev_(char *jobvl, char *jobvr, int *n,
			///            double *a, int *lda,
			///            double *wr, double *wi,
			///            double *vl, int *ldvl, double *vr, int *ldvr,
			///            double *work, int *lwork, int *info);
			/// </code>
			/// </remarks>
			static int dgeev(array<double>^ X, int x_row, int x_col,
							 array<double>^% r_evals,
							 array<double>^% i_evals,
							 array< array<double>^ >^% r_evecs,
							 array< array<double>^ >^% i_evecs )
			{
				char jobvl = 'N';
				// 左固有ベクトルを
				//   if jobvl == 'V' then 計算する
				//   if jobvl == 'N' then 計算しない

				char jobvr = 'V';
				// 右固有ベクトルを
				//   if jobvr == 'V' then 計算する
				//   if jobvr == 'N' then 計算しない

				int n = x_col;
				// 行列 X の大きさ（N×Nなので，片方だけでよい）
				
				int lda = n;
				// the leading dimension of the array A. lda >= max(1, N).

				pin_ptr<double> a = &X[0];
				// [lda, n] N×N の行列 X
				// 配列 a （行列 X）は，計算の過程で上書きされる．			
				
				double* wr = new double[n];
				// 計算された固有値の実部が入る．
				
				double* wi = new double[n];
				// 計算された固有値の虚部が入る．
				// 複素共役対の場合は，wi[j]=(正値)，wi[j+1]=(負値) の順に入る．
				

				/*
				 * ※左固有ベクトルは計算しない
				 */
				

				int ldvl = 1;
				// 必ず 1 <= ldvl を満たす必要がある．
				// if jobvl == 'V' then N <= ldvl

				double* vl = nullptr;
				// vl is not referenced, because jobvl == 'N'.
				
				/*
				 * ※右固有ベクトルは計算する
				 */

				int ldvr = n;
				// 必ず 1 <= ldvr を満たす必要がある．
				// if jobvr == 'V' then N <= ldvr

				double* vr = new double[ldvr * n];
				// if jobvr == 'V' then 右固有ベクトルが vr の各列に，固有値と同じ順序で格納される．
				// if jobvr == 'N' then vr is not referenced.
				//
				// wi[i] が 0 でないとき（固有値が複素共役対のとき）
				//     実部          虚部
				// vr[   0, i]  vr[   0, i+1]
				// vr[   1, i]  vr[   1, i+1]
				//            ...
				// vr[ldvr, i]  vr[ldvr, i+1]
				// のように格納される．
				// （↓ソースのコメントより）
				// If the j-th eigenvalue is real, then v(j) = VR(:,j), the j-th column of VR.
				// If the j-th and (j+1)-st eigenvalues form a complex conjugate pair, then v(j) = VR(:,j) + i*VR(:,j+1) and v(j+1) = VR(:,j) - i*VR(:,j+1).
				
				
				//
				// その他

				int lwork = 4*n;
				// max(1, 3*N) <= lwork
				// if jobvl == 'V' or jobvr == 'V' then 4*N <= lwork
				// 良いパフォーマンスを得るために，大抵の場合 lwork は大きくすべきだ．
				double* work = new double[lwork];
				// if info == 0 then work[0] returns the optimal lwork.
				
				int info = 0;
				// if info == 0 then 正常終了
				// if info <  0 then -info 番目の引数の値が間違っている．
				// if info >  0 then QRアルゴリズムは，全ての固有値を計算できなかった．
				//                   固有ベクトルは計算されていない．
				//                   wr[info+1:N] と wl[info+1:N] には，収束した固有値が含まれている．
				
				
				int ret;
				try
				{
					// CLAPACKルーチン
					ret = dgeev_(&jobvl, &jobvr, &n, a, &lda, wr, wi, vl, &ldvl, vr, &ldvr, work, &lwork, &info);

					if(info == 0)
					{
						// 固有値を格納
						r_evals = gcnew array<double>(n);
						i_evals = gcnew array<double>(n);
						for(int i=0; i<n; ++i)
						{
							r_evals[i] = wr[i];
							i_evals[i] = wi[i];
						}
						
						// 固有ベクトルを格納
						r_evecs = gcnew array< array<double>^ >(n);
						i_evecs = gcnew array< array<double>^ >(n);						
						for(int i=0; i<n; ++i)
						{
							if(wi[i] <= 0.0)
							{
								// 通常の格納処理
								r_evecs[i] = gcnew array<double>(ldvr);
								i_evecs[i] = gcnew array<double>(ldvr);

								for(int j=0; j<ldvr; ++j)
								{
									r_evecs[i][j] = vr[i*ldvr + j];
									i_evecs[i][j] = 0.0;
								}
							}
							else
							{
                                // 虚部がある場合
							    array<double>^ realvec1 = gcnew array<double>(ldvr);
							    array<double>^ realvec2 = gcnew array<double>(ldvr);
							    array<double>^ imgyvec1 = gcnew array<double>(ldvr);
							    array<double>^ imgyvec2 = gcnew array<double>(ldvr);
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
							    ++i;	// 2列ずつ参照するので，ここでカウントアップ
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
					// unmanaged code の後始末
					delete[] wr; wr = nullptr;
					delete[] wi; wi = nullptr;
					delete[] vr; vr = nullptr;
					delete[] work; work = nullptr;
				}
				return ret;
			}


			/// <summary>
			/// 固有値分解（固有値のみ）
			/// <para>
			/// 詳細は 固有値・固有ベクトルの両方を計算する dgeev メソッド を参照せよ．
			/// </para>
			/// </summary>
			/// <param name="X">固有値分解される行列（計算過程で上書きされる）</param>
			/// <param name="x_row">行列 <paramref name="X"/> の行数</param>
			/// <param name="x_col">行列 <paramref name="X"/> の列数</param>
			/// <param name="r_evals">固有値の実数部</param>
			/// <param name="i_evals">固有値の虚数部</param>
			/// <returns>常に 0 が返ってくる．</returns>
			static int dgeev(array<double>^ X, int x_row, int x_col,
							 array<double>^% r_evals,
							 array<double>^% i_evals )
			{
				// 固有ベクトルは計算しない
				char jobvl = 'N';
				char jobvr = 'N';

				int n = x_col;
				
				int lda = n;
				pin_ptr<double> a = &X[0];
				
				double* wr = new double[n];
				double* wi = new double[n];
				
				int ldvl = 1;
				double* vl = nullptr;

				int ldvr = 1;
				double* vr = nullptr;
				
				int lwork = 3*n;
				double* work = new double[lwork];
				
				int info = 0;
				
				int ret;
				try
				{
					ret = dgeev_(&jobvl, &jobvr, &n, a, &lda, wr, wi, vl, &ldvl, vr, &ldvr, work, &lwork, &info);
					
					if(info == 0)
					{
						// 固有値を格納
						r_evals = gcnew array<double>(n);
						i_evals = gcnew array<double>(n);
						
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
					// unmanaged code の後始末
					delete[] wr; wr = nullptr;
					delete[] wi; wi = nullptr;
					delete[] work; work = nullptr;
				}

				return ret;
			}
			
			
			/// <summary>
			/// LU 分解
			/// </summary>
			/// <param name="X">LU 分解の対象となる行列 (分解の結果が格納される)</param>
			/// <param name="x_row">行列 <paramref name="X" /> の行数</param>
			/// <param name="x_col">行列 <paramref name="X" /> の列数</param>
			/// <param name="p">置換情報 (行列 X の i 行 (i は 0 始まり) は，p[i] 行と交換されたことを示す)</param>
			/// <returns>
			/// 正常終了の時は 0 が返ってくる．
			/// LU 分解後の行列 U の対角要素に 0 要素が含まれる場合は，
			/// その位置 +1 (0 は正常終了を示すため) を返す(U[i-1, i-1] == 0.0)．
			/// </returns>
			/// <remarks>
			/// <para>LU 分解結果 (行数：<paramref name="x_row" />，列数：<paramref name="x_col" />) には L と U が格納されている．
            /// L の対角要素 (全て 1) は格納されていない．</para>
			/// <para>対応するCLAPACK関数 (CLAPACK/BLAS/SRC/dgetrf.c)</para>
			/// <code>
			/// int
			/// dgetrf_(int *m, int *n, double *a,
			///         int *lda, int *ipiv, int *info)
			/// </code>
			/// </remarks>
			static int dgetrf(array<double>^ X, int x_row, int x_col, array<int>^% p)
			{
				// the size of matrix X is M-by-N.
				int m = x_row;	// the number of rows of the matrix X
				int n = x_col;	// the number of columns of the matrix X
				
				pin_ptr<double> a = &X[0];
				// M×N の行列 X
				// 実行後は，A = P*L*U の L と U が格納される．
				// ただし，L の対角要素（各要素の値：1）は格納されない．
				
				int lda = m;	// max(1, M) <= lda
				
				int* piv = new int[Math::Min(m, n)];
				// the pivot indices [1, min(M, N)]
				// 行列 X の i 番目の行は，piv[i] 番目の行と交換されたことを示す．
				
				int info;
				// if info == 0 then 正常終了
				// if info <  0 then -info 番目の引数の値が間違っている
				// if info >  0 then info = i, U[i, i] はゼロであることを示す．
				//                   ただし，分解自体は完了している．
				//                   U は特異であり，連立方程式を解く過程で使用すると 0割 が発生する．

				int ret;
				try
				{
					ret = dgetrf_(&m, &n, a, &lda, piv, &info);
					
					if(info == 0)
					{
						// 正常終了
						p = gcnew array<int>(Math::Min(m, n));
						for(int i=0; i< p->Length; ++i)
						{
							p[i] = piv[i] - 1;	// 1始まり を 0始まり にする
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
							ret = info;	// 0要素の位置+1を返す
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

