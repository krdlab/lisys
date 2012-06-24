#pragma once

extern "C" {
    // ref: http://www.netlib.org/lapack/explore-html/

    // the sum of absolute values
    double dasum_(const int *n, const double *dx, const int *incx);

    // dnrm2 := sqrt(x' * x)
    double dnrm2_(const int *n, const double *x, const int *incx);

    // scale a vector (dx) by a constant (da)
    int dscal_(const int *n, const double *da, double *dx, const int *incx);

    // a*x + y
    int daxpy_(const int *n,
               const double *da, const double *dx, const int *incx,
                                       double *dy, const int *incy);

    // dot product
    double ddot_(const int *n,
                 const double *dx, const int *incx,
                 const double *dy, const int *incy);

    // alpha*a*x + beta*y ('a' is a matrix)
    int dgemv_(const char *trans, const int *m /*rows*/, const int *n /*cols*/,
               const double *alpha,
               const double *a, const int *lda,
               const double *x, const int *incx, 
               const double *beta,
                     double *y, const int *incy);

    // A*X = B
    // a: [n, n], b: [n, nrhs]
    int dgesv_(const int *n, const int *nrhs,
                     double *a, const int *lda,
                     int *ipiv,
                     double *b, const int *ldb,
                     int *info);

    // compute SVD
    // a: [m, n]
    int dgesvd_(const char *jobu, const char *jobvt,
                const int *m, const int *n,
                double *a, const int *lda,
                double *s,
                double *u, const int *ldu,
                double *vt, const int *ldvt,
                double *work, const int *lwork,
                int *info);

    // compute the eigenvalues, the left and/or right eigenvectors
    // a: [n, n]
    // wr: the real parts of the computed eigenvalues
    // wi: the imaginary parts of the computed eigenvalues
    int dgeev_(const char *jobvl, const char *jobvr, const int *n,
               double *a, const int *lda,
               double *wr, double *wi,
               double *vl, const int *ldvl,
               double *vr, const int *ldvr,
               double *work, const int *lwork,
               int *info);

    // LU factorization
    // a: [m, n]
    int dgetrf_(const int *m, const int *n,
                double *a, const int *lda,
                int *ipiv,
                int *info);

    // c := alpha*op(a)*op(b) + beta*c
    // op(x) = x or op(x) = x'
    // op(a): [m, k]
    // op(b): [k, n]
    // c:     [m, n]
    int dgemm_(const char *transa, const char *transb, const int *m, const int *n, const int *k,
               const double *alpha,
               const double *a, const int *lda, 
               const double *b, const int *ldb,
               const double *beta,
                     double *c, const int *ldc);
}
