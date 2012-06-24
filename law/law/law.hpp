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
			/// ベクトル要素の総和を計算する．
			/// </summary>
			/// <param name="arr">配列 (変更されない)</param>
			/// <returns>総和</returns>
            static doublereal dasum(array<doublereal>^ arr)
            {
                integer N = arr->Length;
                pin_ptr<doublereal> pX = &arr[0];
                integer incX = 1;
                return dasum_(&N, pX, &incX);
            }

			/// <summary>
			/// ベクトルのノルムを計算する．
			/// </summary>
			/// <param name="arr">ノルムを知りたい配列 (変更されない)</param>
			/// <returns>ノルム</returns>
			static double dnrm2(array<doublereal>^ arr)
			{
				integer N = arr->Length;
				pin_ptr<doublereal> pX = &arr[0];
				integer incX = 1;
				return dnrm2_(&N, pX, &incX);
			}


			/// <summary>
			/// 配列の各要素をスカラ倍する．
			/// </summary>
			/// <param name="ret">結果配列 (内部で適切なサイズが割り当てられる)</param>
			/// <param name="arr">スカラ倍される配列</param>
			/// <param name="d">スカラ</param>
			/// <returns>CLAPACK/BLAS/SRC/dscal.c によれば，常に 0 が返ってくる．</returns>
			static int dscal_r(array<doublereal>^% ret, array<doublereal>^ arr, double d)
			{
				integer S = arr->Length;
                
                ret = gcnew array<doublereal>(S);
                System::Array::Copy(arr, ret, S);

				doublereal N = d;

				pin_ptr<doublereal> pX = &ret[0]; // arr の上書きを防ぎ，ret に結果を入れるため
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
			/// z := ax + y を計算する．
			/// </summary>
			/// <param name="z">結果配列 (内部で適切なサイズが割り当てられる)</param>
			/// <param name="a">スカラ</param>
			/// <param name="x">配列 (変更されない)</param>
			/// <param name="y">配列 (変更されない)</param>
			/// <returns>CLAPACK/BLAS/SRC/daxpy.c によれば，常に 0 が返ってくる．</returns>
			static int daxpy_r(array<doublereal>^% z, doublereal a, array<doublereal>^ x, array<doublereal>^ y)
			{				
				z = gcnew array<double>(y->Length);
				System::Array::Copy(y, z, y->Length);

				integer N = x->Length;

				pin_ptr<doublereal> pX = &x[0];
				integer incx = 1;
				
				pin_ptr<doublereal> pY = &z[0];  // y の上書きを防ぎ，z に結果を入れるため
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
			/// ベクトルの内積をとる．
			/// </summary>
			/// <param name="v1">配列 (変更されない)</param>
			/// <param name="v2">配列 (変更されない)</param>
			/// <returns><paramref name="v1"/> と <paramref name="v2"/> の内積</returns>
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

				// 結果 Matrix のサイズ
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
				
				char trans = 'N';	// A は転置しない

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
			/// dgesv_(integer *n, integer *nrhs,
			///        doublereal *a, integer *lda, integer *ipiv,
			///        doublereal *b, integer *ldb, integer *info);
			/// </code>
			/// <para>dgesv_ 関数の内部では LU 分解が使用されている．</para>
			/// </remarks>
			static int dgesv(array<doublereal>^% X, int% x_row, int% x_col,
							 array<doublereal>^  A, int  a_row, int  a_col,
							 array<doublereal>^  B, int  b_row, int  b_col)
			{
				integer n    = a_row;	// input: 連立一次方程式の式数．正方行列[A]の次数(n≧0)． 
				integer nrhs = b_col;	// input: 行列BのColumn数
				
				pin_ptr<doublereal> a = &A[0];
				// 配列形式は a[lda×n]
				//   input : n行n列の係数行列[A]
				//   output: LU分解後の行列[L]と行列[U]．ただし，行列[L]の単位対角要素は格納されない．
				//           A=[P]*[L]*[U]であり，[P]は行と列を入れ替える操作に対応する置換行列と呼ばれ，0か1が格納される．
				
				integer lda = n;
				// input: 行列Aの第一次元(のメモリ格納数)．lda≧max(1,n)であり，通常は lda==n で良い．

				integer* ipiv = new integer[n];
				// output: 大きさnの配列．置換行列[P]を定義する軸選択用添え字．

				pin_ptr<doublereal> b = &B[0];
				// input/output: 配列形式はb[ldb×nrhs]．通常はnrhsが1なので，配列形式がb[ldb]となる．
				// input : b[ldb×nrhs]の配列形式をした右辺行列{B}．
				// output: info==0 の場合に，b[ldb×nrhs]形式の解行列{X}が格納される．

				integer ldb = b_row;
				// input: 配列bの第一次元(のメモリ格納数)．ldb≧max(1,n)であり，通常は ldb==n で良い．

				integer info = 1;
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
				
				integer m = x_row;	// M: the number of rows of matrix X
				integer n = x_col;	// N: the number of columns of matrix X
				
				pin_ptr<doublereal> a = &X[0];
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
				

				integer lda = m;	// the leading dimension of the array X.（←？），とりあえず max(1, M) <= lda
				
				const integer min_mn = Math::Min(m, n);
				
				// Sの領域を確保，この時点ではベクトル，後で対角行列化する．
				S = gcnew array<doublereal>(min_mn);
				pin_ptr<doublereal> s = &S[0];	// the singular values of X (sorted: s[i] >= s[i+1])
				
				
				integer ldu = Math::Max(1, m);
				// 必ず 1 <= ldu を満たす必要がある．
				// if jobu == ('A' or 'S') then M <= ldu;
				
				u_row = ldu;
				u_col = min_mn;	// jobu == 'S' なので
				U = gcnew array<doublereal>(u_row * u_col);
				// [ldu, m]         if jobu == 'A' (U contains the M-by-M orthogonal matrix U)
				// [ldu, min(m, n)] if jobu == 'S' (U contains the first min(m, n) columns of U)
				// U is not referenced, if jobu == ('N' or 'O')
				pin_ptr<doublereal> u = &U[0];
				
				
				integer ldvt = Math::Max(1, min_mn);	// jobvt == 'S' なので
				// 必ず 1 <= ldvt を満たす必要がある．
				// if jobvt == 'A' then n <= ldvt
				// if jobvt == 'S' then min(m, n) <= ldvt
								
				// ↓[ldvt, n] は V' のサイズであるため，V のサイズは [n, ldvt] となる．
				v_row = n;
				v_col = ldvt;
				array<doublereal>^ VT = gcnew array<doublereal>(v_col * v_row);
				// [ldvt, n] if jobvt == ('A' or 'S')
				// U is not referenced, if jobvt == ('N' or 'O')
				pin_ptr<doublereal> vt = &VT[0];
				
				
				integer lwork = Math::Max(3 * min_mn + Math::Max(m, n), 5 * min_mn);
				// 必ず 1 <= lwork を満たす必要がある．
				// MAX(3*MIN(M,N)+MAX(M,N),5*MIN(M,N)) <= LWORK
				// For good performance, LWORK should generally be larger.
				doublereal* work = new doublereal[lwork];
				// if info == 0 then work[0] returns the optimal LWORK
				// if info >  0 then work[2:min(m, n)] contains the unconverged superdiagonal elements of an upper bidiagonal matrix B whose diagonal is in S (not necessarily sorted).
				//                   B satisfies A = U * B * VT, so it has the same singular values as A, and singular vectors related by U and VT.
				// （意訳）
				// work[2:min(m, n)] は，収束しなかった 上二重対角行列 B の 1つ上対角要素 を含む（行列Bの対角要素は S にある）．
				// 行列 B は，A = U * B * VT を満たす．そして，A と同じ特異値を持ち, and singular vectors related by U and VT.
				
				integer info = 0;
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
						array<doublereal>^ dS = gcnew array<doublereal>(S->Length * S->Length);
						for(int i=0, j=0; j < S->Length; i += (S->Length + 1), ++j)
						{
							dS[i] = S[j];
						}
						s_row = S->Length;
						s_col = S->Length;
						S = dS;

						// Uはそのまま
						// VTを転置してVに
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
				// 左固有ベクトルを
				//   if jobvl == 'V' then 計算する
				//   if jobvl == 'N' then 計算しない

				char jobvr = 'V';
				// 右固有ベクトルを
				//   if jobvr == 'V' then 計算する
				//   if jobvr == 'N' then 計算しない

				integer n = x_col;
				// 行列 X の大きさ（N×Nなので，片方だけでよい）
				
				integer lda = n;
				// the leading dimension of the array A. lda >= max(1, N).

				pin_ptr<doublereal> a = &X[0];
				// [lda, n] N×N の行列 X
				// 配列 a （行列 X）は，計算の過程で上書きされる．			
				
				doublereal* wr = new doublereal[n];
				// 計算された固有値の実部が入る．
				
				doublereal* wi = new doublereal[n];
				// 計算された固有値の虚部が入る．
				// 複素共役対の場合は，wi[j]=(正値)，wi[j+1]=(負値) の順に入る．
				

				/*
				 * ※左固有ベクトルは計算しない
				 */
				

				integer ldvl = 1;
				// 必ず 1 <= ldvl を満たす必要がある．
				// if jobvl == 'V' then N <= ldvl

				doublereal* vl = nullptr;
				// vl is not referenced, because jobvl == 'N'.
				
				/*
				 * ※右固有ベクトルは計算する
				 */

				integer ldvr = n;
				// 必ず 1 <= ldvr を満たす必要がある．
				// if jobvr == 'V' then N <= ldvr

				doublereal* vr = new doublereal[ldvr * n];
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

				integer lwork = 4*n;
				// max(1, 3*N) <= lwork
				// if jobvl == 'V' or jobvr == 'V' then 4*N <= lwork
				// 良いパフォーマンスを得るために，大抵の場合 lwork は大きくすべきだ．
				doublereal* work = new doublereal[lwork];
				// if info == 0 then work[0] returns the optimal lwork.
				
				integer info = 0;
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
						r_evals = gcnew array<doublereal>(n);
						i_evals = gcnew array<doublereal>(n);
						for(int i=0; i<n; ++i)
						{
							r_evals[i] = wr[i];
							i_evals[i] = wi[i];
						}
						
						// 固有ベクトルを格納
						r_evecs = gcnew array< array<doublereal>^ >(n);
						i_evecs = gcnew array< array<doublereal>^ >(n);						
						for(int i=0; i<n; ++i)
						{
							if(wi[i] <= 0.0)
							{
								// 通常の格納処理
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
                                // 虚部がある場合
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
			static int dgeev(array<doublereal>^ X, int x_row, int x_col,
							 array<doublereal>^% r_evals,
							 array<doublereal>^% i_evals )
			{
				// 固有ベクトルは計算しない
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
						// 固有値を格納
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
				// M×N の行列 X
				// 実行後は，A = P*L*U の L と U が格納される．
				// ただし，L の対角要素（各要素の値：1）は格納されない．
				
				integer lda = m;	// max(1, M) <= lda
				
				integer* piv = new integer[Math::Min(m, n)];
				// the pivot indices [1, min(M, N)]
				// 行列 X の i 番目の行は，piv[i] 番目の行と交換されたことを示す．
				
				integer info;
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

